﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace LuisBot.Models
{
    /// <summary>
    /// QnAMakerService is a wrapper over the QnA Maker REST endpoint
    /// </summary>
    [Serializable]
    public class QnAMakerService
    {
        private string qnaServiceHostName;
        private string knowledgeBaseId;
        private string endpointKey;

        /// <summary>
        /// Initialize a particular endpoint with it's details
        /// </summary>
        /// <param name="hostName">Hostname of the endpoint</param>
        /// <param name="kbId">Knowledge base ID</param>
        /// <param name="ek">Endpoint Key</param>
        public QnAMakerService(string hostName, string kbId, string ek)
        {
            qnaServiceHostName = hostName;
            knowledgeBaseId = kbId;
            endpointKey = ek;
        }

        /// <summary>
        /// Call the QnA Maker endpoint and get a response
        /// </summary>
        /// <param name="query">User question</param>
        /// <returns></returns>
        public string GetAnswer(string query)
        {
            var client = new RestClient(qnaServiceHostName + "/knowledgebases/" + knowledgeBaseId + "/generateAnswer");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", "EndpointKey " + endpointKey);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"question\": \"" + query + "\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            // Deserialize the response JSON
            QnAAnswer answer = JsonConvert.DeserializeObject<QnAAnswer>(response.Content);

            // Return the answer if present
            if (answer.answers.Count > 0)
                return answer.answers[0].answer;
            else
                return "No good match found.";
        }
    }

    /* START - QnA Maker Response Class */
    public class Metadata
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Answer
    {
        public IList<string> questions { get; set; }
        public string answer { get; set; }
        public double score { get; set; }
        public int id { get; set; }
        public string source { get; set; }
        public IList<object> keywords { get; set; }
        public IList<Metadata> metadata { get; set; }
    }

    public class QnAAnswer
    {
        public IList<Answer> answers { get; set; }
    }
    /* END - QnA Maker Response Class */
}