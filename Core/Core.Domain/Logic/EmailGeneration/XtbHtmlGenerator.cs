using Core.Model;
using System.Globalization;

namespace Core.Domain.Logic.EmailGeneration
{
    public class XtbHtmlGenerator : HtmlBase, IHtmlGenerator
    {
        private XtbOutput xtbOutput;

        public string HtmlKey => "xtb_body";
        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.XtbTemplate.html";

        public XtbHtmlGenerator() : base()
        {
        }

        public XtbHtmlGenerator(XtbOutput xtbOutput)
        {
            this.xtbOutput = xtbOutput;
        }

        public string GenerateBody()
        {
            DataDictionary.Add("balance", xtbOutput.Balance?.ToString(CultureInfo.InvariantCulture));
            DataDictionary.Add("gain", xtbOutput.Gain?.ToString(CultureInfo.InvariantCulture));

            return CombineHtmlWithData();
        }
    }
}
