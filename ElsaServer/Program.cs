using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using ElsaServer.CustomActivities;
using FluentStorage;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id"))); // Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.

// Add Health Checks.
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    //issue for Elsa fix
    option.CustomSchemaIds(type => type.ToString());

    //預設使用JWT token 
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter: ApiKey [your API key]",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddElsa(elsa =>
{
    // Configure Management layer to use EF Core.
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore());

    // Configure Runtime layer to use EF Core.
    elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore());

    // Default Identity features for authentication/authorization.
    elsa.UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key"; // This key needs to be at least 256 bits long.
        identity.UseAdminUserProvider();
    });

    // Configure ASP.NET authentication/authorization.
    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

    // Expose Elsa API endpoints.
    elsa.UseWorkflowsApi();

    // Setup a SignalR hub for real-time updates from the server.
    elsa.UseRealTimeWorkflows();

    // Enable C# workflow expressions
    elsa.UseCSharp();

    // Enable HTTP activities.
    elsa.UseHttp();

    // Use timer activities.
    elsa.UseScheduling();

    elsa.UseHttp(http => http.ConfigureHttpOptions = options =>
    {
        options.BaseUrl = new Uri("https://localhost:5001");
        options.BasePath = "/workflows"; //工作流程入口
    });

    // Register custom activities from the application, if any.
    elsa.AddActivitiesFrom<Program>();

    // Register custom workflows from the application, if any.
    elsa.AddWorkflowsFrom<Program>();

    elsa.AddActivity<Greeter>();

    // Use email activities.
    elsa.UseEmail(email =>
    {
        email.ConfigureOptions = options =>
        {
            options.Host = "192.168.10.253";
            options.Port = 25;
            options.SecureSocketOptions = MailKit.Security.SecureSocketOptions.None;
        };
    });

    elsa.UseFileStorage(fileStorage => StorageFactory.Blobs.DirectoryFiles("D:\\ElsaTest"));

    elsa.UseSQLServer();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //issue for Elsa fix
        c.SwaggerEndpoint("v1/swagger.json", "Elsa Api V1");
    });
}

// Configure web application's middleware pipeline.
app.UseCors();
app.UseRouting(); // Required for SignalR.
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 


app.Run();
