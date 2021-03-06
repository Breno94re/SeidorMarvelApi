using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProjetoSeidor.Swagger
{
    public class Header : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if (!operation.Tags[0].Name.ToLower().Contains("login"))
            {
                if (operation.Description != null && !operation.Description.ToLower().Contains("{token}"))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {

                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = "User token generated by login",
                        Required = true
                    });
                }
                
            }


        }

    }
}
