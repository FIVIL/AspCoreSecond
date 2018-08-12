using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seccond
{
    public class MySeccondMW : IMiddleware
    {
        private readonly counter c;
        public MySeccondMW(counter _C)
        {
            c = _C;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {        
            c.count++;
            await next.Invoke(context);
        }
    }
}
