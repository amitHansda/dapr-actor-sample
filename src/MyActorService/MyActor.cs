using System;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using MyActor.Interfaces;

namespace MyActorService
{
    internal class MyActor : Actor, IMyActor, IRemindable
    {
        public MyActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            Console.WriteLine($"Activating actor id:{this.Id}");
            return Task.CompletedTask;
        }

        protected override Task OnDeactivateAsync()
        {
            Console.WriteLine($"Deactivating actor id:{this.Id}");
            return Task.CompletedTask;
        }

        public Task<MyData> GetDataAsync()
        {
            return this.StateManager.GetStateAsync<MyData>("my_data");
        }
        /// <summary>
        /// Set MyData into actor's private state store
        /// </summary>
        /// <param name="data">the user-defined MyData which will be stored into state store as "my_data" state</param>        
        public async Task<string> SetDataAsync(MyData data)
        {
            // Data is saved to configured state store implicitly 
            // after each method execution by Actor's runtime.
            // Data can also be saved explicitly by calling this.StateManager.SaveStateAsync();
            // State to be saved must be DataContract serialziable.
            await this.StateManager.SetStateAsync<MyData>("my_data", data);
            return "Success";
        }

        ///<summary>
        /// Register MyReminder reminder with the actor
        ///</summary>
        public async Task RegisterReminder()
        {
            await this.RegisterReminderAsync("MyReminder",
            null,
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(5));
            throw new System.NotImplementedException();
        }

        public Task UnregisterReminder()
        {
            Console.WriteLine("Unregistering MyReminder");
            return this.UnregisterReminderAsync("MyReminder");
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            Console.WriteLine("ReceiveReminderAsync is called!");
            return Task.CompletedTask;
        }


        public Task RegisterTimer()
        {
            return this.RegisterTimerAsync(
                "MyTimer",                  // The name of the timer
                this.OnTimerCallBack,       // Timer callback
                null,                       // User state passed to OnTimerCallback()
                TimeSpan.FromSeconds(5),    // Time to delay before the async callback is first invoked
                TimeSpan.FromSeconds(5));   // Time interval between invocations of the async callback
        }
        private Task OnTimerCallBack(object data)
        {
            Console.WriteLine("OnTimerCallBack is called!");
            return Task.CompletedTask;
        }





        public Task UnregisterTimer()
        {
            Console.WriteLine("Unregistering MyTimer...");
            return this.UnregisterTimerAsync("MyTimer");
        }
    }
}