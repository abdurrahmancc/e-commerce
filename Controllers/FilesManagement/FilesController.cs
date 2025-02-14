using e_commerce.DTOs.Products;
using e_commerce.Services.FilesManagement;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.FileManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FilestService _filesService;

        public FilesController(FilestService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("image/upload")]
        public async Task<IActionResult> UploadImage(IFormFile  file)
        {
            var filePath = await _filesService.UploadImageAsync(file);
            if (filePath != null)
            {
                return Ok(ApiResponse<string>.SuccessResponse(filePath, 200, "successful"));
            }

            return BadRequest(ApiResponse<object>.ErrorResponse(new List<string> { "No file uploaded" }, 400, "Validation failed"));
        }


        [HttpDelete("image/delete")]
        public IActionResult DeleteFile(string filePath)
        {
            var result = _filesService.DeleteImageAsync(filePath);

            if (result.Result)
            {
                return Ok(ApiResponse<string>.SuccessResponse(filePath, 200, "File deleted successfully."));
            }

            return NotFound(ApiResponse<object>.ErrorResponse(new List<string> { "File not found." }, 404, "Validation failed"));
        }
    }
}
