
using Razor.Templating.Core;

namespace PersonalAccounting.BlazorApp.Services
{
    public class HtmlGenerator
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public HtmlGenerator(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment=webHostEnvironment;
        }

        public async Task<string> RenderAndExport(string reportName, object model)
        {
            //var path = System.IO.Path.Combine(webHostEnvironment.ContentRootPath, "components", "Reports", reportName);

            var res = await RazorTemplateEngine.RenderAsync($"~/Views/{reportName}", model);

            //var exists = System.IO.File.Exists(path);
            //var res = await razorViewRenderer.RenderAsync(path, model);

            return res;
        }


    }
}
