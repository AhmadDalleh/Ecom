using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagementService;
        //private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;
        public ICategoryRepository CategoryRepository { get; }

        public IPhotoRepository PhotoRepository { get; }
        public IProductRepository ProductRepository { get; }

        public ICustomerBasketRepositry CustomerBasketRepository {  get; }

        public IAuth Auth {  get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService, 
            IAuth auth, UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken token) //IConnectionMultiplexer redis, )
        {
            _context = context;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            //this.redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
            CategoryRepository = new CategoryRepository(_context);
            PhotoRepository = new PhotoRepository(_context);
            ProductRepository = new ProductRepository(_context, _mapper, _imageManagementService);
            Auth = new AuthRepository(userManager, emailService, signInManager,token);
            //CustomerBasketRepository = new CustomerBasketRepository(redis);

        }
    }
}
