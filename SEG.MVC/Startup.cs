using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SEG.Client;
using SEG.Client.Auth;
using SEG.Domain.Contracts.Clients;
using SEG.Domain.Contracts.Clients.Auth;
using System;

namespace SEG.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Injeção de dependência services
            services.AddTransient<IEventoClient, EventoClient>();
            services.AddTransient<IFormularioClient, FormularioClient>();
            services.AddTransient<IModuloClient, ModuloClient>();
            services.AddTransient<IPerfilClient, PerfilClient>();
            services.AddTransient<IFuncaoClient, FuncaoClient>();
            services.AddTransient<IUsuarioClient, UsuarioClient>();
            
            services.AddTransient<IRegisterClient, RegisterClient>();
            services.AddTransient<ILoginClient, LoginClient>();
            services.AddTransient<ITrocaSenhaClient, TrocaSenhaClient>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Adicionando o serviço de cookies na aplicação
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/Conta/Login");

            //Todos os controllers protegidos contra acesso anônimo 
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
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
                app.UseExceptionHandler("/Error");
                //app.UseExceptionHandler("/Home/Error"); //<== Original
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
