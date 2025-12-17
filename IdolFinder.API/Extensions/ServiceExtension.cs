using FluentValidation;
using IdolFinder.API.Middlewares;
using IdolFinder.Application;
using IdolFinder.Application.Behaviors;
using IdolFinder.Application.Configurations.Options;
using IdolFinder.Application.Images.Endpoints;
using IdolFinder.Application.Services;
using IdolFinder.Core.Repositories;
using IdolFinder.CrawData.Services;
using IdolFinder.Infrastructure.Data;
using IdolFinder.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace IdolFinder.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCorsAllowAny(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Idol Finder API", Version = "v1" });
            });
        }

        public static void RegisterApplicationLayers(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddThirdPartyServices(typeof(AssemblyReference).Assembly);
        }

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStorageService>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<StorageOptions>>().Value;

                return options.Type?.ToLower() switch
                {
                    "blob" => ActivatorUtilities.CreateInstance<BlobStorageService>(provider),
                    _ => ActivatorUtilities.CreateInstance<LocalStorageService>(provider)
                };
            });

            services.Configure<ImageSourceOptions>(configuration.GetSection("ImageSource"));
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));

            services.AddScoped<LockettsEndpointHandler>();
            services.AddScoped<AbcEndpointHandler>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdolFinderContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ExceptionHandlingMiddleware>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IProductRepository, ProductRepository>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddThirdPartyServices(this IServiceCollection services, Assembly assembly)
        {
            services.AddAutoMapper(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            services.AddHttpClient();
        }
    }
}
