using Dapr.Client;
using Dapr.Workflow;
using Google.Api;
using NET.Dapr;
using NET.Dapr.Domains;
using NET.Dapr.Domains.Actors;
using NET.Dapr.Domains.Workflows.LeaveRequest;
using NET.Dapr.Domains.Workflows.LeaveRequest.Activities;
using NET.Dapr.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastructure();
builder.Services.InjectService();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
builder.Services.AddDaprWorkflow(options =>
{
    // Note that it's also possible to register a lambda function as the workflow
    // or activity implementation instead of a class.
    options.RegisterWorkflow<LeaveRequestWorkflow>();

    // These are the activities that get invoked by the workflow(s).
    options.RegisterActivity<LR_GetApproverAndCreateTaskActivity>();
    options.RegisterActivity<LR_AfterManagerApprovalActivity>();
    options.RegisterActivity<LR_ApprovalTimeoutActivity>();
    options.RegisterActivity<LR_SendEmailNotifyActivity>();
    options.RegisterActivity<LR_ReminderApprovalTaskActivity>();
});
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<LRReminderApprovalTaskActor>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.MapControllers();
app.UseCors("MyPolicy");
app.Run();
