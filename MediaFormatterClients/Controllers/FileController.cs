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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile? formFile)
		{
			string downloadFolder = Path.Combine(_environment.WebRootPath, FOLDER_NAME);
			if (!Directory.Exists(downloadFolder))
			{
				Directory.CreateDirectory(downloadFolder);
			}
			string fileType = formFile.ContentType;

			if (fileType.StartsWith("image/"))
			{
				ViewData["Message"] = "This is a picture.";


			} else if (fileType.StartsWith("text/"))
			{
				ViewData["Message"] = "It's a text file.";


			} else if (fileType.StartsWith("application/csv"))
			{
				ViewData["Message"] = "It's a word or pdf file.";


			}
			else
			{
				ViewData["Message"] = "Save failed";
				ViewData["Message1"] = "File unknown type";
				return View();
			}

			var pathFile = Path.Combine(_environment.WebRootPath, FOLDER_NAME, formFile.FileName);

			using (var uploading = new FileStream(pathFile, FileMode.Create))
			{
				await formFile.CopyToAsync(uploading);
				uploading.Close();
				ViewData["Message1"] = "The Selected File " + formFile.FileName + " Is Saved success...";

				return View();
			}
		}
	}
}
