using System.Reflection;
using System.Text;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SecretsManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using TaskManagerBackEnd.DataSeed;
using TaskManagerBackEnd.Middlewares;

namespace TaskManagerBackEnd;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        builder.Services.AddControllers();
        
        string keyStr = builder.Configuration.GetValue<string>("JwtKey");
        
        byte[] key = Encoding.ASCII.GetBytes(keyStr);
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "TaskManager",
                    ValidAudience = "TaskManager",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCustomServices();
        builder.Services.AddEndpointsApiExplorer();
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        builder.Services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(xmlPath);
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Task Manager API",
                Version = "v1", // Alterado para uma versão válida
                Description = "API para gerenciamento de tarefas e usuários.",
                Contact = new OpenApiContact
                {
                    Name = "Equipe de Suporte",
                    Email = "suporte@taskmanager.com"
                },
            });

            c.UseInlineDefinitionsForEnums();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no formato: Bearer {token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
        });
        
        AWSOptions? awsOptions = builder.Configuration.GetAWSOptions();
        builder.Services.AddDefaultAWSOptions(awsOptions);
        builder.Services.AddAWSService<IAmazonSecretsManager>();


        WebApplication? app = builder.Build();

        // Configure the HTTP request pipeline.
       
        app.UseSwagger(s =>
        {
            s.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
            s.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                using StringWriter stringWriter = new();
                swaggerDoc.SerializeAsV3(new OpenApiYamlWriter(stringWriter));
                swaggerDoc.Info.Version = "v1";
                swaggerDoc.Info.Title = "Task Manager API";
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API v1");
        });
        
        

        app.UseHttpsRedirection();
        app.UseMiddleware<JwtMiddleware>();
        app.UseAuthorization();
        
        app.MapControllers();

        app.UseCors(cors =>
        {
            cors.AllowAnyHeader();
            cors.AllowAnyMethod();
            cors.AllowAnyOrigin();
        });

        using (IServiceScope scope = app.Services.CreateScope())
        {
            UserAdminSeed userSeed = new(scope);
            userSeed.SeedTeamAdmin();
            userSeed.SeedUserAdmin();
        }

        app.Run();
    }
}