using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//連接DB
using Portfolio.Data;
using Microsoft.EntityFrameworkCore;

//cookie認證、授權
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Portfolio
{
    public class Startup
    {
        //Startup 建構子
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            #region DB連線字串
            //透過 DbContextOptions 物件上的方法，來連結 appsetting.json 中，設定好的 SQL server connection string
            services.AddDbContext<PortfolioContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HeisenbergHsueh_Portfolio_DB")));
            #endregion

            #region Cookie驗證
            //註冊cookie驗證的服務
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                //設定cookie名稱
                options.Cookie.Name = "HeisenbergHsueh_Cookie";
                //browser 會限制 cookie 只能經由 http protocol 來存取
                options.Cookie.HttpOnly = true;
                //設定登入頁面，未登入時自動導入到登入頁面
                options.LoginPath = new PathString("/LoginSystem/Login");
                //設定登出頁面(此頁面可以省略)
                options.LogoutPath = new PathString("/LoginSystem/Logout");
                //當登入者不具有當下頁面的訪問權限時，會自動轉跳到此Forbidden頁面
                options.AccessDeniedPath = new PathString("/LoginSystem/Forbidden");
                //設定登入有效時間(單位:分鐘)
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //SlidingExpiration = true，表示說，如果cookie當前的時間已經超過開始時間與結束時間的一半，則重新頒發cookie
                //options.SlidingExpiration = true;
            });
            #endregion

            #region 授權
            services.AddAuthorization(options => {
                //原則授權               
                options.AddPolicy("IsIT", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "IT");
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();   //設定使用靜態檔案
            app.UseRouting();

            #region cookie驗證
            //須注意 UseAuthentication() && UseAuthorization() 的順序不可以顛倒
            //啟用 cookie 原則
            app.UseCookiePolicy();
            //啟用驗證
            app.UseAuthentication();
            //啟用授權
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
