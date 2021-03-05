namespace DigitalHowdy.Server.Messaging
{
    public struct SmsMessage
    {
        public string Recipient { get; }
        public string Body { get; }
        
        public SmsMessage(string recipient, string body)
        {
            Recipient = recipient;
            Body = body;
        }
    }
}