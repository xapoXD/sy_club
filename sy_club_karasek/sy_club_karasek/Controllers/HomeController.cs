using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using club.soundyard.web.Data;
using club.soundyard.web.Models;
using System.Diagnostics;

using club.soundyard.web.Services;
using MimeKit;
using MailKit.Net.Smtp;
using NToastNotify;

namespace club.soundyard.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        private readonly IMailService _mailService;

        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IMailService _MailService, IToastNotification toastNotification)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mailService = _MailService;
            _toastNotification = toastNotification;
        }

        [Authorize(Roles = "Admin,Manager,Member,Visitor")]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<ApplicationRole>>();

                foreach (var roleName in roles)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                       
                        Console.WriteLine("Agreement: " + role.Agreement);
                        ViewData["Agreement"] = role.Agreement;
                    }
                    else
                    {
                        ViewData["Agreement"] = "Role not found";
                    }
                }
            }
            else
            {
     
                ViewData["Agreement"] = "User not signed in";
            }


            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Report()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Administration()
        {
            return View();
        }


        public IActionResult AnonymousRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnonymousRegistrationAsync(AnonymousRegistration model)
        {
            if (ModelState.IsValid)
            {
                //Console.WriteLine(model.Name);
                //Console.WriteLine(model.Surname);
                //Console.WriteLine(model.Email);
                MailData maildata = new MailData(); 
                
                maildata.EmailToId = model.Email;
                maildata.EmailToName = "Soundyard club";
                maildata.EmailSubject = "Verification of email address - Soundyard club";
                
                    
                // create user if he isnt in db
                if (await _userManager.FindByEmailAsync(model.Email) == null)
                {
                    var user = new IdentityUser();
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.EmailConfirmed = false;

                    await _userManager.CreateAsync(user); 
                    await _userManager.AddToRoleAsync(user, "Visitor");

                    // generate token for user
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // Home - as controller name    
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Home", new { token, email = user.Email }, Request.Scheme);
                    var messageBody = "Confirmation link:\n" + confirmationLink;
                    maildata.EmailBody = messageBody;

                    bool success = SendMail(maildata);
                    Console.WriteLine("sent? : " + success);
                    if (success)
                    {
                        _toastNotification.AddSuccessToastMessage("Verification email has been sent successfully");
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("Email SMTP error");
                    }

                    return RedirectToAction("Index");
                }

                _toastNotification.AddErrorToastMessage("This email addres is already registered!");
                return View(model);

            }
            return View(model);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
			{
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true); //sign in verified user
                    _toastNotification.AddSuccessToastMessage("Your email address was activated and you have been logged in successfully");
                    return RedirectToAction("Index");
                    // return Ok(new { Status = "Success", Message = "Email Verified Successfully" });                
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Internal Server Error" });

        }


        private bool SendMail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }

        [HttpPost]
        public ActionResult RedirectTologinPage()
        {
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}