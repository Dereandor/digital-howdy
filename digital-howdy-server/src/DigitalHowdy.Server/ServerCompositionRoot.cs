using DigitalHowdy.Server.Authentication;
using DigitalHowdy.Server.Authorization;
using LightInject;
using Microsoft.AspNetCore.Identity;

namespace DigitalHowdy.Server
{
    public class ServerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterSingleton<ITokenManager, JwtTokenManager>()
                .RegisterSingleton<IPasswordPolicy, PasswordPolicy>()
                .RegisterSingleton<IPasswordManager, PasswordManager>();
        }
    }
}