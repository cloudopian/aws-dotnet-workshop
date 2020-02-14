using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            RegionEndpoint region = RegionEndpoint.APSoutheast2;
            IAmazonS3 client = new AmazonS3Client(region);

            ListBucketsRequest request = new ListBucketsRequest();

            ListBucketsResponse response = client.ListBucketsAsync(request).Result;
            foreach (S3Bucket b in response.Buckets)
            {
                Console.WriteLine(b.BucketName);
            }

            Console.ReadLine();
        }
    }
}
