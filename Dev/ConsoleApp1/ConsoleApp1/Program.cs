using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessKey = "AKIAYASBUYUORND23YUN";
            string secretKey = "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";
            AWSCredentials cred = new BasicAWSCredentials(accessKey, secretKey);
            IAmazonS3 client = new AmazonS3Client(cred, RegionEndpoint.APSoutheast2);

            ListBucketsRequest request = new ListBucketsRequest();

            ListBucketsResponse response = client.ListBucketsAsync(request).Result;
            foreach (S3Bucket b in response.Buckets)
            {
                Console.WriteLine(b.BucketName);
            }

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
