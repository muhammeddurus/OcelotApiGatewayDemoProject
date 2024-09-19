using AuthServer.Models;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace AuthServer.Controllers
{
    public class ClientsController : Controller
    {
        /// <summary>
        /// Client ile ilgili işlemleri gerçekleştirebilmek için kullanılan sınıf.
        /// Bus sınıf sayesinde 
        /// ekleme silme güncelleme işlemlerini yürütebileceğiz.
        /// </summary>
        private readonly IOpenIddictApplicationManager _openIddictApplicationManager;

        public ClientsController(IOpenIddictApplicationManager openIddictApplicationManager)
        {
            _openIddictApplicationManager = openIddictApplicationManager;
        }
        [HttpGet]
        public async Task<IActionResult> CreateClient()
        {
            return View();
        }

        /// <summary>
        /// Authorization server a Client tanımlamasını yaptığımız yer.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateClient(ClientCreateViewModel model)
        {
            var client = await _openIddictApplicationManager.FindByClientIdAsync(model.ClientId);
            if (client is null)
            {
                await _openIddictApplicationManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = model.ClientId,
                    ClientSecret = model.ClientSecret,
                    DisplayName = model.DisplayName,
                    RedirectUris = { new(model.RedirectUri) },
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.Authorization,

                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "read",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "write",

                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                }
                });
                ViewBag.Message = "Client başarıyla oluşturulmuştur.";
                return View();
            }
            ViewBag.Message = "Client zaten mevcuttur.";
            return View();
        }
    }
}
