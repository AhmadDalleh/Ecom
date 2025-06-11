using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class AuthRepository:IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generateToken;
        private readonly AppDbContext context;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken, AppDbContext context)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generateToken = generateToken;
            this.context = context;
        }

        public async Task<string> RegisterAsync(RegisterDTO registerDTO )
        {
            if (registerDTO == null) 
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "this User Name is already registered";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null) 
            {
                return "this User Email is already registered";
            }
            AppUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName
            };


            var result = await  userManager.CreateAsync(user,registerDTO.Password);
            if (result.Succeeded is not true) 
            {
                return result.Errors.ToList()[0].Description;
            }
            // Send Active Email
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your email,click on submit to active your email");
            return "done";
        }
        public async Task SendEmail(string email,string code,string component , string subject,string message)
        {
            var result = new EmailDTO(email,
                "ahmeddalleh8@gmail.com",
                subject,
                EmailStringBody.send(email, code, component, message));

           await emailService.SendEmail(result);
        }
        public async Task<string> LoginAsync(LoginDTO login)
        {
            if(login == null)
            {
                return null;
            }
            var findUser= await userManager.FindByEmailAsync(login.Email);
            if (!findUser.EmailConfirmed)
            {
                string token=await userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please active your email,click on submit to active your email");
                return "Please confirm your email first, we have send activate to your E-mail";
            }
            var result = await signInManager.CheckPasswordSignInAsync(findUser, login.Password, true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(findUser);
            }
            return "please check your email and password, something went wrong";
        }

        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if(findUser is null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "Reset-Password", "Reset Password", " click on button to reset your password");
            return true;
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var findUser=await userManager.FindByEmailAsync(resetPassword.Email);
            if(findUser is null)
            {
                return null;
            }
            var result=await userManager.ResetPasswordAsync(findUser,resetPassword.Token,resetPassword.Password);

            if (result.Succeeded)
            {
                return "Password changed successfully"; 
            }
            return result.Errors.ToList()[0].Description;
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO accountDTO)
        {
            var findUser=await userManager.FindByEmailAsync(accountDTO.Email);
            if(findUser is null)
            {
                return false;
            }

            var result = await userManager.ConfirmEmailAsync(findUser,accountDTO.Token);
            if (result.Succeeded)
                return true;
            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Please active your email,click on submit to active your email");
            return false;

        }
    }
}
