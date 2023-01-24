using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;
using Rebus.Pipeline.Send;
using Rebus.Routing.TypeBased;
using Rebus.Transport.InMem;
using Sample.WebAppPipes.Handlers;
using Sample.WebAppPipes.Pipes;
using Sample.WebAppPipes.Session;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISessionData, SessionData>();//scoped data

builder.Services.AddHttpContextAccessor();//.net5+ to access IHttpContextAccessor

// Register handlers 
builder.Services.AutoRegisterHandlersFromAssemblyOf<HandlerA>();//HandlerA and HandlerB

builder.Services.AddRebus((configurer, provider) =>
{
    var config = configurer.Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "rebusexample"))
        .Options(o =>
        {
            o.Decorate<IPipeline>(c =>
            {
                var incomingStep = new CustomFlowIncomingStep();
                var outgoingStep = new CustomFlowOutgoingStep();

                var pipeline = c.Get<IPipeline>();

                pipeline = new PipelineStepInjector(pipeline)
                    .OnReceive(incomingStep, PipelineRelativePosition.After, typeof(DeserializeIncomingMessageStep))
                    .OnSend(outgoingStep, PipelineRelativePosition.Before, typeof(SerializeOutgoingMessageStep));

                return pipeline;
            });
        })
        .Logging(l => l.Trace())
        .Routing(r =>
        {
            r.TypeBased().MapAssemblyOf<HandlerA>("rebusexample");//HandlerA and HandlerB
        })
        .Sagas(s => s.StoreInMemory());

    return config;
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();