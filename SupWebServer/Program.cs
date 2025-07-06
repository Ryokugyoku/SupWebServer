using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scrutor;
using SupWebServer.BusinessModel;
using SupWebServer.DB;
using SupWebServer.System;

var builder = WebApplication.CreateBuilder(args);

var adminSubjects = builder.Configuration.GetSection("AdminSubjects").Get<string[]>()
                    ?? builder.Configuration.GetValue<string>("ADMIN_SUBJECTS")?
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    ?? [];



builder.Services.AddControllers();   
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer(); // ← UI には必須
builder.Services.AddSwaggerGen();           // ← ドキュメント生成
//Google OpenID Connect
string googleClientId = builder.Configuration["GOOGLE_CLIENT_ID"]
                     ??　"環境変数 'GOOGLE_CLIENT_ID' が設定されていません。";

//DB
var conn = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(conn)); 

//サービスクラスのDI登録
builder.Services.Scan(scan => scan
        .FromAssemblyOf<Marker>()           // BusinessModel アセンブリ
        // ------------- Singleton -------------
        .AddClasses(c => c.WithAttribute<ServiceAttribute>(a => a.Lifetime == ServiceLifetime.Singleton))
        .AsImplementedInterfaces()
        .WithSingletonLifetime()

        // ------------- Scoped（既定） -------------
        .AddClasses(c => c.WithAttribute<ServiceAttribute>(a =>
            a == null || a.Lifetime == ServiceLifetime.Scoped))
        .AsImplementedInterfaces()
        .WithScopedLifetime()

        // ------------- Transient -------------
        .AddClasses(c => c.WithAttribute<ServiceAttribute>(a => a.Lifetime == ServiceLifetime.Transient))
        .AsImplementedInterfaces()
        .WithTransientLifetime()
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173","https://blueremarks.com")  // フロントのオリジン
            .AllowAnyHeader()                      // もしくは .WithHeaders("Content-Type","Authorization")
            .AllowAnyMethod());                    // POST, OPTIONS など
});



// 認証パンドラ For Google
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Discovery ドキュメントとJWKSを自動取得
        options.Authority = "https://accounts.google.com";
        // aud クレームを検証        
        options.Audience = googleClientId;


        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            // Google からは 2 通りの issuer が返ることがあるため両方許可
            ValidIssuers = new[]
            {
                "https://accounts.google.com",
                "accounts.google.com"
            },

            ValidateAudience         = true,
            ValidAudience            = googleClientId,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ClockSkew                = TimeSpan.FromSeconds(60),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = "sub"

        };
        
        // ❸ subject ごとに Admin ロールを注入
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"Auth failed: {ctx.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                // "sub" は NameIdentifier にマップされている
                var subject = ctx.Principal!.FindFirstValue(ClaimTypes.NameIdentifier);

                if (subject is not null && adminSubjects.Contains(subject))
                {
                    var identity = (ClaimsIdentity)ctx.Principal.Identity!;
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }

                return Task.CompletedTask;
            }
        };

    });



var app = builder.Build();
//マイグレーション
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();          // ← ここがポイント
}

//Corsの設定
app.UseCors("AllowFrontend");
// Configure the HTTP request pipeline.
app.UseSwagger();  
app.UseSwaggerUI(); 

// Httpsだけの利用を促す。
app.UseHttpsRedirection();

//認証システムの起動
app.UseAuthentication();
app.UseAuthorization();

//属性ルーティングの読み込み
app.MapControllers()
    .RequireAuthorization()   // すべてのコントローラに認可を強制
    .WithTags("v1");  


app.Run();

