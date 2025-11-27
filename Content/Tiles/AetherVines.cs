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

//Courtesy of steviegt6 from the CalamityMod Dev Team
{
    public class AetherVines : ModTile
    {
        private double timer;
        public override string Texture => "MurphysMod/Assets/Textures/Tiles/AetherVines";

        public override void SetStaticDefaults()
        {

            Main.tileLighted[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.IsVine[Type] = true;
            TileID.Sets.ReplaceTileBreakDown[Type] = true;
            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.DrawFlipMode[Type] = 1;
            TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

            TileID.Sets.ReplaceTileBreakUp[Type] = true;
            TileID.Sets.SwaysInWindBasic[Type] = true;

            DustType = DustID.Stone;
            HitSound = SoundID.Grass;

            AddMapEntry(new Color(209, 194, 255), CreateMapEntryName());

            base.SetStaticDefaults();
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Framing.GetTileSafely(i, j - 1);

            if (!tile.HasTile)
            {
                WorldGen.KillTile(i, j);
                return true;
            }
            return true;
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Main.tile[i, j + 1];

            bool vine = false;

            for (int vineYPos = j; vineYPos > j - 10; vineYPos--)
            {
                Tile origin = Main.tile[i, vineYPos];

                if (origin.BottomSlope)
                {
                    vine = false;
                    break;
                }

                if (Main.tile[i, vineYPos].HasTile && !Main.tile[i, vineYPos].BottomSlope && Main.tileSolid[Main.tile[i, vineYPos].TileType])
                {
                    vine = true;
                    break;
                }
            }

            if (vine)
            {
                int x = i;
                int y = j + 1;


                Tile vineXY = Framing.GetTileSafely(x, y);
                Main.tile[x, y].TileType = (ushort)ModContent.TileType<AetherVines>();
                Main.tile[x, y].TileFrameX = (short)(WorldGen.genRand.Next(8) * 18);
                Main.tile[x, y].TileFrameY = 4 * 18;
                vineXY.HasTile = true;

                Main.tile[i, j].TileFrameX = (short)(WorldGen.genRand.Next(12) * 18);
                Main.tile[i, j].TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);

                WorldGen.SquareTileFrame(x, y, true);
                WorldGen.SquareTileFrame(i, j, true);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, y, 3, TileChangeType.None);

            }
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