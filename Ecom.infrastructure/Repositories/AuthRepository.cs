using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
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
        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
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
            AppUser user = new AppUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
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
                return "done!";
            }
            return "please check your email and password, something went wrong";
        }

    }
}
