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
            int attackChance = Globals.Rng.Next(100);
            int defenseChance = Globals.Rng.Next(100);
            System.Console.WriteLine($"attackChance={attackChance} defenseChance={defenseChance}");
            int hitAmount = 0;
            log = "";
            var playerStats = player.PlayerStats.Stats;
            var monsterStats = monster.MonsterStats.Stats;
            if (attackChance <= playerStats["attackChance"].CurrentValue)
            {

                hitAmount = playerStats["attack"].CurrentValue;
                System.Console.WriteLine($"hitAmount inital= {hitAmount}");
                if(defenseChance <= monsterStats["defenseChance"].CurrentValue)
                {
                    hitAmount -= monsterStats["defense"].CurrentValue;
                }
                System.Console.WriteLine($"hitAmount= {hitAmount}");
                if(hitAmount == 0)
                {
                    log = $"{monster.Name} dodged your attack";

                }
                else if(hitAmount < 0)
                {
                    log += $"{monster.Name} attacked you for {-hitAmount} damage";
                    player.PlayerStats.UpdateStat("health", hitAmount);
                }
                else
                {
                    log = $"You hit {monster.Name} for {hitAmount} damage";
                    monster.MonsterStats.UpdateStat("health", -hitAmount);
                }
            }
            else
            {
                log = $"{monster.Name} dodged your attack";
            }
            ResolveDeath(monster);
        }

        private void ResolveDeath(Monster m)
        {
            if(m.MonsterStats.Stats["health"].CurrentValue <= 0)
            {
                Globals.Map.RemoveMonster(m);
            }
        }
    }
}
