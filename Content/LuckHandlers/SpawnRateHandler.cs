using Terraria;
using Terraria.ModLoader;


namespace MurphysMod.Content.LuckHandlers
{
    public class spawnRateHandler : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            double luckVal = player.GetModPlayer<LuckHandler>().luckValue();
            spawnRate *= (int)(1 + (luckVal * 2));
            maxSpawns *= (int)(1 + (luckVal * 2));
        }
    }
}
