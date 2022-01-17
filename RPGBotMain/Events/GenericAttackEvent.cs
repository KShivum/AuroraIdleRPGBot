using System;
using RPGBotMain.Models;

namespace RPGBotMain.Events
{
    public class GenericAttackEvent : Event
    {
        public int AttackerPower { get; set; }
        public int AttackerDefense { get; set; }
        public int AttackerSpeed { get; set; }
        public string AttackerName { get; set; }



        public string[] WinFlavorText = {
            "You beat the @# with a strong slash through the head",
            "You beat the @# with a mighty swing"
        };

        public GenericAttackEvent(string attackerName, int attackerPower, int attackerDefence, int attackerSpeed)
        {
            Type = "AttackEvent";
            AttackerName = attackerName;
            AttackerPower = attackerPower;
            AttackerDefense = attackerDefence;
            AttackerSpeed = attackerSpeed;
        }

        public bool TryBattle(User defender)
        {
            Random rng = new Random();
            while (true)
            {
                if (defender.Speed > AttackerSpeed)
                {
                    int attack = (int)rng.NextInt64(0, defender.Attack + AttackerDefense);
                    if(attack < defender.Attack)
                    {
                        return true;
                    }
                    else
                    {
                        attack = (int)rng.NextInt64(0, AttackerPower + defender.Defense);
                        if(attack < AttackerPower)
                        {
                            return false;
                        }
                        
                    }
                }
                else
                {
                    int attack = (int)rng.NextInt64(0, AttackerPower + defender.Defense);
                    if (attack < AttackerPower)
                    {
                        return false;
                    }
                    else
                    {
                        attack = (int)rng.NextInt64(0, defender.Attack + AttackerDefense);
                        if (attack < defender.Attack)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        public bool TryEscape()
        {
            //Maybe will add a chance to fail
            return true;
        }

        public string GetWinFlavorText()
        {
            Random rng = new Random();
            string text = WinFlavorText[(int)rng.NextInt64(0, WinFlavorText.Length)];
            text = text.Replace("@#", AttackerName);
            return text;
        }



    }
}
