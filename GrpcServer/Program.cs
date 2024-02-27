using GrpcServer;
using GrpcServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ProtoBuf.Grpc.Server;
using System.IO.Compression;
using System.Net;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services
          .AddAuthorization()
          .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.SaveToken = true;
              options.RequireHttpsMetadata = true;

              options.TokenValidationParameters = new()
              {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = JwtAuthenticationManager.Issuer,
                  ValidAudience = JwtAuthenticationManager.Audience,
                  IssuerSigningKey = JwtAuthenticationManager.SymmetricSecurityKey,
              };
          });

        builder.Services.AddGrpc();

        builder.Services.AddCodeFirstGrpc(config =>
        {
            config.ResponseCompressionLevel = CompressionLevel.Optimal;
        });
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 5004, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
                listenOptions.UseHttps();

            });
        });
        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<AuthenticationService>();
        app.MapGrpcService<GreeterService>();

        await app.RunAsync();
    }
}