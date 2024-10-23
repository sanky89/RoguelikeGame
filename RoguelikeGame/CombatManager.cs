using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeGame
{
    public class CombatManager
    {
        private GameRoot _gameRoot;

        public CombatManager(GameRoot gameRoot)
        {
            _gameRoot = gameRoot;
        }

        public void ResolveCombat(Player player, Monster monster, out string log, bool playerAttacks = true)
        {
            int attackChance = _gameRoot.Rng.Next(100);
            int defenseChance = _gameRoot.Rng.Next(100);
            System.Console.WriteLine($"attackChance={attackChance} defenseChance={defenseChance}");
            int hitAmount = 0;
            log = "";
            var attackerStats = player.Stats;
            var defenderStats = monster.Stats;
            if(!playerAttacks)
            {
                attackerStats = monster.Stats;
                defenderStats = player.Stats;
            }
            if (attackChance <= attackerStats["attackChance"].CurrentValue)
            {

                hitAmount = attackerStats["attack"].CurrentValue;
                System.Console.WriteLine($"hitAmount inital= {hitAmount}");

                if(defenseChance <= defenderStats["defenseChance"].CurrentValue)
                {
                    hitAmount -= defenderStats["defense"].CurrentValue;
                }
                System.Console.WriteLine($"hitAmount= {hitAmount}");
                if (playerAttacks)
                {
                    if (hitAmount == 0)
                    {
                        log = $"{monster.Name} dodged your attack";

                    }
                    else if (hitAmount < 0)
                    {
                        log += $"{monster.Name} attacked you for {-hitAmount} damage";
                        player.Stats["health"].CurrentValue += hitAmount;
                    }
                    else
                    {
                        log = $"You hit {monster.Name} for {hitAmount} damage";
                        monster.Stats["health"].CurrentValue += -hitAmount;
                    }
                }
                else 
                {
                    if (hitAmount < 0)
                    {
                        log += $"{monster.Name} attacked you for {-hitAmount} damage";
                        player.Stats["health"].CurrentValue += hitAmount;
                    }
                }
            }
            else if(playerAttacks)
            {
                log = $"{monster.Name} dodged your attack";
            }
            else
            {
                log = $"You dodged {monster.Name}'s attack";
            }
            ResolveDeath(monster);
        }

        private void ResolveDeath(Monster m)
        {
            if(m.Stats["health"].CurrentValue <= 0)
            {
                _gameRoot.Map.RemoveMonster(m);
            }
        }
    }
}
