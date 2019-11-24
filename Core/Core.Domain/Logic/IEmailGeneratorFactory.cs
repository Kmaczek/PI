using Core.Domain.Logic.EmailGeneration;

namespace Core.Domain.Logic
{
    public interface IEmailGeneratorFactory
    {
        IHtmlGenerator GetGenerator(EmailGenerator generatorType);
    }
}