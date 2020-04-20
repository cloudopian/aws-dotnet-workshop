using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyPetApp.Security
{
    public interface ITenantSecurity
    {
        public AWSCredentials GetTenantCredentials();
    }
}
