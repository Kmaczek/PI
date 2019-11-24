using Core.Model.FlatsModels;

namespace Core.Domain.Logic.EmailGeneration
{
    public class OtodomHtmlGenerator : HtmlBase, IHtmlGenerator
    {
        private FlatOutput flatOutput = null;
        public string HtmlKey => "otodom_body";

        public override string HtmlTemplateName => "Core.Domain.Logic.EmailGeneration.OtodomTemplate.html";

        public OtodomHtmlGenerator()
        {
            
        }

        public void SetBodyData(object data)
        {
            this.flatOutput = data as FlatOutput;
            if(this.flatOutput != null)
            {
                DataDictionary.Add("private_flats", flatOutput.PrivateFlatsByCategory);
                DataDictionary.Add("all_flats", flatOutput.AllFlatsByCategory);
            }
        }

        public string GenerateBody()
        {
            if (flatOutput == null)
            {
                return "Brak danych";
            }

            return CombineHtmlWithData();
        }
    }
}
