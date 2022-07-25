using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
namespace testAPI.Controllers;

[ApiController]
public class GitHookController : Controller
{
    [HttpPost("")]
    [Route("/")]
    public async Task<IActionResult> Receive()
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
                var resposne = await client.PostAsync(json["issue"]["url"].ToString(), stringContent);
                resposne.EnsureSuccessStatusCode();
                string content = await resposne.Content.ReadAsStringAsync();
            }
        }
        return Ok();
    }
}