using System;
using System.Collections;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class PlayerStats
    {
        public string StatsLog { get; private set; }
        private Dictionary<string, Stat> _stats;

        public PlayerStats()
        {
            _stats = new Dictionary<string, Stat>
            {
                {
                    "Health",
                    new Stat
                    {
                        Name = "Health",
                        MinValue = 0,
                        MaxValue = 100,
                        CurrentValue = 100
                    }
                },
                {
                    "Attack",
                    new Stat
                    {
                        Name = "Attack",
                        MinValue = 1,
                        MaxValue = 20,
                        CurrentValue = 10
                    }
                },
                {
                    "Defense",
                    new Stat
                    {
                        Name = "Defense",
                        MinValue = 1,
                        MaxValue = 20,
                        CurrentValue = 10
                    }
                }
            };

            foreach(var kvp in _stats)
            {
                StatsLog += $"{kvp.Key}: {kvp.Value.CurrentValue}/{kvp.Value.MaxValue}\n";
            }
        }

        public void UpdateStat(string statName, int amount)
        {
            _stats[statName].CurrentValue += amount;
            StatsLog = "";
            foreach(var kvp in _stats)
            {
                StatsLog += $"{kvp.Key}: {kvp.Value.CurrentValue}/{kvp.Value.MaxValue}\n";
            }
        }

    }
}