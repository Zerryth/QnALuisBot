using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using System.Collections.Generic;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using LuisBot.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }
        
        // Intent
        public const string EatSushi = "EatSushi";
        // QnA
        static readonly string QnAHostName = ConfigurationManager.AppSettings["QnAEndpointHostName"];
        static readonly string KBId = ConfigurationManager.AppSettings["KnowledgeBaseId"];
        static readonly string QnAEndpointKey = ConfigurationManager.AppSettings["QnAAuthKey"];

        //WINONA'S KEYS
        static readonly string BiologyQnAHostName = ConfigurationManager.AppSettings["BiologyQnAEndpointHostName"];
        static readonly string BiologyKBId = ConfigurationManager.AppSettings["BiologyKBId"];
        static readonly string BiologyEndpointKey = ConfigurationManager.AppSettings["BiologyQnAAuthKey"];



        // Instantiate the QnAMakerService class for each of your knowledge bases
        public QnAMakerService BitLockerQnAService = new QnAMakerService(QnAHostName, KBId, QnAEndpointKey);

        // WINONA'S KBs
        public QnAMakerService biologyQnAService = new QnAMakerService
        (BiologyQnAHostName, BiologyKBId, BiologyEndpointKey);
        //("https://qnamakerwin.azurewebsites.net/qnamaker", "3b2e0ac6-11bd-48c8-bdfb-759462d8502a", "45217269-2304-4b74-9155-e98795a87b12");

        public QnAMakerService sociologyQnAService = new QnAMakerService
        ("https://qnamakerwin.azurewebsites.net/qnamaker", "43c68327-b8c1-4398-882b-11b09cfdae7c", "f038dceb-289d-445f-87dc-81cefe57eed6");
        public QnAMakerService geologyQnAService = new QnAMakerService
        ("https://qnamakerwin.azurewebsites.net/qnamaker", "4f080760-9f77-4f2e-9b8b-1945f6c16f1a", "f038dceb-289d-445f-87dc-81cefe57eed6");


        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }
        
        [LuisIntent(EatSushi)]
        public async Task EatSushiIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            await context.PostAsync(BitLockerQnAService.GetAnswer(result.Query));
        }

        [LuisIntent("StudyBiology")]
        public async Task StudyBiologyIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("In study biology intent!");
            await context.PostAsync(biologyQnAService.GetAnswer(result.Query));
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}