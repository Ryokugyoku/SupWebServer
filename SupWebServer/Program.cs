var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();   
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); // ← UI には必須
builder.Services.AddSwaggerGen();           // ← ドキュメント生成


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();  
app.UseSwaggerUI(); 
//一旦コメントアウト
app.UseHttpsRedirection();

app.Run();

