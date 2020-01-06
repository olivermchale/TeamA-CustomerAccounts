using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamA.CustomerAccounts.Data;
using TeamA.CustomerAccounts.Repository;
using TeamA.CustomerAccounts.Services;
using Polly;

namespace TeamA.CustomerAccounts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<AccountsDb>(options => options.UseSqlServer(
                Configuration.GetConnectionString("CustomerAccountsDb")));

            services.AddScoped<IAccountsService, AccountsRepository>();
            if(Environment.IsDevelopment())
            {
                services.AddScoped<IOrdersService, OrdersServiceFake>();
            } else
            {
                services.AddScoped<IOrdersService, OrdersService>();
                var ordersServiceAddress = Configuration.GetValue<Uri>("OrdersServiceUri");
                services.AddHttpClient<IOrdersService, OrdersService>(c =>
                {
                    c.BaseAddress = ordersServiceAddress;
                    c.DefaultRequestHeaders.Accept.Clear();
                    c.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                }).AddTransientHttpErrorPolicy(p =>
                    p.OrResult(r => !r.IsSuccessStatusCode)
                        .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(Math.Pow(2, retry))))
                        .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));

                
            }

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
            options.Audience = "staff_api"; ;
            options.Authority = "https://threeamigosauth.azurewebsites.net/";
            });

                        services.AddAuthorization(options =>
                        {
                            options.AddPolicy("Customer", builder =>
                            {
                                builder.RequireClaim("role", "Customer", "Admin", "Staff");
                            });
                            options.AddPolicy("Staff", builder =>
                            {
                                builder.RequireClaim("role", "Staff", "Admin");
                            });
                            options.AddPolicy("Admin", builder =>
                            {
                                builder.RequireClaim("role", "Admin");
                            });
                        });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
