namespace Core.Domain.Logic.EmailGeneration
{
    public interface IHtmlGenerator
    {
        string HtmlKey { get; }

        string GenerateBody();

        void SetBodyData(object data);
    }
}