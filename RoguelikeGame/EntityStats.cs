using System;
using System.Collections;
using System.Collections.Generic;

namespace RoguelikeGame
{
    public class EntityStats
    {
        public string StatsLog { get; private set; }
        public readonly Dictionary<string, Stat> Stats;

        public EntityStats(List<StatDataModel> statsData)
        {
            Stats = new Dictionary<string, Stat>();
            foreach(var s in statsData)
            {
                var stat = new Stat
                {
                    Name = s.name,
                    MinValue = s.minValue,
                    MaxValue = s.maxValue,
                    CurrentValue = s.defaultValue
                };
                Stats.Add(s.name, stat);
                StatsLog += $"{stat.Name}: {stat.CurrentValue}/{stat.MaxValue}\n";
            }
        }

        public void UpdateStat(string statName, int amount)
        {
            Stats[statName].CurrentValue += amount;
            StatsLog = "";
            foreach(var kvp in Stats)
            {
                var stat = kvp.Value;
                StatsLog += $"{stat.Name}: {stat.CurrentValue}/{stat.MaxValue}\n";
            }
        }

    }
}