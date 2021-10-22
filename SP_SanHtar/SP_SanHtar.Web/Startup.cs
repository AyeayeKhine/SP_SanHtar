using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using SP_SanHtar.Models;
using SP_SanHtar.Web.cls;
using SP_SanHtar.Web.ContextDB;
using SP_SanHtar.Web.DataFormat;
using SP_SanHtar.Web.Helpers;
using SP_SanHtar.Web.Middleware;
using SP_SanHtar.Web.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace SP_SanHtar.Web
{
    public class Startup
    {
        private IWebHostEnvironment env { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            // Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<OnlineContext>();
            services.AddControllers();
            services.AddSingleton<IItemRepository, ItemRepository>();
            //services.AddSwaggerGen(c =>
            //{
            //    c.EnableAnnotations();
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Learn API", Version = "v1" });
            //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            //});
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                // Configure a custom converter
                //options.SerializerOptions.Converters.Add(new MyCustomJsonConverter());
            });
            
            // Helpers
            //AuthenticationHelper.ConfigureService(services, Configuration["JwtSecurityToken:Issuer"], Configuration["JwtSecurityToken:Audience"], Configuration["JwtSecurityToken:Key"]);
            // Configure JWT authentication.
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearerConfiguration(Configuration["JwtSecurityToken:Issuer"], Configuration["JwtSecurityToken:Audience"]);
            CorsHelper.ConfigureService(services);
            SwaggerHelper.ConfigureService(services);
            

            //Settings
            services.Configure<AppSettings>(Configuration.GetSection("JwtSecurityToken"));

            //services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();

            services.AddTransient<IEmailSender, EmailSender>();
            //services.AddSession(options => {
            //    options.IdleTimeout = new TimeSpan(0, 2, 0);
            //});

            //services.AddMvc().AddJsonOptions(opt =>
            //{
            //    if (opt.SerializerSettings.ContractResolver != null)
            //    {
            //        var resolver = opt.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        resolver.NamingStrategy = null;
            //    }
            //});
            //  services.AddControllers().AddJsonOptions(options =>
            //options.JsonSerializerOptions.Converters.Add(new TimeSpanToStringConverter()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //ConfigureAuth(app);
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            //app.UseSession();

            app.UseCors("CorsPolicy");
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learn API V1");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });

            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseErrorHandlingMiddleware();
            //app.Use(async (context, aa) => {
            //    var user = context.Request.Headers;
            //});

            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}