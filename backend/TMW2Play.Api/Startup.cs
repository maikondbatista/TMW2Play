using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TMW2Play.Api.Configuration;
using TMW2Play.Infra;

namespace TMW2Play.Api
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
            services.AddControllers();
            //services.RegisterDependencyInjection();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TMW2Play API", Version = "v1" });
            });

            //services.AddEntityFrameworkNpgsql().AddDbContext<TMW2PlayContext>(options =>
            //    options.UseNpgsql(Configuration.GetConnectionString("DbPaciente"))
            //    .UseLowerCaseNamingConvention()
            //    );
            services.AddSteam();
            services.AddAIApi();
            services.AddSteamKey(Configuration);
            services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        );
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            //app.AddSteam();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}