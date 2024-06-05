# ASP .Net Core - Web Application Documentation

## Overview

This web application is an ASP.NET MVC 6 application designed as per the given requirements. It features a Bootstrap 4 Admin & Dashboard theme, Forms-Based Authentication (FBA) with Asp.Net Identity, and a role-based system extended with an "Agreement" property.

## Features

1. **ASP.NET MVC 5 Application**
   - Created from an ASP.NET MVC 5 template.

2. **Bootstrap 4 Admin & Dashboard Theme**
   - Implemented Admin & Dashboard Bootstrap 4 theme from [Bootstrap Dashboard Example](https://getbootstrap.com/docs/4.3/examples/dashboard/).

3. **Forms-Based Authentication (FBA)**
   - User authentication data is stored in a SQL database using Asp.Net Identity.
   - Local SQL Express database is used.
   - Database name: `sy_club`.

4. **Role-Based System**
   - Standard role system implemented.
   - Extended `Role` entity with an `Agreement` property of type `nvarchar(50)`.

5. **Dashboard Page**
   - Displays the value of `Agreement` for the currently logged-in user.

6. **SMTP Configuration**
   - Configured to use SMTP server `212.71.162.103` without authentication (server does not support relay).

7. **Anonymous Registration Form**
   - Includes fields for First Name, Last Name, and Email Address.
   - Sends an activation email to verify the email address after registration.

8. **Internal Pages (Accessible after login)**
   - **Dashboard:** Created the page, content to be added.
   - **Report:** Created the page, content to be added.
   - **Administration:** Created the page, content to be added.

## Project Structure

- **Namespace:** `club.soundyard.web`
- **Database:** Local SQL Express, named `sy_club`

## SMTP Configuration

- **Server:** 212.71.162.103
- **Authentication:** None (server does not support relay)

## Database Schema

The application uses a local SQL Express database named `sy_club`. It leverages Asp.Net Identity for authentication and stores user roles with an additional `Agreement` property.

## Registration Process

1. User fills out the registration form with the following fields:
   - First Name
   - Last Name
   - Email Address
2. An activation email is sent to the provided email address for verification.

## Admin & Dashboard Theme

The application uses the Bootstrap 4 Admin & Dashboard theme. The theme provides a responsive and modern user interface for the admin and dashboard pages.

## Roles and Agreement Property

- The standard role system is extended with an additional `Agreement` property (`nvarchar(50)`).
- On the Dashboard page, the `Agreement` value for the currently logged-in user is displayed.

## Pages

- **Dashboard:** Initial setup completed, content to be added.
- **Report:** Initial setup completed, content to be added.
- **Administration:** Initial setup completed, content to be added.

## Usage

1. Register a new user using the registration form.
2. Check your email for the activation link and verify your email address.
3. Log in to access the internal pages (Dashboard, Report, Administration).

## Acknowledgements

- Bootstrap 4 for the Admin & Dashboard theme.
- ASP.NET Identity for authentication and role management.
