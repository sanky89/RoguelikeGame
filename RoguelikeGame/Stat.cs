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
    }
}