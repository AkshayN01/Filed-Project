using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Filed.PaymentGateway.Library.Dependency
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {

        public SwaggerDocumentFilter(IHostingEnvironment env)
        {
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext documentFilterContext)
        {
            List<OpenApiServer> servers = new List<OpenApiServer>()
            {
                new OpenApiServer() { Url = "/" }
            };

            swaggerDoc.Servers = servers;
        }
    }
}
