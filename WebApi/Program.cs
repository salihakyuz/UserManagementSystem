using Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models; //using kullanmadan builder k?sm? ekenemiyor, �ncesinde dependencies k?sm?nda infr. katman?n? ba?lamak gerekiyor
using FluentValidation;
using FluentValidation.AspNetCore;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        /*add swaggerr!!!!*/
        builder.Services.AddSwaggerGen(swagger =>
        {
            //DEFAULT UI OF SWAGGER DOCUMENTATION
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ASP.NET WEB API",
                Description = "AUTHENTICATION"
            });
            //ENABLE AUTHARIZATION USING SWAGGER (JWT)
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "Jwt",
                In = ParameterLocation.Header,
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id= "Bearer"
                        }
                    },Array.Empty<string>()
                }

            });

        });
        var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy
                          .AllowAnyOrigin()
                                                 .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});
        builder.Services.InfrastructureServices(builder.Configuration); //s?ra �nemli, bu sayede inf. layerinden connection bilgilerini ald?k
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(MyAllowSpecificOrigins);
        app.UseHttpsRedirection();

        app.UseAuthentication();//eklendi 
        app.UseAuthorization();

        //app.UseMiddleware<validationMiddleware>();
        app.MapControllers();

        app.Run();
    }
}