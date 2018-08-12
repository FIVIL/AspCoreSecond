using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace seccond
{
    public class Startup
    {

        //private static counter co = new counter();
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //singeltone lifespan: yek nemune dar kolle barname
            services.AddSingleton<counter>();
            services.AddSingleton<MySession>();
            //transient life span: yek nemune jadid be ezaye harbar darkhast object
            services.AddTransient<MySeccondMW>();
            //scoped life span: yek nemune jadid be ezaye har req http
            services.AddScoped<SessionData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.CounterMW();

            app.Map("/DL", app2 =>
            {
                app2.Run(async (c) =>
                {
                    var Path = System.IO.Path.Combine(env.ContentRootPath, "Files", "TextFile.txt");
                    var content = await System.IO.File.ReadAllTextAsync(Path);
                    await c.Response.WriteAsync(content);
                });
            });
            app.Use(async (c, n) =>
            {
                var key1 = c.Request.Query["key1"];
                c.Request.Headers.Add(new KeyValuePair<string, StringValues>("key1", key1));
                await n.Invoke();
            });
            app.Map("/count", app2 =>
            {
                app2.Run(async c =>
                {
                    var co = c.RequestServices.GetRequiredService<counter>();
                    c.Response.ContentType = "application/json";
                    await c.Response.WriteAsync(co.ToString());
                });
            });
            //agar ke req property khasti dasht vared mishavad
            app.MapWhen(c => c.Request.Headers.ContainsKey("h"), app2 =>
             {
                 app2.Use(async (c, next) =>
                 {
                     await c.Response.WriteAsync("have h header.");
                     await next.Invoke();
                 });
             });

            //zamani ke url shamel masir khasi shavad pardazesh req vared in mishavad
            app.Map("/map", app2 =>
             {
                 app2.Map("/map2", app3 =>
                 {
                     app3.Run(async c =>
                     {
                         await c.Response.WriteAsync("map3");
                     });
                 });
                 app2.Use(async (c, next) =>
                 {
                     await c.Response.WriteAsync("<h1>map </h1>");
                     await next.Invoke();
                 });
                 app2.Run(async c =>
                 {
                     await c.Response.WriteAsync("map2");
                 });
             });

            //req ra ejra karde  va sepas be MW badi pass midahad.
            app.Use(async (c, next) =>
            {
                await c.Response.WriteAsync("<h1>Use </h1>");
                await next.Invoke();
            });

            //be ejra req mipardazad va pasokh ro bar migardanad.
            app.Run(async c =>
            {
                await c.Response.WriteAsync("First MW!");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("seccond MW");
            });
        }
    }
}
