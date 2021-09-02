using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
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
using Swashbuckle.AspNetCore.SwaggerUI;

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
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Learn API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                // Configure a custom converter
                //options.SerializerOptions.Converters.Add(new MyCustomJsonConverter());
            });
            services.AddTransient<IEmailSender, EmailSender>();

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

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
        }
    }
}