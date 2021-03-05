using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DigitalHowdy.Server.Messaging
{
    public class TwilioService : ISmsService
    {
        private readonly PhoneNumber _sender; 
        private readonly ILogger<TwilioService> _logger;

        public TwilioService(IConfiguration configuration, ILogger<TwilioService> logger)
        {
            var accountSID = configuration["Twilio:AccountSID"];
            var authToken = configuration["Twilio:AuthToken"];
            var phoneString = configuration["Twilio:FromPhoneNumber"];

            _sender = new PhoneNumber(phoneString);

            TwilioClient.Init(accountSID, authToken);

            _logger = logger;
        }
        public void SendSMS(SmsMessage message)
        {
            try
            {
                var recipient = new PhoneNumber(message.Recipient);
                var messageBody = message.Body;

                var twilioMessage = MessageResource.Create(
                    body: messageBody,
                    from: _sender,
                    to: recipient
                );
            }
            catch (ApiException e)
            {
                _logger.LogError($"An error occured while processing the Twilio request. SMS was not sent. Error: {e.Message}");
            }
        }
    }
}