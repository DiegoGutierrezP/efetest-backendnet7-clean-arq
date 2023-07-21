using EfeTest.Backend.Application.Interfaces.Repositories;
using EfeTest.Backend.Application.Interfaces.Services;
using EfeTest.Backend.Infrastructure.Context;
using EfeTest.Backend.Infrastructure.Repositories;
using EfeTest.Backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Infrastructure.Extensions
{
    public static class IServiceColletionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMappings();
            services.AddDbContext(configuration);
            services.AddRepositories();
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database"), options => options.EnableRetryOnFailure()));

            //services.AddDbContext<AppDbContext>(options =>
            //   options.UseSqlServer("data source=DESKTOP-OHFUPDQ; Initial Catalog=EfeTest;Integrated Security=True;MultipleActiveResultSets=true",
            //       options => options.EnableRetryOnFailure()));

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("EfeTest.Backend.Api")));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddHttpClient();


            services
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<IJwtService, JwtService>()
                .AddTransient<IOrderService, OrderService>();
                //.AddTransient<IUserRepository, UserRepository>()
                //.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        }
    }
}
