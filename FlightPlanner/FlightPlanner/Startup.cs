using AutoMapper;
using FlightPlanner.Core.Deletion;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Search;
using FlightPlanner.Core.Services;
using FlightPlanner.Core.Validations;
using FlightPlanner.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FlightPlanner.Handlers;
using FlightPlanner.Services;
using FlightPlanner.Services.Deletion;
using FlightPlanner.Services.Search;
using FlightPlanner.Services.Validations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlightPlanner", Version = "v1" });
            });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddDbContext<FlightPlannerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IFlightPlannerContext, FlightPlannerContext>();
            services.AddScoped<IDbService, DbService>();
            services.AddScoped<IEntityService<Flight>, EntityService<Flight>>();
            services.AddScoped<IEntityService<Airport>, EntityService<Airport>>();
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IValidate, FlightValidation>();
            services.AddScoped<IValidate, FlightTimesValidator>();
            services.AddScoped<IValidate, FlightCarrierValidator>();
            services.AddScoped<IValidate, AirportValdator>();
            services.AddScoped<IValidate, AirportPropsValidator>();
            services.AddScoped<IValidate, SameAirportValidator>();
            services.AddScoped<IValidate, FlightTimeIntervalValidator>();
            services.AddScoped<IFlightDelete, DeleteFlight>();
            services.AddScoped<ISearch, Search>();
            services.AddSingleton<IMapper>(AutoMapperConfig.CreateMapper());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightPlanner v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
