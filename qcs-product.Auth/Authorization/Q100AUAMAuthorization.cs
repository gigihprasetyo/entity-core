using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace qcs_product.Auth.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Q100AUAMAuthorization : Attribute, IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<Q100AUAMAuthorizationFilter>();
            return filter;
        }
    }
}