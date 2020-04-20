using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPetApp.Web.Security
{
    public class CustomAWSOptions : AWSOptions
    {
        IAmazonCognitoIdentityProvider _identityProvider;
        CognitoUserPool _userPool;
        public CustomAWSOptions(IAmazonCognitoIdentityProvider identityProvider, CognitoUserPool userPool)
        {
            _identityProvider = identityProvider;
            _userPool = userPool;
        }

        public new AWSCredentials Credentials
        {
            get
            {
                string username = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                return null;
            }

            set
            {
                int i = 0;
            }
        }



    }
}
