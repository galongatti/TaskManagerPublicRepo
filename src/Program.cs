using Amazon.SecretsManager;

namespace TaskManagerBackEnd;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        builder.Services.AddAWSService<IAmazonSecretsManager>();

        builder.Services.AddCustomServices();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        WebApplication? app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.UseCors(cors =>
        {
            cors.AllowAnyHeader();
            cors.AllowAnyMethod();
            cors.AllowAnyOrigin();
        });

        app.Run();
    }
}