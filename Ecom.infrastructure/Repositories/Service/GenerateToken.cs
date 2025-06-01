using Ecom.Core.Entities;
using Ecom.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories.Service
{
    public class GenerateToken : IGenerateToken
    {
        public Task<string> GetAndCreateToken(AppUser user)
        {

        }
    }
}
