using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using WPCCrimeData;
using WPCCrimeData.Interfaces.Services;
using WPCCrimeData.Services;

Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => {
        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();

class Startup
{
    public IConfiguration Configuration { get; init; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Action<GlobalOptions> globals = (g =>
        {
            g.CrimeDataAPI = Configuration.GetConnectionString("CrimeDataAPI");
        });

        services.Configure(globals);

        services.AddSingleton(s => s.GetRequiredService<IOptions<GlobalOptions>>().Value);
        services.AddSingleton<ICrimeDataService, CrimeDataService>();

        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
        });
    }
}
