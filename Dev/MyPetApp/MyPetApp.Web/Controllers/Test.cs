using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyPetApp.Web.Models;

namespace MyPetApp.Web.Controllers
{
    public class Test : Controller
    {
        IAmazonDynamoDB _dbProvider;
        IAuthenticationService _service;

        public Test(IAmazonDynamoDB db,  IAuthenticationService service)
        {
            _dbProvider = db;
            _service = service;
        }

    
        public ActionResult Index()
        {
            //works nicely
            var currentClaims = this.User.Claims;

            var kk = _service.AuthenticateAsync(this.HttpContext, IdentityConstants.ApplicationScheme).Result;
            CognitoAWSCredentials cogCred = new CognitoAWSCredentials("ap-southeast-2:67c062aa-1a19-49da-b30c-8a5b0f2287f2", Amazon.RegionEndpoint.APSoutheast2);

            string provider = "cognito-idp.ap-southeast-2.amazonaws.com/ap-southeast-2_FFDK2FLGl";
           // cogCred.AddLogin(provider, kk.Properties.Items[".Token.id_token"]);


            ImmutableCredentials tempCred = cogCred.GetCredentials();

            string accessKey = tempCred.AccessKey;
            string secretKey = tempCred.SecretKey;


            IAmazonDynamoDB db = new AmazonDynamoDBClient(cogCred, Amazon.RegionEndpoint.APSoutheast2);



            string tableName = "Animals";
            Table animalTbl = Table.LoadTable(db, tableName);
            List<AnimalInfo> result = new List<AnimalInfo>();


            var singleItem = animalTbl.GetItemAsync("ap-southeast-2:7926aa93-dd73-482d-83fd-7bc2efac7cd6").Result;
            if (singleItem != null) { }
            AnimalInfo info = new AnimalInfo();
            info.AnimalId = singleItem["AnimalId"];
            info.Name = singleItem["Name"];
            info.Age = singleItem["Age"].AsInt();
            info.Breed = singleItem["Breed"];
            result.Add(info);
        

            //ScanFilter scanFilter = new ScanFilter();
            //scanFilter.AddCondition("AnimalId", ScanOperator.Contains, "07738fa2-b0ee-4799-be82-f9088855df80");
            //Search search = animalTbl.Scan(scanFilter);
            //do
            //{
            //    var items = search.GetNextSetAsync().Result;
            //    foreach (var i in items)
            //    {
            //        AnimalInfo info = new AnimalInfo();
            //        info.AnimalId = i["AnimalId"];
            //        info.Name = i["Name"];
            //        info.Age = i["Age"].AsInt();
            //        info.Breed = i["Breed"];
            //        result.Add(info);
            //    }
            //} while (!search.IsDone);


            return View(result);
        }

        // GET: Step/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Step/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Step/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}