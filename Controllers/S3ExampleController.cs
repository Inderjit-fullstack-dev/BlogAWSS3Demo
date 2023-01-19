using BlogAWSS3Demo.Infrastructure.S3Service;
using Microsoft.AspNetCore.Mvc;

namespace BlogAWSS3Demo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class S3ExampleController : ControllerBase
    {
        private readonly IAWSS3Sevice _s3Service;

        public S3ExampleController(IAWSS3Sevice s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpGet]
        public async Task<IActionResult> ListBuckets()
        {
            try
            {
                var buckets =  await _s3Service.ListBuckets();
                return Ok(buckets);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{bucketName}")]
        public async Task<IActionResult> UploadObjectToBucket(string bucketName, IFormFile file)
        {
            try
            {
                var response = await _s3Service.WriteObject(bucketName, file);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{bucketName}")]
        public async Task<IActionResult> GetBucketObjects(string bucketName)
        {
            try
            {
                var response = await _s3Service.ListBucketObjects(bucketName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBucketObject(string bucketName, string key)
        {
            try
            {
                var response = await _s3Service.GetBucketObject(bucketName, key);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBucketObject(string bucketName, string key)
        {
            try
            {
                var response = await _s3Service.DeleteBucketObject(bucketName, key);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
