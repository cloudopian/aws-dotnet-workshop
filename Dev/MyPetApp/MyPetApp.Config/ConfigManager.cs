using Amazon;
using Amazon.Runtime;
using System;

namespace MyPetApp.Config
{
    public class ConfigManager
    {
        static string accessKey = "<Your account's IAM user's access key or setup seucrity in a way your program has access to AWS>";
        static string secretKey = "<Your account's IAM user's secret key or setup seucrity in a way your program has access to AWS>";

        public static AWSCredentials GetCredentails()
        {
            AWSCredentials creds = new BasicAWSCredentials(accessKey, secretKey);
            return creds;
        }

        public static RegionEndpoint GetRegion()
        {
            return RegionEndpoint.APSoutheast2;
        }
    }
}
