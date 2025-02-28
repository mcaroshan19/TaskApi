//using System.Data.SqlClient;

//var builder = WebApplication.CreateBuilder(args);



//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////builder.Services.AddTransient<SqlConnection>(provider =>
////{
////    var configuration = provider.GetRequiredService<IConfiguration>();
////    var connectionString = configuration.GetConnectionString("DbContextConnection");
////    return new SqlConnection(connectionString);
////});


//builder.Services.AddTransient<SqlConnection>(provider =>
//{
//    var configuration = provider.GetRequiredService<IConfiguration>();
//    var connectionString = configuration.GetConnectionString("DbContextConnection");
//    return new SqlConnection(connectionString);
//});


//var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



using APITask.Implimention;
using APITask.Interface;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IIFoodService, FoodService>();

// Register SqlConnection as a transient service
builder.Services.AddTransient(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DbContextConnection");
    return new SqlConnection(connectionString);
});

var app = builder.Build();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

