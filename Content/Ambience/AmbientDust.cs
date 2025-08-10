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


namespace MurphysMod //Add a check for the TorchGodLava to add custom particles
{

    public class AmbientDust : ModPlayer
    {
        public static int dustAmountMultiplier = 1; //fix this statement, and then fix the shimmer sparkles (encase spawning particles in loop)
                                                    //TODO: set the dust type with a dust variable, and then a variable at the end if dust != null, then spawn the particle. If it doesn't equal null and bool loop == true, then set it to another if statement
                                                    //that uses a for loop to spawn more particles, as set by the if statement that sets the particle type 

        public override void PostUpdate()
        {
            if (Main.myPlayer == Player.whoAmI && !Main.dedServ) //local only
            {
                int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60); //adjust these values based off of velocity (xvel = 60 * player.velocity.x, negxvel = xvel * -1)
                int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36); //also adjust this based off of screen size

                int rand;

                Tile tile = Framing.GetTileSafely(x, y);

                if ((tile.TileType == TileID.Grass || tile.TileType == TileID.Mud || tile.TileType == TileID.HallowedGrass) && !Main.dayTime) //fireflies
                {
                    for (int i = 0; i < 5; i++)
                    {
                        x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                        y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);
                        if (Player.ZoneForest || Player.ZoneJungle || Player.ZoneHallow)
                        {
                            if (Player.ZoneHallow)
                            {
                                rand = Main.rand.Next(0, 6 / dustAmountMultiplier);
                            }
                            else
                            {
                                rand = Main.rand.Next(0, 2);
                            }

                            int randomFlyLocation = Main.rand.Next(10, 26);
                            Tile airCheck = Framing.GetTileSafely(x, y - randomFlyLocation);

                            if (rand == 0 && (!airCheck.HasTile && tile.WallType == WallID.None) && !Player.ZoneHallow)
                            {
                                Dust.NewDust(new Vector2((x * 16), (y - randomFlyLocation) * 16), 16, 16, DustID.Firefly, 0, 0, 100, default, 1f); //keep fireflydust for now
                            }
                            else if (rand == 0 && (!airCheck.HasTile && tile.WallType == WallID.None))
                            {
                                int color = Main.rand.Next(0, 2);

                                var dustType = ModContent.DustType<PinkHallowFireflyDust>();

                                if (color % 2 == 0)
                                {
                                    dustType = ModContent.DustType<BlueHallowFireflyDust>();
                                }
                                Dust.NewDust(new Vector2((x * 16), (y - randomFlyLocation) * 16), 16, 16, dustType, 0, 0, 100, default, 1f);
                            }
                        }
                    }
                }

                if (!tile.HasTile)
                {
                    if (tile.WallType == 3 || (Player.ZoneCorrupt && Player.position.Y / 16 >= Main.rockLayer)) //corruption
                    {
                        rand = Main.rand.Next(0, 15 / dustAmountMultiplier);

                        if (rand <= 7)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.Corruption, 0, 0, 100, default, 1f);
                        }

                        else if (rand <= 8)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }

                        else if (rand <= 9)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.Demonite, 0, 0, 100, default, 1f);
                        }
                    }

                    if (tile.WallType == 83 || (Player.ZoneCrimson && Player.position.Y / 16 >= Main.rockLayer)) //crimson
                    {
                        rand = Main.rand.Next(0, 15 / dustAmountMultiplier);

                        if (rand <= 7)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.Crimson, 0, 0, 100, default, 1f);

                        }

                        else if (rand <= 8)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }

                        else if (rand <= 9)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.Crimstone, 0, 0, 100, default, 1f);
                        }
                    }

                    if (((tile.WallType > 99 && tile.WallType < 104) || tile.WallType == 28) || (Player.ZoneHallow && Player.position.Y / 16 >= Main.rockLayer)) //Hallow
                    {
                        rand = Main.rand.Next(0, 40 / dustAmountMultiplier);

                        if (rand <= 10)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }

                        else if (rand == 11)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.PurpleCrystalShard, 0, 0, 100, default, 1f);
                        }

                        else if (rand == 12)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.PinkCrystalShard, 0, 0, 100, default, 1f);
                        }
                        else if (rand == 13)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.BlueCrystalShard, 0, 0, 100, default, 1f);
                        }
                    }

                    if (Player.ZoneDungeon && ((tile.WallType >= 7 && tile.WallType <= 9) || (tile.WallType >= 94 && tile.WallType <= 99))) //dungeon
                    {
                        rand = Main.rand.Next(0, 40 / dustAmountMultiplier);

                        if (rand <= 15)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }
                        else if (rand == 16)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<DungeonWaterDust>(), 0, 0, 100, default, 1f);
                        }
                    }

                    if (Player.ZoneNormalUnderground || Player.ZoneNormalCaverns) //caves
                    {
                        rand = Main.rand.Next(0, 40 / dustAmountMultiplier);
                        if (rand <= 15)
                        {
                            int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                            Dust dust = Main.dust[dustLocation];
                        }
                    }

                    if (Player.ZoneShimmer)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                            y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);
                            tile = Framing.GetTileSafely(x, y);
                            Tile shimmerTileAirCheck = Framing.GetTileSafely(x, y - 1); //checks for air

                            if (tile.LiquidAmount >= 1 && tile.LiquidType == LiquidID.Shimmer && shimmerTileAirCheck.LiquidAmount == 0) //shimmer liquid
                            {
                                Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.ShimmerSpark, 0, 0, 100, default, 1f);
                            }
                        }

                    }


                    for (int i = 0; i < 3; i++)
                    {
                        x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                        y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);
                        tile = Framing.GetTileSafely(x, y);

                        if (tile.LiquidAmount >= 1 && tile.LiquidType == LiquidID.Lava) //lava embers
                        {
                            rand = Main.rand.Next(0, 5 / dustAmountMultiplier);
                            Dust dust;

                            if (rand == 0 && Player.ZoneUnderworldHeight) //fewer particles for underworld 
                            {
                                int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<EmberDust>(), 0, 0, 100, default, 1f);
                                dust = Main.dust[dustLocation];
                                dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);
                            }
                            else if (rand <= 4)
                            {
                                int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<EmberDust>(), 0, 0, 100, default, 1f);
                                dust = Main.dust[dustLocation];
                                dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);
                            }
                            /*else if (rand == 5)
                            {
                                int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                                dust = Main.dust[dustLocation];
                                dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);

                            }*/
                        }

                        //int forgeLavaType = ModContent.Find<ModLiquid>("MurphysMod", "GodlyForgeLava").Type;
                        try {
                            if (tile.LiquidAmount >= 1 && tile.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "OrdainedMoltenSlag").Type)
                            {
                                rand = Main.rand.Next(0, 10 / dustAmountMultiplier);
                                Dust dust;

                                if (rand <= 5)
                                {
                                    int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<TorchGodDust>(), 0, 0, 100, default, 1f);
                                    dust = Main.dust[dustLocation];
                                    dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);
                                    Main.dust[dustLocation].noLight = false;
                                    Main.dust[dustLocation].noLightEmittence = true;
                                    dust.color = Color.White;

                                }
                                else if (rand == 6)
                                {
                                    int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<TorchGodDustBlue>(), 0, 0, 100, default, 1f);
                                    dust = Main.dust[dustLocation];
                                    dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);
                                    Main.dust[dustLocation].noLight = false;
                                    Main.dust[dustLocation].noLightEmittence = true;
                                    dust.color = Color.White;
                                }
                                else if (rand == 7)
                                {
                                    int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<TorchGodDustOrange>(), 0, 0, 100, default, 1f);
                                    dust = Main.dust[dustLocation];
                                    dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, -.5f);
                                    Main.dust[dustLocation].noLight = false;
                                    Main.dust[dustLocation].noLightEmittence = true;
                                    dust.color = Color.White;
                                }

                                

                                /*for (int j = 0; j < 50; j++)
                                {
                                    x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                                    y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);
                                    tile = Framing.GetTileSafely(x, y);
                                    Tile shimmerTileAirCheck = Framing.GetTileSafely(x, y - 1); //checks for air

                                    if (tile.LiquidAmount >= 1 && tile.LiquidType == ModContent.Find<ModLiquid>("MurphysMod", "GodlyForgeLava").Type && shimmerTileAirCheck.LiquidAmount == 0) //forge liquid
                                    {
                                        Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.SandstormInABottle, 0, 0, 100, default, 1f);
                                    }
                                } */
                            }
                        } catch (Exception){
                            Main.NewText("Error parsing ModLiquid GodlyForgeLava from MurphysMod.");
                    }
                    } 


                    Tile tileAirCheck = Framing.GetTileSafely(x, y - 1); //checks for air
                    if (tile.LiquidAmount == 255 && tile.LiquidType == LiquidID.Water && tileAirCheck.LiquidAmount != 0)
                    { //water bubbles
                        Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.BreatheBubble, 0, 0, 100, default, 1f);
                    }


                    if (Player.ZoneUnderworldHeight) //hell
                    { //change this logic to only spawn particles off screen
                      //also make embers fade in
                        rand = Main.rand.Next(0, 20 / dustAmountMultiplier);
                        Dust dust;

                        if (Math.Abs(Player.velocity.X) >= 8f)
                        {
                            rand = Main.rand.Next(0, 15 / dustAmountMultiplier);
                        }
                        else if (Math.Abs(Player.velocity.X) >= 12f)
                        {
                            rand = Main.rand.Next(0, 10 / dustAmountMultiplier);
                        }
                        else if (Math.Abs(Player.velocity.X) >= 14f)
                        {
                            rand = Main.rand.Next(0, 5 / dustAmountMultiplier);
                        }
                        else if (Math.Abs(Player.velocity.X) >= 18f)
                        {
                            rand = Main.rand.Next(0, 2 / dustAmountMultiplier);
                        }

                        if (rand == 0)
                        {
                            int dustLocation = Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<EmberDust>(), 0, 0, 100, default, 1f);
                            dust = Main.dust[dustLocation];
                            dust.position += dust.velocity * new Vector2(Main.rand.Next(-10, 10) / 50, Main.rand.Next(-10, 10) / 25);
                        }
                        else if (rand == 1)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }
                    }

                    if (Player.ZoneUndergroundDesert) //desert
                    {
                        var sandType = DustID.Sand;
                        rand = Main.rand.Next(0, 40 / dustAmountMultiplier);

                        if (rand <= 10)
                        {
                            if (Player.ZoneCrimson)
                            {
                                sandType = DustID.Crimson;
                            }
                            else if (Player.ZoneCorrupt)
                            {
                                sandType = DustID.Corruption;
                            }
                            else if (Player.ZoneHallow)
                            {
                                sandType = DustID.Pearlsand;
                            }
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, sandType, 0, 0, 100, default, 1f);
                        }
                        else if (rand <= 20)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }
                    }

                    if (Player.ZoneGlowshroom) //mushroom biome
                    {
                        rand = Main.rand.Next(0, 20 / dustAmountMultiplier);
                        if (rand <= 3)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }
                        else if (rand == 4)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<BlueMushroomDust>(), 0, 0, 100, default, 1f);
                        }
                    }

                    if (Player.ZoneJungle && Player.position.Y / 16 >= Main.rockLayer) //jungle
                    {
                        rand = Main.rand.Next(0, 40 / dustAmountMultiplier);
                        if (rand <= 10)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, ModContent.DustType<UndergroundDust>(), 0, 0, 100, default, 1f);
                        }
                        else if (rand == 11)
                        {
                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, DustID.JungleSpore, 0, 0, 100, default, 1f);
                        }
                    }
                }
            }
        }


    }
}