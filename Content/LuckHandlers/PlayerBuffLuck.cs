using MurphysMod.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class PlayerBuffLuck : ModPlayer
    {
        public override void PostUpdate()
        {
            getBuffLuck();
        }
        public double getBuffLuck()
        {
            Player player = Main.LocalPlayer;
            double BuffLuckVal = 0;

            if (player.usedGalaxyPearl)
            {
                BuffLuckVal -= .03;
            }

            if (player.HasBuff(BuffID.Lucky))
            {
                int luckBuffIndex = player.FindBuffIndex(BuffID.Lucky);
                int timeLeft = player.buffTime[luckBuffIndex];

                if (timeLeft >= 36000)
                    BuffLuckVal -= .3;
                else if (timeLeft >= 18000)
                    BuffLuckVal -= .2;
                else
                    BuffLuckVal -= .1;
            }
            
           /* if (Player.HasBuff<Sanctified>())
            {
                if (BuffLuckVal >= .25)
                    BuffLuckVal -= .25;
                else
                    BuffLuckVal = 0;
            }

            if (Player.HasBuff<Augury>())
                BuffLuckVal += .1;
            else if (Player.HasBuff<BadOmen>())
                BuffLuckVal += .2;
            else if (Player.HasBuff<Portent>())
                BuffLuckVal += .3;*/

            return BuffLuckVal;
        }
    }

}