using System.Text;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SecretsManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        builder.Services.AddSwaggerGen();
        
        AWSOptions? awsOptions = builder.Configuration.GetAWSOptions();
        builder.Services.AddDefaultAWSOptions(awsOptions);
        builder.Services.AddAWSService<IAmazonSecretsManager>();


        WebApplication? app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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