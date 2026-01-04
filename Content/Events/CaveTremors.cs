using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using MurphysMod.Content.LuckHandlers;
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
            Volume = 2f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static readonly SoundStyle tremorM = new("MurphysMod/Assets/Audio/UndergroundTremorMedium")
        {
            Volume = 2f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static readonly SoundStyle tremorS = new("MurphysMod/Assets/Audio/UndergroundTremorSmall")
        {
            Volume = 2f,
            Pitch = 0f,
            PitchVariance = 0f
        };

        public static int tremorTick;

        public static int length;
        public static double intensity;
        public static int tremorType;

        public static double currentIntensity;
        public static int intensityPeak;
        public static int[] dusts = { DustID.Dirt, DustID.Stone, DustID.Mud, DustID.Clay, DustID.WoodFurniture, DustID.Corruption, DustID.Crimson, DustID.JungleGrass, DustID.Sand, DustID.Snow, DustID.Ice, DustID.Ash };
        public static int[] tileTypes = { TileID.Dirt, TileID.Stone, TileID.Mud, TileID.ClayBlock, TileID.WoodBlock, TileID.Ebonstone, TileID.Crimstone, TileID.JungleGrass, TileID.Sand, TileID.SnowBlock, TileID.IceBlock, TileID.Ash };
        public override void PostUpdate()
        {
            TremorActive = true;
            

            if (Main.LocalPlayer.position.Y / 16 >= Main.rockLayer)
            {
                if (tremorTick > 60 * 15)
                {
                    tremorTick = 0;
                    TremorShake.multiplyStrength = .5f;
                    TremorActive = false;
                }

                if (TremorActive)
                {
                    tremorTick++;

                    if (tremorTick == 1)
                    {
                        tremorType = Main.rand.Next(1, 4);
                        currentIntensity = 1;
                        SlotId tremorSound;
                        switch (tremorType) //modifies circumstances that transform matrix is modified under
                        {
                            case 1:
                                tremorSound = SoundEngine.PlaySound(tremorS);
                                intensity = 40;
                                length = 315;
                                intensityPeak = 135;
                                TremorShake.shakeStrength = 1.25f;
                                break;

                            case 2:
                                tremorSound = SoundEngine.PlaySound(tremorM);
                                intensity = 75;
                                length = 348;
                                intensityPeak = 180;
                                TremorShake.shakeStrength = 2.5f;
                                break;

                            case 3:
                                tremorSound = SoundEngine.PlaySound(tremorL);
                                intensity = 125;
                                length = 690;
                                intensityPeak = 420;
                                TremorShake.shakeStrength = 2.75f;
                                break;
                        }
                        //TODO: figure out how to make the sound stop if the player teleports
                        //if(Main.LocalPlayer.position.Y / 16 >= Main.rockLayer){}
                        //tremorSound.Stop();
                    }

                    if (CaveTremors.tremorTick < CaveTremors.intensityPeak && intensity >= currentIntensity)
                        currentIntensity *= 1.1f;
                    else
                        currentIntensity *= .99f;

                    for (int i = 0; i < currentIntensity; i++)
                    {
                        int x = (int)(Player.Center.X / 16) + Main.rand.Next(-60, 60);
                        int y = (int)(Player.Center.Y / 16) + Main.rand.Next(-36, 36);

                        Tile selectedTile = Framing.GetTileSafely(x, y);
                        Tile belowtile = Framing.GetTileSafely(x, y + 1);

                        if (tileTypes.Contains(selectedTile.TileType) && !belowtile.HasTile && selectedTile.HasTile)
                        {
                            int index = Array.IndexOf(tileTypes, Framing.GetTileSafely(x, y).TileType);

                            double scale = 1f;

                            bool spawnDebris = false;
                            int chance = -1;
                            int projectileType = -1;
                            int damage = 15; //TODO: balance this, 150 does 1.5k dmg but not to enemies (double check this)

                            Player player = Main.LocalPlayer;
                            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                            double luckVal = luckHandler.luckValue();

                            switch (tremorType) //modifies circumstances that transform matrix is modified under and projectile types
                            {
                                case 1:
                                    scale = Main.rand.Next(5, 16) / 10;
                                    spawnDebris = false;
                                    break;

                                case 2:
                                    scale = Main.rand.Next(5, 21) / 10;
                                    spawnDebris = true;
                                    chance = Main.rand.Next(1, 2001 - (int)(luckVal * 1000));
                                    projectileType = ProjectileID.MiniBoulder;

                                    //TODO: Make the ceiling drop stalagtites and mini boulders (only large at max bad luck), give player and NPCs slowness during tremor
                                    break;

                                case 3:
                                    scale = Main.rand.Next(5, 26) / 10;
                                    spawnDebris = true;
                                    chance = Main.rand.Next(1, 2501 - (int)(luckVal * 1000));

                                    if (chance <= 20)
                                        projectileType = ProjectileID.MiniBoulder;

                                    else if (chance <= 25)
                                    {
                                        projectileType = ProjectileID.Boulder;
                                        damage = 20;
                                    }
                                        
                                    else if (chance == 26 && luckVal >= .75f)
                                    {
                                        projectileType = ProjectileID.BouncyBoulder;
                                        damage = 25;
                                    }
                                        
                                    break;
                                default:
                                    Main.NewText("CaveTremors class threw an error.");
                                break;
                            }

                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, dusts[index], 0, 0, 100, default, (float)scale);

                            if (spawnDebris == true && chance <= 26)
                            {
                                int projID = Projectile.NewProjectile(
                                default,
                                new Vector2(x * 16f, (y + 1) * 16f),
                                default,
                                projectileType,
                                (int)(damage * (luckVal + .5f)), //for damage balancing
                                default,
                                -1);

                                Projectile proj = Main.projectile[projID];
                                proj.hostile = true;
                                proj.friendly = true;
                                proj.tileCollide = true;
                                proj.netUpdate = true;
                            }

                        }
                    }
                }
            }
        }

        public class TremorShake : ModSystem
        {
            public static float shakeStrength;

            public static float multiplyStrength;

            public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
            {
                if (CaveTremors.TremorActive == true && (tremorTick <= length) && Main.LocalPlayer.position.Y / 16 >= Main.rockLayer) //checks just in case if player teleports, only works underground
                {
                    float randX = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);
                    float randY = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);

                    if (tremorTick == 1)
                    {
                        multiplyStrength *= shakeStrength;
                    }

                    if (CaveTremors.tremorTick < CaveTremors.intensityPeak)
                        multiplyStrength *= 1.0005f;
                    else
                        multiplyStrength *= .99f;

                    Vector2 displaceScreen = new Vector2(randX, randY) * multiplyStrength;

                    Main.screenPosition += displaceScreen;

                    if (Main.gameMenu)
                    {
                        return;
                    }

                    if (CaveTremors.tremorTick >= CaveTremors.length)
                    {
                        shakeStrength = 0f;
                        CaveTremors.TremorActive = false;
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
