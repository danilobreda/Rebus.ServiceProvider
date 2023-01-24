using Rebus.Bus;
using Rebus.Handlers;
using Sample.WebAppPipes.Session;

namespace Sample.WebAppPipes.Handlers
{
    public class HandlerB : IHandleMessages<MessageB>
    {
        private readonly ILogger _logger;
        private readonly ISessionData _sessionData;
        private readonly IBus _bus;

        public HandlerB(ILogger<HandlerB> logger, ISessionData sessionData, IBus bus)
        {
            _logger = logger;
            _sessionData = sessionData;
            _bus = bus;
        }

        public Task Handle(MessageB message)
        {
            _logger.LogInformation("Handlerb received : {message}", message);

            Console.WriteLine(_sessionData);

            return Task.CompletedTask;
        }
    }
}
