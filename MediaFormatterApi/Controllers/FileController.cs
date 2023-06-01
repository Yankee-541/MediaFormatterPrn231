using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace MediaFormatterApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FileController : ControllerBase
	{
		public FileController()
		{
			
		}

		[HttpPost]
		[Route("SaveFileInformation")]
		[Produces("application/json")]
		public async Task<IActionResult> SaveFileInformation([FromBody] Documentt document)
		{
			var documentEntity = new Documentt
			{
				DocumentName = document.DocumentName,
				DocumentOriginalName = document.DocumentOriginalName,
				ContentType = document.ContentType,
				PathFile = document.PathFile,
			};
			return Ok(documentEntity);
		}
	}
}
