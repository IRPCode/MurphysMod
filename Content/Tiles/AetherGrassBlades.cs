using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Tiles

{
	public class AetherGrassBlades : ModTile
	{
        private double timer;
		public override string Texture => "MurphysMod/Assets/Textures/Tiles/AetherGrassBlades";

		public override void SetStaticDefaults() {

			Main.tileCut[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileWaterDeath[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;

            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            DustType = DustID.Stone;
            HitSound = SoundID.Grass;

            AddMapEntry(new Color(209, 194, 255), CreateMapEntryName());

			base.SetStaticDefaults();
		}

        public override void PostDraw(int x, int y, SpriteBatch spriteBatch)
        {
            Lighting.AddLight(new Vector2(x + .5f, y + .5f) * 16f, colorOsciliator().ToVector3());
        }

        public Color colorOsciliator()
        {
            timer++;
            double lerpAmount = Math.Sin(timer * (Math.PI / (180 * 150)));
            return Color.Lerp(new Color(209, 194, 255), new Color(135, 130, 153), (float)lerpAmount);
        }
	}	
}