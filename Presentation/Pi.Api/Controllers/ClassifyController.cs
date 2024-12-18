using Core.Domain.Logic.Chatbot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pi.Api.Models.Request;
using System.IO;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("classify")]
    public class ClassifyController : SecureController
    {
        private readonly ILogger<ClassifyController> _logger;
        private readonly ITransactionClassifier transactionClassifier;

        public ClassifyController(
            ILogger<ClassifyController> logger,
            ITransactionClassifier transactionClassifier)
        {
            _logger = logger;
            this.transactionClassifier = transactionClassifier;
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> Get(ClassifyTransactionsRequest classifyTransactionsRequest)
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            transactionClassifier.AssignCategoryInBatch(classifyTransactionsRequest.ToTransactionModel());

            return Ok();
        }
    }
}
