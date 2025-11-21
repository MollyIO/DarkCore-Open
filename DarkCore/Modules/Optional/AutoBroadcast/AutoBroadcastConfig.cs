using System.Collections.Generic;

namespace DarkCore.Modules.Optional.AutoBroadcast
{
    public class AutoBroadcastConfig
    {
        public List<BroadcastMessage> WelcomeMessages { get; set; } = new List<BroadcastMessage>
        {
            new BroadcastMessage { Text = "Welcome to the server! Enjoy your stay!", Time = 15 },
            new BroadcastMessage { Text = "Remember to follow the server rules and have fun!", Time = 15 }
        };
        
        public List<BroadcastMessage> EndRoundBroadcast { get; set; } = new List<BroadcastMessage>
        {
            new BroadcastMessage { Text = "Thanks for playing! See you next round!", Time = 10 },
            new BroadcastMessage { Text = "Don't forget to check out our Discord server!", Time = 10 }
        };
        
        public Dictionary<int, List<BroadcastMessage>> ScheduledMessages { get; set; } = new Dictionary<int, List<BroadcastMessage>>
        {
            { 60, new List<BroadcastMessage> { new BroadcastMessage { Text = "Welcome to our server! Have fun!", Time = 10 } } },
            { 120, new List<BroadcastMessage> { new BroadcastMessage { Text = "Remember to read the rules in the #rules channel!", Time = 10 } } },
        };
        
        public Dictionary<int, List<BroadcastMessage>> PeriodicMessages { get; set; } = new Dictionary<int, List<BroadcastMessage>>
        {
            { 600, new List<BroadcastMessage> { new BroadcastMessage { Text = "Remember to take breaks and stay hydrated!", Time = 10 } } },
            { 900, new List<BroadcastMessage> { new BroadcastMessage { Text = "Check out our website for the latest news and updates!", Time = 10 } } },
        };
        
        public class BroadcastMessage
        {
            public string Text { get; set; }
            public int Time { get; set; }
        }
    }
}