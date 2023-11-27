var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.DocumentTitle = "CrimeAPI";
        c.RoutePrefix = string.Empty;  
    }
);


app.UseHttpsRedirection();

app.MapGet("/crime/{page}", (int page) =>
{
    int pageSize = 10;
    CrimeDatabase c = new CrimeDatabase();
    CrimePageAPI res = c.GetCrimeData(page, pageSize);
    return res;
})
.WithName("crime")
.WithOpenApi();

app.Run();


