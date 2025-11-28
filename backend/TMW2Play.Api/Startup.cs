using Microsoft.OpenApi;
using TMW2Play.Api.Configuration;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TMW2Play API", Version = "v1" });
            });

            services.AddSteam(Configuration);
            services.AddGeminiAi(Configuration);
            services.AddServices();
            services.AddHybridCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API");
                });
                app.UseCors(builder => builder
                     .AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     );
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseCors(builder => builder
                 .WithOrigins("https://maikondbatista.github.io/TMW2Play")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 );
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}