using Microsoft.OpenApi;
using Microsoft.AspNetCore.RateLimiting;
using TMW2Play.Api.Configuration;
using System.Threading.RateLimiting;

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
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("AILimiter", opt =>
                {
                    opt.PermitLimit = 2;
                    opt.Window = TimeSpan.FromSeconds(1);
                });
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 15,
                        Window = TimeSpan.FromMinutes(1)
                    }));
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync("Rate limit exceeded");
                };
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
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
            app.UseRateLimiter();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}