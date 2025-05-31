using Ecom.Core.DTOs;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
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

        public AuthRepository(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
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

            return "done";
        }
    }
}
