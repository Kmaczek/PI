using Core.Model;

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
            DataDictionary.Add("balance", xtbOutput.Balance?.ToString());
            DataDictionary.Add("gain", xtbOutput.Gain?.ToString());

            return CombineHtmlWithData();
        }
    }
}
