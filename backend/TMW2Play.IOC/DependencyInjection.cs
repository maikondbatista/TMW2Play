namespace TMW2Play.IOC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<TMW2PlayContext>();
        }
}
