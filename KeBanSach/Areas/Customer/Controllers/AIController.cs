using KeBanSach.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeBanSach.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AIController : Controller
    {
        private readonly APIModel _apiModel;

        public AIController(IWebHostEnvironment webHostEnvironment)
        {
            _apiModel = new APIModel(webHostEnvironment);
        }
        public IActionResult GenerateContent()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GenerateContent(string inputText)
        {
            string apiKey = "YOUR API_KEY RIGHT HERE";
            var result = await _apiModel.GenerateContentAsync(apiKey, inputText);
            if (result != null)
            {
                ViewBag.GeneratedContent = result;
                string generatedContent = ViewBag.GeneratedContent;
                if (!string.IsNullOrEmpty(generatedContent))
                {
                    int startIndex = generatedContent.IndexOf("\"text\" : \"") + 9;  
                    int endIndex = generatedContent.IndexOf("\"", startIndex);  
                    if (startIndex > 8 && endIndex > startIndex)
                    {
                        generatedContent = generatedContent.Substring(startIndex, endIndex - startIndex);
                        generatedContent = generatedContent.Replace("\n", "<br />");
                    }
                }
                ViewBag.GeneratedContent = generatedContent;
                return View();
            }
            ViewBag.ErrorMessage = "Error generating content";
            return View();
        }
    }
}
