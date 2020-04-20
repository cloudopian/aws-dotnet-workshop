using Amazon.CognitoIdentity.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.AspNetCore.Identity.Cognito
{
    public partial class CognitoUserStore<TUser> : IUserLoginStore<TUser> where TUser : CognitoUser
    {
        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            GetIdRequest r = new GetIdRequest();
            r.AccountId = "919173020320";
            r.IdentityPoolId = "ap-southeast-2:67c062aa-1a19-49da-b30c-8a5b0f2287f2";
            r.Logins.Add("accounts.google.com", loginProvider);

            var t=_identityPoolClient.GetIdAsync(r).Result;
            //Amazon.CognitoIdentity.CognitoAWSCredentials creds=new CognitoIdentity.CognitoAWSCredentials("")


            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
