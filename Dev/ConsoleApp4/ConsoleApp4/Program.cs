using System;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using Amazon.Runtime;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().Wait() ;
            Console.ReadLine();
        }

        static async Task Run()
        {
            string bucketName = "test-some-random-unique-name-1122aabbcc";

            IAmazonS3 s3Client = new AmazonS3Client(RegionEndpoint.APSoutheast2);
            
            
            PutBucketRequest createBucketRequest = new PutBucketRequest();
            createBucketRequest.BucketName = bucketName;
            createBucketRequest.BucketRegion = S3Region.APS2;
            //await s3Client.PutBucketAsync(createBucketRequest);

            PutObjectRequest uploadRequest = new PutObjectRequest();
            uploadRequest.BucketName = bucketName;
            uploadRequest.FilePath = @"C:\temp\sample_file.txt";
            uploadRequest.Key = @"xyz/abc/test.txt";

            await s3Client.PutObjectAsync(uploadRequest);

        }

    }

}
