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
//加解密
using Portfolio.Security;
//連接DB
using Portfolio.Data;
using Microsoft.EntityFrameworkCore;
//cookie認證、授權
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
//多國語系
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Authentication;

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
            string CipherText = Configuration.GetConnectionString("SQL_ConnectionEncryptString");
            string PublicKey = Configuration.GetConnectionString("Public_Key");

            CipherServices CS = new CipherServices();

            //解密連線字串
            string SQLConnectionString = CS.CipherToPlainText(CipherText, PublicKey);

            //透過 DbContextOptions 物件上的方法，來連結 appsetting.json 中，設定好的 SQL server connection string
            //services.AddDbContext<PortfolioContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("HeisenbergHsueh_Portfolio_DB")));
            services.AddDbContext<PortfolioContext>(options => options.UseSqlServer(SQLConnectionString));
            #endregion

            #region Cookie驗證
            //註冊cookie驗證的服務
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                //設定cookie名稱
                options.Cookie.Name = "HeisenbergHsueh_Cookie";
                //browser 會限制 cookie 只能經由 http protocol 來存取
                options.Cookie.HttpOnly = true;
                //設定登入頁面，未登入時自動導入到登入頁面
                //options.LoginPath = new PathString("/LoginSystem/Login");
                //設定登出頁面(此頁面可以省略)
                //options.LogoutPath = new PathString("/LoginSystem/Logout");
                //當登入者不具有當下頁面的訪問權限時，會自動轉跳到此Forbidden頁面
                //options.AccessDeniedPath = new PathString("/LoginSystem/Forbidden");
                //設定登入有效時間(單位:分鐘)
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //SlidingExpiration = true，表示說，如果cookie當前的時間已經超過開始時間與結束時間的一半，則重新頒發cookie
                //options.SlidingExpiration = true;
                options.EventsType = typeof(HeisenbergHsuehCookieAuthenticationEvents);
            });
            #endregion

            services.AddScoped<HeisenbergHsuehCookieAuthenticationEvents>();

            #region 授權
            services.AddAuthorization(options => {
                //原則授權               
                options.AddPolicy("IsIT", policy => {
                    policy.RequireClaim(ClaimTypes.Role, "IT");
                });
            });
            #endregion

            #region 多國語系設定
            //設定讀取資源檔(.resx)的路徑
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //加入讓View、Model也可使用多國語系的語法
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            //宣告一份CultureInfo的清單
            var supportedCultures = new[]
            {
                new CultureInfo("zh-TW"),
                new CultureInfo("en-US")
            };
            //
            services.Configure<RequestLocalizationOptions>(options =>
            {

                options.DefaultRequestCulture = new RequestCulture(culture: "zh-TW", uiCulture: "zh-TW");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                options.AddInitialRequestCultureProvider(new RouteDataRequestCultureProvider()
                {
                    Options = options,
                    RouteDataStringKey = "culture",
                    UIRouteDataStringKey = "culture"
                });
            });
            #endregion
        }

        #region 動態改變cookie authentication的LoginPath、LogoutPath、AccessDenied
        //因為網站設定了多國語系，因此導致user在進入需要登入or需要授權才看得見的頁面時，cookie authentication的跳轉頁面出現問題
        //起因是由於，cookie authentication的LoginPath、LogoutPath、AccessDenied的路徑是寫死的，但因為語系設定，所以希望user在不同語系的頁面時
        //可以跳轉到對應語系的頁面，因此以override的方式，重新改寫了RedirectToLogin、RedirectToLogout、RedirectToAccessDenied的跳轉方法
        //參考資料(1) : https://social.msdn.microsoft.com/Forums/en-US/8626b4cf-be32-45e7-9704-e98c6d11b10f/how-can-i-set-dynamic-configureapplicationcookie?forum=aspdotnetcore
        public class HeisenbergHsuehCookieAuthenticationEvents : CookieAuthenticationEvents
        {
            public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if(GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if(GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Login";

                return base.RedirectToLogin(context);
            }

            public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if (GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if (GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Logout";

                return base.RedirectToLogout(context);
            }

            public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
            {
                string GetCurrentContextPath = context.Request.Path.ToString();

                string SetCulture = "";

                if (GetCurrentContextPath.Contains("zh-TW"))
                {
                    SetCulture = "zh-TW";
                }
                else if (GetCurrentContextPath.Contains("en-US"))
                {
                    SetCulture = "en-US";
                }

                context.RedirectUri = $"{context.Request.Scheme}://{context.Request.Host}/{SetCulture}/LoginSystem/Forbidden";

                return base.RedirectToAccessDenied(context);
            }
        }
        #endregion

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

            #region 多國語系設定
            app.UseRequestLocalization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=zh-TW}/{controller=JobRecordSystem}/{action=JobRecordSystemIndex}/{id?}");
            });
        }
    }
}
