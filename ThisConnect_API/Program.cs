
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using ThisConnect_API.Models;
using ThisConnect_API.Hubs;
using Microsoft.Extensions.FileProviders;
using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();// SignalR ile credentialed CORS izinleri i�in gerekli
    });
});
builder.Services.AddSignalR();
builder.Services.AddDbContext<Db7877Context>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");
var userProfilePhotosPath = Path.Combine(rootFolderPath, "UserProfilePhotos");
var rootFilesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
var userUploadedFilesPath = Path.Combine(rootFilesFolderPath, "UserFiles");

if (!Directory.Exists(userProfilePhotosPath))
{
    Directory.CreateDirectory(userProfilePhotosPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos", "UserProfilePhotos")),
    RequestPath = "/Photos/UserProfilePhotos"
});

if (!Directory.Exists(rootFilesFolderPath))
{
    Directory.CreateDirectory(userUploadedFilesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "UserFiles")),
    RequestPath = "/UploadedFiles/UserFiles"
});


app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Run();




