using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SendMessage.Models
{
    public class SendMessageModels
    {
        public static string SendWhatsAppMessage(string PhoneNumber, string Message)
        {
            const string accountSid = "AC085dc98c3c9088e4455917a1e96aa506";
            const string authToken = "9b3e051ea2d99a8ca2dcf53d401108c8";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                body: $"{Message}",
                to: new Twilio.Types.PhoneNumber($"whatsapp:{PhoneNumber}")
            );

            return message.Sid;
        }
    }
}