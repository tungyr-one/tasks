using OnlineStoreApp.Data;
using OnlineStoreApp.Helpers;
using OnlineStoreApp.Repositories;
using OnlineStoreApp.Services;

var builder = WebApplication.CreateBuilder(args);

 var services = builder.Services;

services.AddControllers();
services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));

services.AddSingleton<DataContext>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductService, ProductService>();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.MapControllers();

app.Run();
