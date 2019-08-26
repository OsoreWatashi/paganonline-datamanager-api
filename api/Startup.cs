using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaganOnline.DataManager.API.Models;

namespace PaganOnline.DataManager.API
{
  public class Startup
  {
    private const string DevelopmentCORS_Development = nameof(DevelopmentCORS_Development);
    private const string DevelopmentCORS_Production = nameof(DevelopmentCORS_Production);
    
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
        {
          options.AddPolicy(DevelopmentCORS_Development, builder => { builder.WithOrigins("http://localhost:8080"); });
          options.AddPolicy(DevelopmentCORS_Production, builder => { builder.WithOrigins("https://paganonline.wiki"); });
        });
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddSingleton(new DataManagerContext(Configuration.GetConnectionString("paganonline-datamanager")));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseCors(DevelopmentCORS_Development);
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseCors(DevelopmentCORS_Production);
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}