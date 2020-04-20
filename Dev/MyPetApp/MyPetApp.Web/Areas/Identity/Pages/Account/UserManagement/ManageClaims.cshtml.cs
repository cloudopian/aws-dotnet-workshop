using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyPetApp.Web.Areas.Identity.Pages.Account.Claims
{
    public class ManageClaimsModel : PageModel
    {
        IAmazonCognitoIdentityProvider _identityProvider;
        CognitoUserPool _userPool;
        public ManageClaimsModel(IAmazonCognitoIdentityProvider provider, CognitoUserPool userPool)
        {
            _identityProvider = provider;
            _userPool = userPool;
        }
        public class UserInfoSet
        {
            public string Username { get; set; }
            public bool IsEnabled { get; set; }

            public DateTime CreatedDate { get; set; }
            public IEnumerable<KeyValuePair<string,string>> Attributes { get; set; }

        }
        [BindProperty]
        public KeyValuePair<string,string> CustomClaim { get; set; }
        
        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public string SearchEmailAddress { get; set; }

        [BindProperty]
        public UserInfoSet UserInfo { get; set; }


        public void OnGet()
        {
            CustomClaim = new KeyValuePair<string, string>();
        }

        public void OnPostSearch()
        {

            try
            {
                var res = _identityProvider.AdminGetUserAsync(new AdminGetUserRequest() { Username = SearchEmailAddress, UserPoolId = _userPool.PoolID }).Result;
                Message = "Found a user";
                UserInfoSet ui = new UserInfoSet();
                ui.Username = res.Username;
                List<KeyValuePair<string, string>> att = new List<KeyValuePair<string, string>>();
                foreach (var a in res.UserAttributes)
                {
                    att.Add(new KeyValuePair<string, string>(a.Name, a.Value));
                }
                ui.Attributes = att;
                ui.IsEnabled = res.Enabled;
                ui.CreatedDate = res.UserCreateDate;
                UserInfo = ui;

            }
            catch (System.AggregateException ex)
            {
                if (ex.InnerException.GetType() == typeof(UserNotFoundException))
                {
                    Message = $"There is no such user with the email address {SearchEmailAddress}";
                }
            }
        }


        public void OnPostCreateClaim()
        {
            List<AttributeType> att = new List<AttributeType>();
            att.Add(new AttributeType() { Name = CustomClaim.Key, Value = CustomClaim.Value });
            try
            {
                AdminUpdateUserAttributesRequest updateRequest = new AdminUpdateUserAttributesRequest();
                updateRequest.Username = UserInfo.Username;
                updateRequest.UserPoolId = _userPool.PoolID;
                updateRequest.UserAttributes = att;

                var res = _identityProvider.AdminUpdateUserAttributesAsync(updateRequest).Result;
                if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Message = "Successfully added the new attribute";
                }

                var newUserInfo = _identityProvider.AdminGetUserAsync(new AdminGetUserRequest() { Username = UserInfo.Username, UserPoolId = _userPool.PoolID }).Result;
                Message = "Found a user";
                UserInfoSet ui = new UserInfoSet();
                ui.Username = newUserInfo.Username;
                List<KeyValuePair<string, string>> newAtt = new List<KeyValuePair<string, string>>();
                foreach (var a in newUserInfo.UserAttributes)
                {
                    newAtt.Add(new KeyValuePair<string, string>(a.Name, a.Value));
                }
                ui.Attributes = newAtt;
                ui.IsEnabled = newUserInfo.Enabled;
                ui.CreatedDate = newUserInfo.UserCreateDate;
                UserInfo = ui;
            }catch(System.AggregateException ex)
            {
                Message = ex.Message;
            }

            Console.WriteLine("test");
        }
  


    }
}