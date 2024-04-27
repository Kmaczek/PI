using Azure;
using Azure.AI.OpenAI;
using Core.Model.Exceptions;
using Core.Model.Transaction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Domain.Logic.Chatbot
{
    public class TransactionClassifier : ITransactionClassifier
    {
        private const string OpenAiApiKey = "PI_OpenAiApiKey";
        private readonly string apiKey;
        private readonly string chatModel;
        private OpenAIClient openAIClient;
        private readonly ILogger<TransactionClassifier> _logger;
        private readonly List<ChatRequestMessage> systemPrompts = [];
        private readonly List<ChatRequestMessage> userPrompts = [];

        public TransactionClassifier(IConfigurationRoot configuration, ILogger<TransactionClassifier> logger)
        {
            _logger = logger;
            apiKey = configuration.GetSection(OpenAiApiKey).Value;
            if (string.IsNullOrEmpty(apiKey))
                throw new EmailServiceException($"Missing configuration for {nameof(Chatbot)} [{OpenAiApiKey}]");

            InitClient();
        }

        public void AssignCategoryInBatch(IEnumerable<TransactionModel> records)
        {
            _logger.LogInformation("Assigning categories to transactions");
            foreach (var record in records)
            {
                _logger.LogInformation(record.ToString());
            }

            var chatChoices = GetResponses(records);
            foreach (ChatChoice choice in chatChoices)
            {
                _logger.LogInformation(choice.Message.Content);
            }
        }

        private List<ChatChoice> GetResponses(IEnumerable<TransactionModel> records)
        {
            var take = 10;
            var loops = (int)Math.Ceiling((float)records.Count() / take);
            var chatChoices = new List<ChatChoice>();
            AddSystemPrompt(GetChatSystemPrompt());

            for (int i = 0; i < loops; i++)
            {
                var batchedRecords = records.Skip(take * i).Take(take);
                ResetUserPrompts();
                AddUserPrompt(GetUserPrompt(batchedRecords));

                Response<ChatCompletions> response = openAIClient.GetChatCompletions(CreateOptions());
                //OpenAiModels.Gpt35Turbo16k

                chatChoices.AddRange(response.Value.Choices);
            }

            return chatChoices;
        }

        private void AddSystemPrompt(string systemPrompt)
        {
            systemPrompts.Add(new ChatRequestSystemMessage(systemPrompt));
        }

        private void AddUserPrompt(string userPrompt)
        {
            userPrompts.Add(new ChatRequestUserMessage(userPrompt));
        }

        private void ResetUserPrompts()
        {
            userPrompts.Clear();
        }

        private string GetChatSystemPrompt()
        {
            var p = $"Twoje zadanie to przypisywanie kategorii do transakcji. Użyj tylko podanych kategorii:\n [\n" +
                $"{string.Join(",\n", Categories.All)} \n]" +
                $"\nJeżeli nie jesteś pewien co do kategorii, to wstaw '{Categories.NieWiadomo}'" +
                $"\n Nie generuj innych kategorii. Nie modyfikuj nazwy transakcji.";

            return p;
        }

        private string GetUserPrompt(IEnumerable<TransactionModel> transactions)
        {
            var p = $"Przypisz wcześniej podane kategorie do poniższych transakcji: {string.Join("\n", transactions)} ";

            return p;
        }

        private void InitClient()
        {
            openAIClient = new OpenAIClient(apiKey);
        }

        private ChatCompletionsOptions CreateOptions()
        {
            var chatOptions = new ChatCompletionsOptions()
            {
                DeploymentName = OpenAiModels.Gpt35Turbo16k,
                ChoiceCount = 1,
                Temperature = 0.01f
            };
            systemPrompts.ForEach(p => chatOptions.Messages.Add(p));
            userPrompts.ForEach(p => chatOptions.Messages.Add(p));

            return chatOptions;
        }
    }
}
