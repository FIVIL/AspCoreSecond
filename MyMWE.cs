using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace seccond
{
    public static class MyMWE
    {
        public static void CounterMW(this IApplicationBuilder app)
        {
            //app.Use(async (c, n) =>
            //{
            //    var co = c.RequestServices.GetRequiredService<counter>();
            //    co.count++;
            //    await n.Invoke();
            //});
            app.UseMiddleware<MySeccondMW>();
        }
    }
}
