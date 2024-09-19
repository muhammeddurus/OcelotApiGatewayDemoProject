using AuthServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//Uygulamanın kimlik doğrulama özelliğini aktif ediyoruz. account/login adresi login işlemleri için ayarlanmıştır.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => options.LoginPath = "/account/login");

//OpenIddict servisini uygulamaya ekliyoruz.
builder.Services.AddOpenIddict()
    //OpenIddict core/çekirdek yapilandirmalari gerçeklestiriliyor.
    .AddCore(options =>
    {
        //Entity Framework Core kullanilacagi bildiriliyor.
        options.UseEntityFrameworkCore()
               //Kullanilacak context nesnesi bildiriliyor.
               .UseDbContext<AuthDbContext>();
    })
    //OpenIddict server yapilandirmalari gerçekleþtiriliyor.
    .AddServer(options =>
    {
        //Token talebinde bulunulacak endpoint'i set ediyoruz.
        options.SetTokenEndpointUris("/connect/token")
               //Authorization Code talebinde bulunulacak endpoint'i set ediyoruz.
               .SetAuthorizationEndpointUris("/connect/authorize")
               //Logout istegi geldiginde yönlendirilecek endpoint'i set ediyoruz.
               .SetLogoutEndpointUris("/connect/logout")
               //Kullanici bilgilerini edinebilmek için userinfo endpoint'ini set ediyoruz.
               .SetUserinfoEndpointUris("/connect/userinfo");
        //Akis türü olarak Client Credentials Flow'u etkinlestiriyoruz.
        options.AllowClientCredentialsFlow()
               //Authorization Code Flow'u etkileþtiriyoruz.
               .AllowAuthorizationCodeFlow()
               .RequireProofKeyForCodeExchange();
        //Signing ve encryption sertifikalarýný ekliyoruz.
        options.AddEphemeralEncryptionKey()
               .AddEphemeralSigningKey()
               //Normalde OpenIddict üretilecek token'ý güvenlik amacýyla þifreli bir þekilde bizlere sunmaktadýr.
               //Haliyle jwt.io sayfasýnda bu token'ý çözümleyip görmek istediðimizde þifresinden dolayý
               //incelemede bulunamayýz. Bu DisableAccessTokenEncryption özelliði sayesinde üretilen access token'ýn
               //þifrelenmesini iptal ediyoruz.
               .DisableAccessTokenEncryption();
        //OpenIddict Server servislerini IoC Container'a ekliyoruz.
        options.UseAspNetCore()
               //EnableTokenEndpointPassthrough : OpenID Connect request'lerinin OpenIddict tarafýndan iþlenmesi için gerekli konfigürasyonu saðlar.
               .EnableTokenEndpointPassthrough()
               //EnableAuthorizationEndpointPassthrough: OpenID Connect request'lerinin Authorization Endpoint için aktifleþtirilmesini saðlar.
               .EnableAuthorizationEndpointPassthrough()
               .EnableLogoutEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough();
        //Yetkileri(scope) belirliyoruz.
        options.RegisterScopes("read", "write");
    });

//OpenIddict'i SQL Server'ý kullanacak þekilde yapýlandýrýyoruz.
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ESale"));
    //OpenIddict tarafýndan ihtiyaç duyulan Entity sýnýflarýný kaydediyoruz.
    options.UseOpenIddict();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();