using Newtonsoft.Json;
using SendMessage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SendMessage.Controllers
{
    public class TransactionController : ApiController
    {
        [Route("Transaction/AddTransaction")]
        public IHttpActionResult Post([FromBody] Transaction transaction)
        {
            string ConnectionString = "Connection-String";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand($"Insert into [dbo].[Transactions] (TransactionID, TransactionDateTime, SKU, CustomerID, NetPrice, Quantity, State) Values('{transaction.TransactionID}', '{transaction.TransactionDateTime}', '{transaction.SKU}', '{transaction.CustomerID}', {transaction.NetPrice}, {transaction.Quantity}, '{transaction.State}')", sqlConnection);
            sqlCommand.ExecuteNonQuery();

            sqlCommand = new SqlCommand($"Update [dbo].[Inventory] SET Quantity = (Quantity - {transaction.Quantity}) Where SKU ='{transaction.SKU}'", sqlConnection);
            sqlCommand.ExecuteNonQuery();

            sqlCommand = new SqlCommand($"Select Quantity from [dbo].[Inventory] Where SKU='{transaction.SKU}'", sqlConnection);
            int Quantity = (int)sqlCommand.ExecuteScalar();

            sqlConnection.Close();

            int predictionQuantity = 0;
            string productname = string.Empty;
            int Weeks = 0;
            List<Prediction> predictions = GetPredictions(transaction.SKU);
            foreach(var prediction in predictions)
            {
                productname = prediction.ProductName;
                predictionQuantity += prediction.Quantity;
                Weeks = prediction.Period;
            }

            if(predictionQuantity> Quantity)
            {
                string Message = $"I am NCR Smart Predictor. I predict that '{productname}' is going to run Out of Stock by next Week. Please consider ordering {predictionQuantity} within next {Weeks} week(s).";
                SendMessageModels.SendWhatsAppMessage("Provide-Number", Message);
            }

            return Ok();
        }

        [Route("Transaction/GetPredictions")]
        public List<Prediction> GetPredictions(string SKU)
        {

            HttpClient ApiClient = new HttpClient();
            ApiClient.Timeout = new TimeSpan(0, 0, 0, 30);          
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string ApiURL = $"https://inventory-sales-prediction.herokuapp.com/predict?SKU={SKU}";

            StringContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = ApiClient.GetAsync(ApiURL).GetAwaiter().GetResult())
            {
                if (response.IsSuccessStatusCode)
                {
                    var res = response.Content.ReadAsStringAsync().Result;
                    return (List<Prediction>)JsonConvert.DeserializeObject(res, typeof(List<Prediction>));
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
