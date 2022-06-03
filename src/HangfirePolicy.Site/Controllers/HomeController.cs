using HangfirePolicy.Site.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HangfirePolicy.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "teste"),
                    new Claim("teste", "teste"),
                    new Claim("HangfireClaims", "PodeAcessarHangfire")
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(claimsPrincipal);

            return Redirect("/hangfire");
        }

        [Route("/error/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var erro = new ErrorViewModel { Codigo = id, Mensagem = "Ocorreu um erro inesperado." };
            if (id == 403)
                erro.Mensagem = "Acesso proibido. Você não tem acesso a esta página.";

            return View(erro);
        }
    }
}