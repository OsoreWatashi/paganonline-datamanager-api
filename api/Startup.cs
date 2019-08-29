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
    private const string CORS = nameof(CORS);
    
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
        options.AddPolicy(CORS, builder => builder.
          WithOrigins("*").
          AllowAnyMethod().
          AllowAnyHeader().
          WithExposedHeaders("*"));
      });
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddSingleton(new DataManagerContext(Configuration.GetConnectionString("paganonline-datamanager")));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseCors(CORS);
      app.UseMvc();
    }
  }
}