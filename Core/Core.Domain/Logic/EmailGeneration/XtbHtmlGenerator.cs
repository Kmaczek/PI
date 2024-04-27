using Core.Common.Logging;
using Core.Model;
using System;
using System.Globalization;

namespace Core.Domain.Logic.EmailGeneration
{
    public class XtbHtmlGenerator : HtmlBase, IHtmlGenerator
    {
        private XtbOutput xtbOutput;

        public string HtmlKey => "xtb_body";
        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.XtbTemplate.html";

        public ILogger Log { get; }

        public XtbHtmlGenerator(ILogger log) : base()
        {
            Log = log;
        }

        public string GenerateBody()
        {
            if (xtbOutput == null)
            {
                return "Brak danych";
            }

            return CombineHtmlWithData();
        }

        public void SetBodyData(object xtbData)
        {
            xtbOutput = xtbData as XtbOutput;

            if(xtbOutput != null)
            {
                try
                {
                    DataDictionary.Add("balance", xtbOutput.Balance?.ToString(CultureInfo.InvariantCulture));
                    DataDictionary.Add("gain", xtbOutput.Gain?.ToString(CultureInfo.InvariantCulture));
                }
                catch (Exception e)
                {
                    if (xtbOutput == null)
                    {
                        Log.Error("Can't create HTML body. Xtb data is empty.");
                    }
                    else
                    {
                        Log.Error($"Can't create HTML body out of this data: Balance={this.xtbOutput.Balance} Gain={this.xtbOutput.Gain}");
                    }
                    Log.Error("XtbHtmlGenerator error.", e);

                    return;
                }
            }
        }
    }
}
