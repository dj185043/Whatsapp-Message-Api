using SendMessage.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SendMessage.Controllers
{
    public class InventoryController : ApiController
    {
        [Route("Inventory/UpdateInventory")]
        public IHttpActionResult Post([FromBody] Inventory inventory)
        {
            string ConnectionString = "Connection-String";
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand($"Update [dbo].[Inventory] SET Quantity = {inventory.Quantity} Where SKU ='{inventory.SKU}'", sqlConnection);
            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();
            
            return Ok();
        }
    }
}
