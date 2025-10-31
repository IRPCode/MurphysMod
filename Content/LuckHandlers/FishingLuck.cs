using Terraria;
using Terraria.ModLoader;

namespace MurphysMod.Content.LuckHandlers
{
    public class FishingLuck : ModPlayer
    {
        public override void GetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel)
        {

            float luckVal = Player.GetModPlayer<LuckHandler>().luckValue();

            if(luckVal <= .2f)
            {
                fishingLevel *= (1.1f - luckVal);
            }
            else
            {
                fishingLevel *= Utils.Clamp(1f - luckVal, 0f, 1f);
            }
        }
    }
}