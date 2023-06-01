using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace MediaFormatterClients.Controllers
{
	public class FileController : Controller
	{
		private readonly HttpClient _httpClient = null;
		private readonly IWebHostEnvironment _environment;
		private const string FOLDER_NAME = "Download";
		private const string _url = "http://localhost:5036/api/File/";
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
		public async Task<IActionResult> Upload(IFormFile formFile)
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
			string fileExtention = Path.GetExtension(formFile.FileName);
			var fileName = Guid.NewGuid().ToString() + fileExtention;
			var document = new Documentt
			{
				DocumentName = fileName,
				ContentType = formFile.ContentType,
				DocumentOriginalName = formFile.FileName,
				PathFile = Path.Combine(_environment.WebRootPath, FOLDER_NAME, fileName),
			};

			var pathFile = Path.Combine(_environment.WebRootPath, FOLDER_NAME, formFile.FileName);

			using (var uploading = new FileStream(pathFile, FileMode.Create))
			{
				await formFile.CopyToAsync(uploading);
				uploading.Close();
				ViewData["Message1"] = "The Selected File " + formFile.FileName + " Is Saved success...";
				HttpResponseMessage responseMessage = await _httpClient.PostAsJsonAsync(_url + "SaveFileInformation", document);
				switch (responseMessage.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						return View();

					case System.Net.HttpStatusCode.Conflict:
						if (System.IO.File.Exists(pathFile))
						{
							System.IO.File.Delete(pathFile);
						}
						return View("Error");

					case System.Net.HttpStatusCode.Forbidden:
						return StatusCode(StatusCodes.Status403Forbidden);

				}

				return View();
			}
		}
	}
}
