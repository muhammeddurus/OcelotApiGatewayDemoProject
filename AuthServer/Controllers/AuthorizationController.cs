using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using AuthServer.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.Controllers
{
    public class AuthorizationController : Controller
    {
        readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(IOpenIddictApplicationManager applicationManager)
        {
            _applicationManager = applicationManager;
        }

        //Bu action'ın endpoint'ini token endpoint ile aynı şekilde ayarlıyoruz.
        /// <summary>
        /// Gelen token taleplerinde client-id ve cilent-secret 
        /// değerleri doğrulanacak. Gerekli operasyonlar gerçekleşicek.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();


            if (request?.IsAuthorizationCodeGrantType() is not null)
            {
                //Authorization Code'da store edilen request sorumlusunu elde ediyoruz.
                var principal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

                //Principal'ı yani kullanıcıyı doğrula
                //if ((await _userManager.GetUserAsnyc(principal)) != null)
                //{

                //}

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            }
            else if (request?.IsClientCredentialsGrantType() is not null)
            {
                //Client credentials OpenIddict tarafından otomatik olarak doğrulanır.
                //Eğer ki gelen request'in body'sindeki client_id veya client_secret bilgileri geçersizse, burası tetiklenmeyecektir.

                var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
                if (application is null)
                    throw new InvalidOperationException("This clientId was not found");

                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                //Token'a claim'leri ekleyelim. Subject'in eklenmesi zorunludur.
                //Destination'lar ile claim'lerin hangi token'a ekleneceğini belirtiyoruz. AccessToken mı? Identity Token mı? Yoksa her ikisi de mi?
                identity.AddClaim(Claims.Subject, (await _applicationManager.GetClientIdAsync(application) ?? throw new InvalidOperationException()));
                identity.AddClaim(Claims.Name, (await _applicationManager.GetDisplayNameAsync(application) ?? throw new InvalidOperationException()));
                identity.AddClaim("ozel-claim", "ozel-claim-value");

                identity.AddClaim(JwtRegisteredClaimNames.Aud, "Example-OpenIddict", Destinations.AccessToken);

                identity.SetDestinations(static claim => claim.Type switch
                {
                    Claims.Name when claim.Subject.HasScope(Scopes.Profile) => new[]
                    {
                        Destinations.AccessToken,
                        Destinations.IdentityToken
                    },
                    _ => new[] { Destinations.AccessToken }
                });

                var claimsPrincipal = new ClaimsPrincipal(identity);
                claimsPrincipal.SetScopes(request.GetScopes());

                //SignIn return etmek, biryandan OpenIddict'ten uygun access/identity token talebinde bulunmaktır.
                //SignIn metodu ile yapılan return neticesinde OpenIddict kütüphanesinin
                //devreye girmesi ve access token ile identity token değerlerinin
                //üretiminin gerçekleştirmesi sağlanmakta ve bu değerler request yapan istemciye gönderilmektedir.
                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            throw new NotImplementedException("The specified grant type is not implemented.");
        }

        [HttpGet("~/connect/authorize"), HttpPost("~/connect/authorize")]
        public async Task<IActionResult> Authorize(string accept, string deny)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            //Cookie'de authentication için tutulan veriden kullanıcı bilgisini alıyoruz.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Eğer kullanıcı bilgisi yoksa kullanıcıyı login sayfasına yönlendiriyoruz.
            if (!result.Succeeded)
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties()
                    {
                        RedirectUri = $"{Request.PathBase}{Request.Path}{(QueryString.Create(Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList()))}"
                    });

            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            identity.AddClaim(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name, Destinations.AccessToken);
            identity.AddClaim(JwtRegisteredClaimNames.Aud, "Example-OpenIddict", Destinations.AccessToken);
            identity.AddClaim("ornek-claim", "ornek claim value", Destinations.AccessToken);

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
            if (HttpContext.Request.Method == "GET")
                return View(new AuthorizeVM
                {
                    ApplicationName = await _applicationManager.GetLocalizedDisplayNameAsync(application),
                    Scopes = request.Scope
                });
            else if (!string.IsNullOrEmpty(accept))
            {
                var claimsPrincipal = new ClaimsPrincipal(identity);
                var scopes = request.GetScopes();
                claimsPrincipal.SetScopes(scopes);
                //SignIn fonksiyonunu kullanarak authorization code’a karşılık access token döndürebilmesi için OpenIddict middleware’i tetiklemektedir. 
                
                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            //onay verilmediyse yetkilendirme engelleniyor
            return Forbid(authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidScope
                }));
        }
        [HttpGet("~/connect/logout")]
        public IActionResult Logout() => View();

        [ActionName(nameof(Logout)), HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPost()
        {
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo")]
        public async Task<IActionResult> Userinfo()
        {
            var claimPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

            return Ok(new
            {
                Name = claimPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
                Aud = claimPrincipal.GetClaim(OpenIddictConstants.Claims.Audience),
                A = claimPrincipal.GetClaim("a"),
                B = claimPrincipal.GetClaim("b"),
                OrnekClaim = claimPrincipal.GetClaim("ornek-claim"),
            });
        }
    }


}
