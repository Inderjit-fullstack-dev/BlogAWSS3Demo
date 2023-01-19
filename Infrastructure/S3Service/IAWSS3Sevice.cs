using Amazon.S3.Model;

namespace BlogAWSS3Demo.Infrastructure.S3Service
{
    public interface IAWSS3Sevice
    {
        public Task<ListBucketsResponse> ListBuckets();
        public Task<PutObjectResponse> WriteObject(string bucketName, IFormFile file);
        public Task<IEnumerable<object>> ListBucketObjects(string bucketName);
        public Task<object> GetBucketObject(string bucketName, string key);
        public Task<DeleteObjectResponse> DeleteBucketObject(string bucketName, string key);
    }
}
