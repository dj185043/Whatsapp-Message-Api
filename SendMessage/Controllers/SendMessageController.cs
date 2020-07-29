using SendMessage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SendMessage.Controllers
{
    public class SendMessageController : ApiController
    {
        [Route("SendMessage/WhatsApp")]
        public IHttpActionResult Post(string PhoneNumber, string Message)
        {
            if (!string.IsNullOrEmpty(SendMessageModels.SendWhatsAppMessage(PhoneNumber, Message)))
                return Ok();
            else
                return InternalServerError();
        }
    }
}
