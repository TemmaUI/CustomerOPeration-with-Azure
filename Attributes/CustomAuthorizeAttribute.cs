using Microsoft.AspNetCore.Authorization;
using System;

namespace AzureCustomerOPeration.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationRequirement
    {
        public string Role { get; set; }

        public CustomAuthorizeAttribute(string role)
        {
            Role = role ?? throw new ArgumentNullException(nameof(role));
        }
    }
}
