using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using WebApp1.Models;

namespace WebApp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]  // TODO: この行を追加
        public IActionResult Privacy()
        {
            var user = User;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task Weather()
        {
            // アクセストークンを取得
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var httpClient = _httpClientFactory.CreateClient();
            // HTTP リクエストのメッセージを生成
            // TODO: URI には Web API のデバッグ時の URI をセット
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://localhost:5001/weatherforecast"
            );
            // アクセストークンをヘッダーに追加
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // Web API をコール
            var response = await httpClient.SendAsync(request);

            _logger.LogDebug(response.StatusCode.ToString());

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
        }
    }
}