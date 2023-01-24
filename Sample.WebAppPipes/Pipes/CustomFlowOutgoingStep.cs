using Rebus.Messages;
using Rebus.Pipeline;
using Sample.WebAppPipes.Session;

namespace Sample.WebAppPipes.Pipes
{
    public class CustomFlowOutgoingStep : IOutgoingStep
    {
        public async Task Process(OutgoingStepContext context, Func<Task> next)
        {
            try
            {
                var contextMessage = context.Load<Message>();

                var serviceProvider = context.Load<IServiceProvider>();
                var requestScoped = serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.RequestServices;

                //setting sessiondata
                if (requestScoped != null)//from http (api)
                {
                    
                    var sessionData = requestScoped.GetService<ISessionData>();
                    contextMessage.Headers.Add("sessiondataid", sessionData?.Id.ToString());

                    await next();
                }
                else//from other handler
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var sessionData = scope.ServiceProvider.GetService<ISessionData>();
                        contextMessage.Headers.Add("sessiondataid", sessionData?.Id.ToString());

                        await next();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
