using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Core.Domain.Logic.EmailGeneration
{
    public class EmailAssembler
    {
        private readonly string DateTimeTag = "date_time";
        public string HtmlEmail { get; set; }
        protected List<IHtmlGenerator> HtmlGenerators { get; } = new List<IHtmlGenerator>();

        public EmailAssembler(List<IHtmlGenerator> htmlGenerators)
        {
            HtmlEmail = GetEmailTemplate();
            if(htmlGenerators != null)
                HtmlGenerators = htmlGenerators;
        }

        public string GenerateEmail()
        {
            var sb = new StringBuilder(HtmlEmail);

            sb.Replace($"[[{DateTimeTag}]]", DateTime.Now.ToShortDateString());

            foreach (var htmlGenerator in HtmlGenerators)
            {
                sb.Replace($"[[{htmlGenerator.HtmlKey}]]", htmlGenerator.GenerateBody());
            }

            return sb.ToString();
        }

        protected static string GetEmailTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Core.Domain.Logic.EmailGeneration.EmailTemplate.html";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }

    }
}
