using Rebus.Bus;
using Rebus.Handlers;
using Sample.WebAppPipes.Session;

namespace Sample.WebAppPipes.Handlers
{
    public class HandlerA : IHandleMessages<MessageA>
    {
        private readonly ILogger _logger;
        private readonly ISessionData _sessionData;
        private readonly IBus _bus;

        public HandlerA(ILogger<HandlerA> logger, ISessionData sessionData, IBus bus)
        {
            _logger = logger;
            _sessionData = sessionData;
            _bus = bus;
        }

        public Task Handle(MessageA message)
        {
            _logger.LogInformation("HandlerA received : {message}", message);

            Console.WriteLine(_sessionData);

            _bus.Send(new MessageB()
            {
                ProcessedTitle = message.Title.Trim().ToUpper()
            });

            return Task.CompletedTask;
        }
    }
}
