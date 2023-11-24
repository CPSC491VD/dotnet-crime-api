var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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


