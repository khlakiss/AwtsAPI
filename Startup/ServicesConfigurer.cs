using AWTS.BLL.Repositories.AboutUsRepo;
using AWTS.BLL.Repositories.AdvertisingRepo;
using AWTS.BLL.Repositories.ContactRepo;
using AWTS.BLL.Repositories.EmailRepo;
using AWTS.BLL.Repositories.HomeRepo;
using AWTS.BLL.Repositories.OurMissionRepo;
using AWTS.BLL.Repositories.ServiceRepo;
using AWTS.BLL.Repositories.UserRepo;
using AWTS.BLL.Repositories.WhatsAppRepo;
using AWTS.BLL.Repositories.WhatWeDoRepo;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwtsAPI.Startup
{
    public class ServicesConfigurer
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAboutUsRepository, AboutUsRepository>();
            services.AddScoped<IAdvertisingRepository, AdvertisingRepository>();
            services.AddScoped<IContactRepo, ContactRepo>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IOurMissionRepository, OurMissionRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IWhatsAppRepository, WhatsAppRepository>();
            services.AddScoped<IWhatWeDoRepository, WhatWeDoRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
