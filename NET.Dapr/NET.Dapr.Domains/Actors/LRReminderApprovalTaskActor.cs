using Dapr.Actors;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;

namespace NET.Dapr.Domains.Actors
{
    public interface ILRReminderApprovalTaskActor:IActor
    {
        Task RegisterReminderTaskWithTimeToLive(TimeSpan dueTime, TimeSpan period, TimeSpan timeToLive);
        Task RegisterReminderTask(TimeSpan dueTime, TimeSpan period);
        Task UnregisterReminderTask();
    }
    public class LRReminderApprovalTaskActor(ActorHost host) : Actor(host), ILRReminderApprovalTaskActor, IRemindable
    {
        const string reminderName = "ReviewApprovalTaskReminder";
        protected override Task OnActivateAsync()
        {
            // Provides opportunity to perform some optional setup.
            Logger.LogDebug($"Activating actor id: {this.Id}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called whenever an actor is deactivated after a period of inactivity.
        /// </summary>
        protected override Task OnDeactivateAsync()
        {
            // Provides Opporunity to perform optional cleanup.
            Logger.LogDebug($"Deactivating actor id: {this.Id}");
            return Task.CompletedTask;
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            Logger.LogInformation($"Reminder {reminderName} for actor id {Id} is triggered.");
            return Task.CompletedTask;
        }

        public async Task RegisterReminderTaskWithTimeToLive(TimeSpan dueTime,TimeSpan period, TimeSpan timeToLive)
        {
            Logger.LogInformation($"RegisterReminderTaskWithTimeToLive {reminderName} for actor id {Id} registered.");
            await RegisterReminderAsync(reminderName, null, dueTime, period, timeToLive);

        }
        public async Task RegisterReminderTask(TimeSpan dueTime, TimeSpan period)
        {
            Logger.LogInformation($"RegisterReminderTask {reminderName} for actor id {Id} registered.");
            await RegisterReminderAsync(reminderName, null, dueTime, period);
        }

        public async Task UnregisterReminderTask()
        {
            await UnregisterReminderAsync(reminderName);
        }
    }
}
