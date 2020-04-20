using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using Amazon.SecurityToken.Model;
using Amazon.SecurityToken;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace MyPetApp.Security
{
    public class TenantSecurity : ITenantSecurity
    {
        IAmazonSecurityTokenService _sts;
        ClaimsPrincipal _claims;
        public TenantSecurity(IAmazonSecurityTokenService sts, ClaimsPrincipal claims)
        {
            _sts = sts;
            _claims = claims;
        }
        public AWSCredentials GetTenantCredential()
        {
            string claimType = "custom:tenant-id";
            string tierType = "custom:tier";

            var tenantId = _claims.Claims.Where(c => c.Type == claimType).FirstOrDefault().Value;
            var membership = (MembershipTier)Enum.Parse(typeof(MembershipTier), _claims.Claims
                .Where(c => c.Type == tierType).FirstOrDefault().Value);

            string template = GetMembershipBasedPolicy(membership);
            
            string dynamicPolicy = template
            .Replace("###tenant-id###", tenantId)
            .Replace("###table-arn###", "arn:aws:dynamodb:ap-southeast-2:123456789123:table/Animals");
            
            AssumeRoleRequest request = new AssumeRoleRequest();
            request.DurationSeconds = 900;
            request.RoleArn = "arn:aws:iam::123456789123:role/MyPetApp-SecurityModuleRole";
            request.RoleSessionName = $"MyPetApp-Session-{tenantId}";
            request.Policy = dynamicPolicy;

            return _sts.AssumeRoleAsync(request).Result.Credentials;
        }

        public AWSCredentials GetTenantCredentials()
        {
            string claimType = "custom:tenant-id";
            string tierType = "custom:tier";
            var tenantId = _claims.Claims.Where(c => c.Type == claimType).FirstOrDefault().Value;
            MembershipTier membership = (MembershipTier)Enum.Parse(typeof(MembershipTier), _claims.Claims
                .Where(c => c.Type == tierType).FirstOrDefault().Value);

            string template = GetPolicyTemplate("TestPolicyTemplate");
            AssumeRoleRequest request = new AssumeRoleRequest();
            request.DurationSeconds = 900;
            request.RoleArn = "arn:aws:iam::123456789123:role/MyPetApp-SecurityModuleRole";
            request.RoleSessionName = $"MyPetApp-Session-{tenantId}";

            string dynamicPolicy = template
                .Replace("###tenant-id###", tenantId)
                .Replace("###bucket-name###", "my-pet-app")
                .Replace("###table-arn###", "arn:aws:dynamodb:ap-southeast-2:123456789123:table/Animals");

            request.Policy = dynamicPolicy;

            return _sts.AssumeRoleAsync(request).Result.Credentials;
        }

        private string GetMembershipBasedPolicy(MembershipTier membership)
        {
            string policyTemplateFile = String.Empty;
            switch (membership)
            {
                case MembershipTier.Silver:
                    {
                        policyTemplateFile = "SilverTierPolicyTemplate";
                        break;
                    }
                case MembershipTier.Gold:
                    {
                        policyTemplateFile = "GoldTierPolicyTemplate";
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Found an unknown membership type");
                    }
            }
            return GetPolicyTemplate(policyTemplateFile);
        }

        private string GetPolicyTemplate(string templateName)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string templateFilePath = Path.Combine(assemblyFolder, "PolicyTemplates", $"{templateName}.json");
            using (StreamReader reader = new StreamReader(templateFilePath))
            {
                return reader.ReadToEnd();
            }
        }
    }


}
