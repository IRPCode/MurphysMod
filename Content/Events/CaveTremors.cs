using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;


namespace MurphysMod.Content
{

    public class CaveTremors : ModPlayer
    {
        public static bool TremorActive = false;
        public static readonly SoundStyle tremorL = new("MurphysMod/Assets/Audio/UndergroundTremorLarge")
        {
            Volume = 1.5f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static readonly SoundStyle tremorM = new("MurphysMod/Assets/Audio/UndergroundTremorMedium")
        {
            Volume = 1.5f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static readonly SoundStyle tremorS = new("MurphysMod/Assets/Audio/UndergroundTremorSmall")
        {
            Volume = 1.5f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static int tremorTick;

        public static int length;
        public static int intensity;
        public static int offset = 24; //only used for offsetting tick for files that don't immediately start
        public static int intensityPeak;
        public static int[] dusts = { DustID.Dirt, DustID.Stone, DustID.Mud, DustID.Clay, DustID.WoodFurniture, DustID.Corruption, DustID.Crimson, DustID.JungleGrass, DustID.Sand, DustID.Snow, DustID.Ice, DustID.Ash };
        public static int[] tileTypes = { TileID.Dirt, TileID.Stone, TileID.Mud, TileID.ClayBlock, TileID.WoodBlock, TileID.Ebonstone, TileID.Crimstone, TileID.JungleGrass, TileID.Sand, TileID.SnowBlock, TileID.IceBlock, TileID.Ash };
        public override void PostUpdate()
        {
            TremorActive = true;

            if(tremorTick > 60 * 15)
                tremorTick = 0;

            if (TremorActive)
            {
                tremorTick++;

                int tremorType = Main.rand.Next(1, 4);

                if (tremorTick == 1)
                {
                    SlotId tremorSound;
                    switch (tremorType) //modifies circumstances that transform matrix is modified under
                    {
                        case 1:
                            tremorSound = SoundEngine.PlaySound(tremorS);
                            intensity = 25;
                            length = 315;
                            intensityPeak = 135;
                            break;

                        case 2:
                            tremorSound = SoundEngine.PlaySound(tremorM);
                            intensity = 50;
                            length = 348;
                            intensityPeak = 180;
                            break;

                        case 3:
                            tremorSound = SoundEngine.PlaySound(tremorL);
                            intensity = 100;
                            length = 690;
                            intensityPeak = 420;
                            break;
                    }
                    //TODO: figure out how to make the sound stop if the player teleports
                        //if(Main.LocalPlayer.position.Y / 16 >= Main.rockLayer){}
                        //tremorSound.Stop();
                }

                for (int i = 0; i < intensity; i++)
                {
                    int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                    int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);

                    Tile selectedTile = Framing.GetTileSafely(x, y);
                    Tile belowtile = Framing.GetTileSafely(x, y + 1);

                    if (tileTypes.Contains(selectedTile.TileType) && !belowtile.HasTile && selectedTile.HasTile)
                    {
                        int index = Array.IndexOf(tileTypes, Framing.GetTileSafely(x, y).TileType);

                        Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, dusts[index], 0, 0, 100, default, 1f);

                        //TODO: use tiletype array to find what dust to select, then do a check if air is by block, spawn dust, play sound, make screen shake, and then spawn small boulders or large depending on what intensity is selected on random
                    }
                }

                //1 == small, 2 == medium, 3 == large
                //Small starts at .4 seconds, climaxes at 2.25 seconds, ends at 5.25 seconds (315 total ticks)
                //Medium starts at 0, climaxes at 3 seconds, and ends at 5.8 seconds (348 ticks)
                //Large starts at .4 seconds, climaxes at 4.8 seconds, continues until 7 seconds, fades out until 11.5 seconds (690)

            }


        }

        public class TremorShake : ModSystem
        {
            private float shakeStrength = 1f;

            private float multiplyStrength = 1f;

            public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
            {
                if (CaveTremors.TremorActive == true && (tremorTick <= length) &&  Main.LocalPlayer.position.Y / 16 >= Main.rockLayer) //checks just in case if player teleports, only works underground
                {
                    float randX = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);
                    float randY = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);

                    multiplyStrength *= 1.00005f;

                    Vector2 displaceScreen = new Vector2(randX, randY) * multiplyStrength;

                    Main.screenPosition += displaceScreen;

                    if (Main.gameMenu)
                    {
                        return;
                    }

                }
                else
                {
                    multiplyStrength = 1f;
                }
            }
        }
    }
}
