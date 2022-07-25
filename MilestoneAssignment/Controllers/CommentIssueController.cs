using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
namespace testAPI.Controllers;

[ApiController]
public class GitHookController : Controller
{
    [HttpPost("")]
    [Route("/")]
    public async Task<IActionResult> post()
    {
        using (var reader = new StreamReader(Request.Body))
        {
            var txt = await reader.ReadToEndAsync();
            JObject json = JObject.Parse(txt);
            if (json["action"].ToString().Equals("opened"))
            {
                HttpClient client = new HttpClient();
                var token = "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";

                client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);
                var postData = JToken.Parse("{\"body\": \"Thanks for submitting this issue. Our Team will get back to you as soon as possible!\"}");

                var stringContent = new StringContent(postData.ToString(), UnicodeEncoding.UTF8, "application/json");
                client.PostAsync(json["issue"]["url"].ToString(), stringContent);


            }

        }
        return Ok();
    }
}