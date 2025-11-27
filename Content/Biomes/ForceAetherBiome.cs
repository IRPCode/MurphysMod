using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Tiles;
using System;
using MurphysMod.Content.Biomes;

namespace MurphysMod.Content.Biomes
{
    public class ShimmerPlayer : ModPlayer
{
    public static bool biomeActive;

    public override void PostUpdate()
    {
        if(AetherCount.Amount >= 10)
            Player.ZoneShimmer = true;
        else
        Player.ZoneShimmer = false;
    }

}
public class AetherCount : ModSystem
	{
		public static int Amount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			Amount = tileCounts[ModContent.TileType<AetherGrass>()];
		}
	}

}


