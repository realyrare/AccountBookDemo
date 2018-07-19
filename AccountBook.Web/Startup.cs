﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AccountBook.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

      
        public void ConfigureServices(IServiceCollection services)
        {
            //利用ASP.NET Core的依赖注入容器系统，通过请求获取IHttpContextAccessor接口，我们拥有模拟使用HttpContext.Current这样API的可能性。但是因为IHttpContextAccessor接口默认不是由依赖注入进行实例管理的。我们先要将它注册到ServiceCollection中：
           // services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var sqlconnectionStr = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<AppliactionDbContext>(options => options.UseSqlServer(sqlconnectionStr));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
