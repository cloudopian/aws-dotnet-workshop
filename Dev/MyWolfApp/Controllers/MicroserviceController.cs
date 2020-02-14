using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWolfApp.Models;

namespace MyWolfApp.Controllers
{
    public class MicroserviceController : Controller
    {

        [HttpGet]
        public IActionResult  MicroserviceTest()
        {
            ServiceData initialValue=new ServiceData();
            initialValue.InputName="John";
            initialValue.InputAge=35;
            initialValue.Error="";
            return View(initialValue);
        }


        [HttpPost]
        public IActionResult  MicroserviceTest(ServiceData serviceData)
        {
            ServiceData result=new ServiceData();
            result.InputName=serviceData.InputName;
            result.InputAge=serviceData.InputAge;

            result.OutputAge=45;
            result.OutputName=serviceData.InputName+"binogo";
            result.Error="Web service timeout and can't be invoked";
            Console.WriteLine("Test");
            return View("MicroServiceTest",result);
        }
        

      
    }
}
