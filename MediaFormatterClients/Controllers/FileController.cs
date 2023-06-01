using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MediaFormatterClients.Controllers
{
	public class FileController : Controller
	{
		private readonly HttpClient _httpClient = null;
		private readonly IWebHostEnvironment _environment;
		private const string FOLDER_NAME = "Download";

		public FileController(IWebHostEnvironment webHost)
		{
			_environment = webHost;
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
		}

		[HttpGet]
		public async Task<IActionResult> Upload()
		{
			ViewBag.LeftMenu = true;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile formFile)
		{
			string downloadFolder = Path.Combine(_environment.WebRootPath, FOLDER_NAME);
			if (!Directory.Exists(downloadFolder))
			{
				Directory.CreateDirectory(downloadFolder);
			}
			var fileName = formFile.FileName;
			var pathFile = Path.Combine(_environment.WebRootPath, FOLDER_NAME, fileName);

			using (var uploading = new FileStream(pathFile, FileMode.Create))
			{
				await formFile.CopyToAsync(uploading);
				uploading.Close();
				ViewData["Message"] = "The Selected File " + formFile.FileName + " Is Saved success...";
				ViewBag.LeftMenu = true;
				return View();
			}
		}
	}
}
