//Since the LiquidLib is planned to be merged into tModLoader, remove these in the future.
﻿using ModLiquidLib.ModLoader;
using Terraria;

namespace MurphysMod.Content.Liquids
{
    //An example of the ModLiquidFall class (although pretty empty here, a proper example will be made soon)
	public class EnchantedWaterFall : ModLiquidFall
	{
		public override bool PlayWaterfallSounds()
		{
			return true;
		}
        
		public override float? Alpha(int x, int y, float Alpha, int maxSteps, int s, Tile tileCache)
        {
            return .75f;
        }

		public override void AddLight(int i, int j)
		{
			Lighting.AddLight(i, j, .043f, .298f, .506f);
		}
	}
}