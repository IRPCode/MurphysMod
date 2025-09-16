using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience.Dusts;
using MurphysMod.Content.Ambience;
using System;
using MurphysMod.Content.Liquids;

using ModLiquidLib.ModLoader;
using ModLiquidLib.Utils.Structs;
using System.Linq.Expressions;
using MurphysMod.Content.Items.Placeables;
using MurphysMod.Content.Tiles;
using ModLiquidLib.Utils;


namespace MurphysMod //Add a check for the TorchGodLava to add custom particles
{

    public class BrazierFireHandler : ModPlayer
    {
        
        public override void PostUpdate()
        {
            if (Player.unlockedBiomeTorches)
            {
                for (int i = 0; i < 250; i++)
                {
                    if (Main.myPlayer == Player.whoAmI && !Main.dedServ) //local only
                    {
                        int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60); //adjust these values based off of velocity (xvel = 60 * player.velocity.x, negxvel = xvel * -1)
                        int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36); //also adjust this based off of screen size

                        int rand;

                        Tile tile = Framing.GetTileSafely(x, y);

                        Tile checkBrazier = Framing.GetTileSafely(x, y - 3);

                        if (tile.TileType == ModContent.TileType<Content.Tiles.TorchGodsBrazier>() && checkBrazier.TileType != ModContent.TileType<Content.Tiles.TorchGodsBrazier>())
                        {

                            rand = Main.rand.Next(0, 3);

                            if (rand == 0)
                            {
                                Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.InfernoFork, 0, 0, 100, default, 1f);
                            }
                            else if (rand == 1)
                            {
                                Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.IceTorch, 0, 0, 100, default, 1f);
                            }
                            else
                            {
                                Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.YellowTorch, 0, 0, 100, default, 1f);
                            }
                        }
                    }
                }
            }
        }
    }
}