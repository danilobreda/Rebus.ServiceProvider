using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using Sample.WebAppPipes.Session;

namespace Sample.WebAppPipes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public BookController(ILogger<BookController> logger, IBus bus, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _bus = bus;
            _serviceProvider = serviceProvider;
        }

        [HttpGet(Name = "save")]
        public IActionResult SaveBook([FromQuery]string title)
        {
            var session = _serviceProvider.GetRequiredService<ISessionData>();
            Console.WriteLine(session);//session.Id should be null here

            session.SetNewId();
            Console.WriteLine(session);//session.Id should have a Guid here
            
            _bus.Send(new MessageA()
            {
                Title = title
            });

            return Accepted();
        }
    }
}