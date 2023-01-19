using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace BlogAWSS3Demo.Infrastructure.S3Service
{
    public class AWSS3Sevice : IAWSS3Sevice
    {
        private readonly AmazonS3Client _amazonS3Client;
        public AWSS3Sevice()
        {
            _amazonS3Client = new AmazonS3Client(RegionEndpoint.APSouth1);
        }

        public async Task<DeleteObjectResponse> DeleteBucketObject(string bucketName, string key)
        {
            try
            {
                return await _amazonS3Client.DeleteObjectAsync(bucketName, key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> GetBucketObject(string bucketName, string key)
        {
            try
            {
                var bucketObject = await _amazonS3Client.GetObjectAsync(new GetObjectRequest
                {
                    Key = key,
                    BucketName = bucketName,
                });

                var signedUrl = _amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = bucketObject.BucketName,
                    Key = bucketObject.Key,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                return new
                {
                    key,
                    url = signedUrl
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<object>> ListBucketObjects(string bucketName)
        {
            try
            {
                var bucketObjects = await _amazonS3Client.ListObjectsV2Async(new ListObjectsV2Request { 
                    BucketName = bucketName
                });

                return bucketObjects.S3Objects.Select(x =>
                {
                    var urlRequest = new GetPreSignedUrlRequest
                    {
                        Key = x.Key,
                        BucketName = bucketName,
                        Expires = DateTime.Now.AddHours(1)
                    };

                    return new {name = x.Key, url = _amazonS3Client.GetPreSignedURL(urlRequest) };
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListBucketsResponse> ListBuckets()
        {
            try
            {
                return await _amazonS3Client.ListBucketsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PutObjectResponse> WriteObject(string bucketName, IFormFile file)
        {
            try
            {
                if (file == null)
                    throw new Exception("Invalid file.");

                var putObjectRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = $"key-{Guid.NewGuid().ToString("n")[..8]}",
                    InputStream = file.OpenReadStream(),
                    
                };

                putObjectRequest.Metadata.Add("Content-Type", file.ContentType);
                return await _amazonS3Client.PutObjectAsync(putObjectRequest);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
