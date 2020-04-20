using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyPetApp.Security;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;

namespace MyPetApp.Web.Areas.Animal.Pages
{
    [Authorize]
    public class AnimalSearchModel : PageModel
    {
        ITenantSecurity _securityClient;
        public AnimalSearchModel(ITenantSecurity securityClient)
        {
            _securityClient = securityClient;
        }
        public class AnimalInfo
        {
            public string AnimalId { get; set; }
            public string Name { get; set; }
            public string Breed { get; set; }
            public int Age { get; set; }
            public string Image { get; set; }
        }

        [BindProperty]
        public List<AnimalInfo> AnimalSet { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public string AnimalId { get; set; }
        public void OnGet()
        {
 
        }
 
        public void OnPostSearch1()
        {
            RegionEndpoint region = Config.ConfigManager.GetRegion();
            string idToken = HttpContext.GetTokenAsync("id_token").Result;
            string identityPoolId = "ap-southeast-2:c8b11111-ea22-3333-4444-555555555c5a";
            string userPoolId = "ap-southeast-2_U634533B";
            string providerName = $"cognito-idp.ap-southeast-2.amazonaws.com/{userPoolId}";

            CognitoAWSCredentials cognitoCredentials = new CognitoAWSCredentials(identityPoolId, region);
            cognitoCredentials.AddLogin(providerName, idToken);

            IAmazonDynamoDB db = new AmazonDynamoDBClient(cognitoCredentials, region);
            string tableName = "Animals";
            Table animalTbl = Table.LoadTable(db, tableName);
   
            var singleItem = animalTbl.GetItemAsync(AnimalId).Result;

            AnimalSet = new List<AnimalInfo>();
            if (singleItem != null)
            {
                AnimalInfo info = new AnimalInfo();
                info.AnimalId = singleItem.ContainsKey("AnimalId") ? (string)singleItem["AnimalId"] : String.Empty;
                info.Name = singleItem.ContainsKey("Name") ? (string)singleItem["Name"] : String.Empty;
                info.Age = singleItem.ContainsKey("Age") ? int.Parse(singleItem["Age"]) : 0;
                info.Breed = singleItem.ContainsKey("Breed") ? (string)singleItem["Breed"] : String.Empty;
                info.Image = singleItem.ContainsKey("Image") ? (string)singleItem["Image"] : String.Empty;
                AnimalSet.Add(info);
            }
        }

        public void OnPostSearch2()
        {
            AWSCredentials stsCredentails=_securityClient.GetTenantCredentials();

            IAmazonDynamoDB db = new AmazonDynamoDBClient(stsCredentails, Amazon.RegionEndpoint.APSoutheast2);
            string tableName = "Animals";
            Table animalTbl = Table.LoadTable(db, tableName);

            GetItemOperationConfig config = new GetItemOperationConfig();
            config.AttributesToGet = new List<string>();
            config.AttributesToGet.Add("AnimalId");
            config.AttributesToGet.Add("Name");
            config.AttributesToGet.Add("Age");
           // config.AttributesToGet.Add("Breed");
            config.AttributesToGet.Add("Image");

            var singleItem = animalTbl.GetItemAsync(AnimalId,config).Result;
            AnimalSet = new List<AnimalInfo>();
            if (singleItem != null)
            {
                AnimalInfo info = new AnimalInfo();
                info.AnimalId = singleItem.ContainsKey("AnimalId")? (string)singleItem["AnimalId"]:String.Empty;
                info.Name = singleItem.ContainsKey("Name") ? (string)singleItem["Name"] : String.Empty;
                info.Age = singleItem.ContainsKey("Age") ? int.Parse(singleItem["Age"]) : 0;
                info.Breed = singleItem.ContainsKey("Breed") ? (string)singleItem["Breed"] : String.Empty;
                info.Image = singleItem.ContainsKey("Image") ? (string)singleItem["Image"] : String.Empty;
                AnimalSet.Add(info);
            }
        }

        public void OnPostSearch()
        {
            AWSCredentials stsCredentails = _securityClient.GetTenantCredentials();

            IAmazonDynamoDB db = new AmazonDynamoDBClient(stsCredentails, Amazon.RegionEndpoint.APSoutheast2);
            string tableName = "Animals";
            Table animalTbl = Table.LoadTable(db, tableName);

            GetItemOperationConfig config = new GetItemOperationConfig();
            config.AttributesToGet = new List<string>();
            config.AttributesToGet.Add("AnimalId");
            config.AttributesToGet.Add("Name");
            config.AttributesToGet.Add("Age");
            config.AttributesToGet.Add("Image");

            var singleItem = animalTbl.GetItemAsync(AnimalId, config).Result;
            
            IAmazonS3 s3 = new AmazonS3Client(stsCredentails, Amazon.RegionEndpoint.APSoutheast2);

            AnimalSet = new List<AnimalInfo>();
            if (singleItem != null)
            {
                AnimalInfo info = new AnimalInfo();
                info.AnimalId = singleItem.ContainsKey("AnimalId") ? (string)singleItem["AnimalId"] : String.Empty;
                info.Name = singleItem.ContainsKey("Name") ? (string)singleItem["Name"] : String.Empty;
                info.Age = singleItem.ContainsKey("Age") ? int.Parse(singleItem["Age"]) : 0;
                info.Breed = singleItem.ContainsKey("Breed") ? (string)singleItem["Breed"] : String.Empty;
                string imagePath = singleItem.ContainsKey("Image") ? (string)singleItem["Image"] : String.Empty;
                info.Image=s3.GeneratePreSignedURL("my-pet-app", imagePath, DateTime.Now.AddMinutes(5), null);
                AnimalSet.Add(info);
            }
        }
    }
}
