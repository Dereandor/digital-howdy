namespace DigitalHowdy.Server.Messaging
{
    public interface ISmsService
    {
        void SendSMS(SmsMessage message);
    }
}