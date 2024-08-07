using System;

namespace RoguelikeGame
{
    public class Stat
    {
        public string Name { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int DefaultValue { get; set; }
        public int CurrentValue { get; set; }
        public bool Display { get; set; }

        public bool ShouldDisplayStat()
        {
            return Display && CurrentValue > 0;
        }
    }
}