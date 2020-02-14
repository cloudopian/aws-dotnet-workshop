using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessKey = "AKIAYASBUYUORND23YUN";
            string secretKey = "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";
            RegionEndpoint region = RegionEndpoint.APSoutheast2;
            AWSCredentials cred = new BasicAWSCredentials(accessKey, secretKey);
            IAmazonSecurityTokenService stsClient = new AmazonSecurityTokenServiceClient(cred, region);

            AssumeRoleRequest stsRequest = new AssumeRoleRequest();
            stsRequest.DurationSeconds = 910;
            stsRequest.RoleArn = "arn:aws:iam::550967231773:role/MyAssumeRole";
            stsRequest.RoleSessionName = "MyAssumeRolesessionFromDotNet";

            AssumeRoleResponse temporaryCred = stsClient.AssumeRoleAsync(stsRequest).Result;

            IAmazonS3 client = new AmazonS3Client(temporaryCred.Credentials, region);

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
