using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Pipeline;
using Sample.WebAppPipes.Session;

namespace Sample.WebAppPipes.Pipes
{
    public class CustomFlowIncomingStep : IIncomingStep
    {
        public async Task Process(IncomingStepContext context, Func<Task> next)
        {

            try
            {
                var message = context.Load<Message>();

                var serviceProvider = context.Load<IServiceProvider>();
                using (var scope = serviceProvider.CreateScope())
                {
                    //setting sessiondata into IsessionData
                    var sess = scope.ServiceProvider.GetRequiredService<ISessionData>();
                    var sessionDataGuid = Parse(message.Headers.GetValueOrNull("sessiondataid"));
                    sess.SetId(sessionDataGuid);

                    context.Save(scope);
                    await next();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Guid? Parse(string s)
        {
            if (Guid.TryParse(s, out var guid))
                return guid;
            return null;
        }
    }
}
