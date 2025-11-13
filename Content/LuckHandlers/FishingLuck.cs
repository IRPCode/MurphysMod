using Terraria;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class FishingLuck : ModPlayer
    {
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {

            double luckVal = Player.GetModPlayer<LuckHandler>().luckValue();

            if(luckVal <= .2)
            {
                fishingLevel *= (float)(1.1 - luckVal);
            }
            else
            {
                fishingLevel *= (float)Utils.Clamp(1 - luckVal, 0f, 1f);
            }
        }
    }
}