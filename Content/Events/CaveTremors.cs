using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Enemies;
using MurphysMod.Content.LuckHandlers;
using MurphysMod.Content.Tiles;
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
        public static int tremorTick;

        public static int length;
        public static double intensity;
        public static int tremorType;

        public static SlotId sound;

        public static double currentIntensity;
        public static int intensityPeak;
        public static int[] dusts = { DustID.Dirt, DustID.Stone, DustID.Mud, DustID.Clay, DustID.WoodFurniture, DustID.Corruption, DustID.Crimson, DustID.JungleGrass, DustID.Sand, DustID.Snow, DustID.Ice, DustID.Ash, DustID.Corruption, DustID.Crimson, DustID.Pearlsand, DustID.Corruption, DustID.Crimson, DustID.Pearlsand, DustID.DungeonBlue, DustID.DungeonPink, DustID.DungeonGreen, DustID.GlowingMushroom, DustID.JungleGrass, DustID.Granite, DustID.Marble };
        public static int[] tileTypes = { TileID.Dirt, TileID.Stone, TileID.Mud, TileID.ClayBlock, TileID.WoodBlock, TileID.Ebonstone, TileID.Crimstone, TileID.JungleGrass, TileID.Sand, TileID.SnowBlock, TileID.IceBlock, TileID.Ash, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand, TileID.HallowHardenedSand, TileID.CorruptSandstone, TileID.CrimsonSandstone, TileID.HallowHardenedSand, TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.GreenDungeonBrick, TileID.MushroomGrass, TileID.JungleGrass, TileID.Granite, TileID.Marble };
        public override void PostUpdate()
        {
            if (Main.LocalPlayer.position.Y / 16 >= Main.worldSurface && !Player.ZoneUnderworldHeight)
            {
                Player player = Main.LocalPlayer;
                LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
                double luckVal = luckHandler.luckValue();

                if (Main.GameUpdateCount % (int)(5 * (1 + luckVal)) == 0)
                {
                    if (checkTileDestruction.tileBroken >= 0)
                    {
                        checkTileDestruction.tileBroken--;
                        checkTileDestruction.tileBroken = Utils.Clamp(checkTileDestruction.tileBroken, 0, int.MaxValue);
                        Main.NewText(checkTileDestruction.tileBroken);
                    }

                    if (checkTileDestruction.tileBroken >= 200)
                    {
                        TremorActive = true;
                    }
                }

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

                        switch (tremorType) //modifies circumstances that transform matrix is modified under
                        {
                            case 1:
                                sound = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/UndergroundTremorSmall")
                                {
                                    IsLooped = false,
                                    Volume = 3f
                                });

                                intensity = 40;
                                length = 315;
                                intensityPeak = 135;
                                TremorShake.shakeStrength = 1.25f;

                                checkTileDestruction.tileBroken = Utils.Clamp(checkTileDestruction.tileBroken - 300, 0, int.MaxValue);
                                break;

                            case 2:
                                sound = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/UndergroundTremorMedium")
                                {
                                    IsLooped = false,
                                    Volume = 3f
                                });

                                intensity = 75;
                                length = 348;
                                intensityPeak = 180;
                                TremorShake.shakeStrength = 2.5f;

                                checkTileDestruction.tileBroken = 0;
                                break;

                            case 3:
                                sound = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/UndergroundTremorLarge")
                                {
                                    IsLooped = false,
                                    Volume = 3f
                                });

                                intensity = 125;
                                length = 690;
                                intensityPeak = 420;
                                TremorShake.shakeStrength = 2.75f;

                                checkTileDestruction.tileBroken = -300;
                                break;
                        }
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
                            int damage = 50;

                            switch (tremorType) //modifies circumstances that transform matrix is modified under and projectile types
                            {
                                case 1:
                                    scale = Main.rand.Next(5, 16) / 10;
                                    spawnDebris = false;
                                    break;

                                case 2:
                                    scale = Main.rand.Next(5, 21) / 10;
                                    spawnDebris = true;
                                    chance = Main.rand.Next(1, 2001 - (int)((1 + luckVal) * 100));

                                    if (selectedTile.TileType == TileID.Stone || selectedTile.TileType == TileID.Dirt || selectedTile.TileType == TileID.Ebonstone ||
                                    selectedTile.TileType == TileID.Crimstone || selectedTile.TileType == TileID.Pearlstone || selectedTile.TileType == ModContent.TileType<AetherGrass>() ||
                                    selectedTile.TileType == TileID.Granite || selectedTile.TileType == TileID.Marble)
                                        projectileType = ProjectileID.MiniBoulder;
                                    else if (selectedTile.TileType == TileID.Mud || selectedTile.TileType == TileID.JungleGrass || selectedTile.TileType == TileID.MushroomGrass)
                                        projectileType = ModContent.ProjectileType<MudBall>();
                                    else if (selectedTile.TileType == TileID.SnowBlock || selectedTile.TileType == TileID.IceBlock)
                                        projectileType = ProjectileID.SnowBallFriendly;
                                    else if (selectedTile.TileType == TileID.HardenedSand || selectedTile.TileType == TileID.Sandstone)
                                        projectileType = ProjectileID.SandBallFalling;
                                    else if (selectedTile.TileType == TileID.HallowSandstone || selectedTile.TileType == TileID.HallowHardenedSand)
                                        projectileType = ProjectileID.PearlSandBallFalling;
                                    else if (selectedTile.TileType == TileID.CorruptSandstone || selectedTile.TileType == TileID.CorruptHardenedSand)
                                        projectileType = ProjectileID.EbonsandBallFalling;
                                    else if (selectedTile.TileType == TileID.CrimsonSandstone || selectedTile.TileType == TileID.CrimsonHardenedSand)
                                        projectileType = ProjectileID.CrimsandBallFalling;
                                    else if (selectedTile.TileType == TileID.BlueDungeonBrick)
                                        projectileType = ProjectileID.BlueDungeonDebris;
                                    else if (selectedTile.TileType == TileID.PinkDungeonBrick)
                                        projectileType = ProjectileID.PinkDungeonDebris;
                                    else if (selectedTile.TileType == TileID.GreenDungeonBrick)
                                        projectileType = ProjectileID.GreenDungeonDebris;

                                    break;

                                case 3:
                                    scale = Main.rand.Next(5, 26) / 10;
                                    spawnDebris = true;
                                    chance = Main.rand.Next(1, 2501 - (int)((1 + luckVal) * 150));

                                    if (chance <= 20)
                                    {
                                        if (selectedTile.TileType == TileID.Stone || selectedTile.TileType == TileID.Dirt || selectedTile.TileType == TileID.Ebonstone ||
                                        selectedTile.TileType == TileID.Crimstone || selectedTile.TileType == TileID.Pearlstone ||
                                        selectedTile.TileType == ModContent.TileType<AetherGrass>() || selectedTile.TileType == TileID.Granite || selectedTile.TileType == TileID.Marble)
                                            projectileType = ProjectileID.MiniBoulder;
                                        else if (selectedTile.TileType == TileID.Mud || selectedTile.TileType == TileID.JungleGrass || selectedTile.TileType == TileID.MushroomGrass)
                                            projectileType = ModContent.ProjectileType<MudBall>();
                                        else if (selectedTile.TileType == TileID.SnowBlock || selectedTile.TileType == TileID.IceBlock)
                                            projectileType = ProjectileID.SnowBallFriendly;
                                        else if (selectedTile.TileType == TileID.HardenedSand || selectedTile.TileType == TileID.Sandstone)
                                            projectileType = ProjectileID.SandBallFalling;
                                        else if (selectedTile.TileType == TileID.HallowSandstone || selectedTile.TileType == TileID.HallowHardenedSand)
                                            projectileType = ProjectileID.PearlSandBallFalling;
                                        else if (selectedTile.TileType == TileID.CorruptSandstone || selectedTile.TileType == TileID.CorruptHardenedSand)
                                            projectileType = ProjectileID.EbonsandBallFalling;
                                        else if (selectedTile.TileType == TileID.CrimsonSandstone || selectedTile.TileType == TileID.CrimsonHardenedSand)
                                            projectileType = ProjectileID.CrimsandBallFalling;
                                        else if (selectedTile.TileType == TileID.BlueDungeonBrick)
                                            projectileType = ProjectileID.BlueDungeonDebris;
                                        else if (selectedTile.TileType == TileID.PinkDungeonBrick)
                                            projectileType = ProjectileID.PinkDungeonDebris;
                                        else if (selectedTile.TileType == TileID.GreenDungeonBrick)
                                            projectileType = ProjectileID.GreenDungeonDebris;
                                    }


                                    else if (chance <= 25)
                                    {
                                        if (selectedTile.TileType == TileID.Stone || selectedTile.TileType == TileID.Dirt || selectedTile.TileType == TileID.Ebonstone ||
                                        selectedTile.TileType == TileID.Crimstone || selectedTile.TileType == TileID.Pearlstone || selectedTile.TileType == TileID.Granite || selectedTile.TileType == TileID.Marble ||
                                        selectedTile.TileType == ModContent.TileType<AetherGrass>())
                                            projectileType = ProjectileID.Boulder;

                                        else if (selectedTile.TileType == TileID.Mud || selectedTile.TileType == TileID.JungleGrass || selectedTile.TileType == TileID.MushroomGrass ||
                                        selectedTile.TileType == TileID.BlueDungeonBrick ||
                                        selectedTile.TileType == TileID.PinkDungeonBrick || selectedTile.TileType == TileID.GreenDungeonBrick)
                                            projectileType = ProjectileID.MiniBoulder;

                                        else if (selectedTile.TileType == TileID.HardenedSand || selectedTile.TileType == TileID.Sandstone ||
                                        selectedTile.TileType == TileID.CrimsonSandstone || selectedTile.TileType == TileID.CrimsonHardenedSand ||
                                        selectedTile.TileType == TileID.CorruptSandstone || selectedTile.TileType == TileID.CorruptHardenedSand ||
                                        selectedTile.TileType == TileID.HallowSandstone || selectedTile.TileType == TileID.HallowHardenedSand)
                                            projectileType = ProjectileID.RollingCactus;

                                        damage = 100;

                                        if (projectileType == ProjectileID.MiniBoulder)
                                            damage = 75;
                                        else if (projectileType == ProjectileID.SnowBallFriendly)
                                            damage = 25;
                                    }

                                    else if (chance == 26 && luckVal >= .15f)
                                    {
                                        if (selectedTile.TileType == TileID.Stone || selectedTile.TileType == TileID.Dirt || selectedTile.TileType == TileID.Ebonstone ||
                                        selectedTile.TileType == TileID.Crimstone || selectedTile.TileType == TileID.Pearlstone || selectedTile.TileType == TileID.HallowSandstone ||
                                        selectedTile.TileType == TileID.HallowHardenedSand || selectedTile.TileType == TileID.JungleGrass || selectedTile.TileType == TileID.MushroomGrass ||
                                        selectedTile.TileType == ModContent.TileType<AetherGrass>())
                                            projectileType = ProjectileID.BouncyBoulder;

                                        if (selectedTile.TileType == TileID.Mud || selectedTile.TileType == TileID.JungleGrass)
                                        {
                                            if (player.ZoneJungle)
                                                projectileType = ProjectileID.BeeHive;
                                            else
                                                projectileType = ProjectileID.Boulder;
                                        }


                                        damage = 75;
                                    }

                                    break;
                                default:
                                    Main.NewText("CaveTremors class threw an error.");
                                    break;
                            }

                            Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, dusts[index], 0, 0, 100, default, (float)scale);

                            Vector2 velocity;

                            if (projectileType == ProjectileID.SnowBallFriendly || projectileType == ModContent.ProjectileType<MudBall>() || projectileType == ModContent.ProjectileType<ToxicJungleSpore>())
                                velocity = new Vector2(0, 10);
                            else
                                velocity = default;

                            if (spawnDebris == true && chance <= 26 && projectileType != -1)
                            {
                                int projID = Projectile.NewProjectile(
                                default,
                                new Vector2(x * 16f, (y + 1) * 16f),
                                velocity,
                                projectileType,
                                damage,
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

                    if (Main.LocalPlayer.position.Y / 16 >= Main.worldSurface && !Player.ZoneUnderworldHeight && CaveTremors.TremorActive == true && (tremorTick <= length))
                    {
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && npc.position.Y / 16 >= Main.worldSurface && !(npc.position.Y / 16 >= Main.bottomWorld - 200) && TremorActive)
                                npc.AddBuff(BuffID.Slow, 1);

                        }

                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            player = Main.player[i];
                            if (player.active && Main.LocalPlayer.position.Y / 16 >= Main.worldSurface && !Player.ZoneUnderworldHeight && TremorActive)
                                Player.AddBuff(BuffID.Slow, 1);
                        }
                    }
                }
            }
            else
            {
                if (SoundEngine.TryGetActiveSound(sound, out var activeSound))
                {
                    activeSound.Volume *= .95f;
                }

            }
        }

        public class checkTileDestruction : GlobalTile
        {
            public static int tileBroken;
            public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
            {
                if (fail)
                    return;   
                else if (Main.tileSolid[type])
                {
                    tileBroken++;
                }
            }
        }

        public class TremorShake : ModSystem
        {
            public static float shakeStrength;

            public static float multiplyStrength;

            public override void ModifyTransformMatrix(ref SpriteViewMatrix transform)
            {
                Player player = Main.LocalPlayer;

                if (CaveTremors.TremorActive == true && (tremorTick <= length)) //checks just in case if player teleports, only works underground
                {
                    float randX = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);
                    float randY = Main.rand.Next((int)-shakeStrength, (int)shakeStrength);

                    if (tremorTick == 1)
                    {
                        multiplyStrength *= shakeStrength;
                    }

                    if (CaveTremors.tremorTick < CaveTremors.intensityPeak && Main.LocalPlayer.position.Y / 16 >= Main.worldSurface && !player.ZoneUnderworldHeight)
                        multiplyStrength *= 1.0005f;
                    else if (Main.LocalPlayer.position.Y / 16 <= Main.worldSurface || player.ZoneUnderworldHeight)
                        multiplyStrength *= .95f;
                    else
                        multiplyStrength *= .99f;

                    if ((Main.LocalPlayer.position.Y / 16 <= Main.worldSurface || player.ZoneUnderworldHeight) && multiplyStrength <= .01f) //prevents visual bugs with player/background items
                    {
                        multiplyStrength = 0f;
                        CaveTremors.TremorActive = false;
                    }

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

                    if (multiplyStrength <= .05f)
                        CaveTremors.TremorActive = false;
                }
                else
                {
                    multiplyStrength = 1f;
                }
            }
        }
    }
}
