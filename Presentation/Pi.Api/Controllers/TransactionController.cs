using Azure.Core;
using Core.Domain.Logic.Chatbot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pi.Api.Models.Request;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionClassifier transactionClassifier;

        public TransactionController(
            ILogger<TransactionController> logger,
            ITransactionClassifier transactionClassifier)
        {
            _logger = logger;
            this.transactionClassifier = transactionClassifier;
        }

        [HttpPost("classify")]
        public async Task<IActionResult> Get(ClassifyTransactionsRequest classifyTransactionsRequest)
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();

            transactionClassifier.AssignCategoryInBatch(classifyTransactionsRequest.ToTransactionModel());

            return Ok();
        }
    }
}
