using System;
using MurphysMod.Content.Liquids;
using MurphysMod.Content.Tiles;
using Terraria.ModLoader;

namespace MurphysMod.Common.Biomes
{
	public class ForgeTileCounter : ModSystem
	{
		public int forgeBlockCount;

		public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
			forgeBlockCount = tileCounts[ModContent.TileType<TorchGodsBrazier>()];
		}
	}
}