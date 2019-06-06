using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Client_Customer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

namespace Client_Customer
{
    /// <summary>
    /// This class is used for configurin the Razor Pages application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Represent a set of key/value application coniguration properties.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Specifies what to execute at startup.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();                             // clearing the mapping used by services by default (urls), and keeps them as defined at IDP level
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // adds MVC framework services 
            services.AddMvc();
            /*// adds services Razor Pages and ASP.NET MVC require
            .AddRazorPagesOptions(options => {options.Conventions.AddPageRoute("/index", "home"); });*/

            // dependency injection of services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();                     // registers an IHttpContextAccessor so we can access the current HttpContext in services by injecting it
            services.AddScoped<IThisHttpClient, ThisHttpClient>();                                  // registers an "ThisHttpClient"
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddLogging();

            // adding open id connect authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")                                       // configures the cookie handler and enables the application to use cookie based authentication for the default scheme

                // The below handler creates authorization requests, token and other requests, and handles the identity token validation
                // "oidc" ensure when part of application requires authentication, "OpenIdConnect" will be triggered by default 
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";                       // matches the default scheme for authentication, ensures that succesful authentication result will be stored in a our applications "cookie"
                    options.Authority = "https://localhost:44370/";         // the authority is set to be the identity provider (the authority responsible for the IDP part of the OIDC flow), the middleware will use this value to read the metadata on the discovery end point, so it knows where to find different endpoints ad other information
                    options.ClientId = "3TL";                               // must match client ID at IDP level
                    options.ResponseType = "code id_token";                 // one of response type of the Hybrid ground to be used
                    //options.CallbackPath = new PathString("...");         // allows the change of the redirect Uri from inside the IdentityServerConfig (hand made class)
                    //options.SignedOutCallbackPath = new PathString("...") // 
                    options.Scope.Add("openid");                            // required scope which requires in sub value being included
                    options.Scope.Add("profile");                           // ensures profile related claims are included
                    options.Scope.Add("roles");                             // requesting a CUSTOM MADE scope (check IDP scopes/roles for details)
                    options.SaveTokens = true;                              // allows the middleware to save the tokens it receives from the identity provider so we can easelly use them afterwards
                    options.ClientSecret = "test_secret";                   // must match secret at IDP level
                    options.GetClaimsFromUserInfoEndpoint = true;           // enables GETing claims from user info endpoint regarding the current authenticate user
                    //options.ClaimActions.Remove("amr");                   // allows us to remove CLAIM FILTERS (AKA this ensures the AMR(Authentication Method Reference) claim is dispalyed and not filtered out)
                    options.ClaimActions.DeleteClaim("sid");                // removing unnecessary claims from the initial cookie (session ID at level of IDP)
                    options.ClaimActions.DeleteClaim("idp");                // removing unnecessary claims from the initial cookie (the Identity Provider)
                    options.ClaimActions.DeleteClaim("name");               // removing unnecessary claims from the initial cookie (removing this type of data reduces the cookie size)
                    options.ClaimActions.DeleteClaim("given_name");         // removing unnecessary claims from the initial cookie (removing this type of data reduces the cookie size)
                    options.ClaimActions.DeleteClaim("family_name");        // removing unnecessary claims from the initial cookie (removing this type of data reduces the cookie size)
                    options.ClaimActions.MapUniqueJsonKey("role", "role");  // adding the CUSTOM MADE claim to the claims map
                    //options.SignedOutCallbackPath = "/signout-callback-oidc";     // NOTE THIS DOES NOT WORK
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //// SELF NOTE: "else" is extra code according to https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-2.2
            //// NOTE: This will not currently work
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            app.UseAuthentication();                                        // this should be moved higher is authentication must be made before static file requests
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
