using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class CombatManager
    {

        public void ResolveCombat(Player player, Monster monster, out string log)
        {
            int chance = Globals.Rng.Next(100);
            int hitAmount = 0;
            log = "";
            if (chance <= player.PlayerStats.Stats["attackChance"].CurrentValue)
            {
                hitAmount = player.PlayerStats.Stats["attack"].CurrentValue;
                log = $"You hit {monster.Name} for {hitAmount} damage";
            }
            else
            {
                log = $"{monster.Name} dodged your attack";
            }
            monster.MonsterStats.UpdateStat("health", -hitAmount);
        }
    }
}
