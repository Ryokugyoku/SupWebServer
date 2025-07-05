var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // ← UI には必須
builder.Services.AddSwaggerGen();           // ← ドキュメント生成

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapOpenApi();
app.UseSwaggerUI(); 
app.MapControllers(); 
//一旦コメントアウト
//app.UseHttpsRedirection();

app.Run();

