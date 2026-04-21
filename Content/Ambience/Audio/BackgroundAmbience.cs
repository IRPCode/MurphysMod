using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience.Dusts;
using System;
using ModLiquidLib.ModLoader;
using ExampleMod.Common.Configs;
using Terraria.Audio;
using ReLogic.Utilities;
using MurphysMod.Content.LuckHandlers;


namespace MurphysMod
{
    public class BackgroundAmbience : ModPlayer //TODO: stopping and starting ambient sounds causes sooooo much lag >:(
    {
        public static float volume = 1f;
        public static float soundDay = 1f;
        public static float soundNight = 1f;
        private float dayVolSmooth;
        private float nightVolSmooth;
        public static int initState;
        public static float dayVol;
        public static float nightVol;
        public static float badluckVol;
        public static SlotId mainBiomeSoundDay;
        public static SlotId mainBiomeSoundNight;
        public static SlotId badLuckRumble;
        public static SlotId wind;
        public static string chosenSoundDay;
        public static string chosenSoundNight;
        public static string currentBiome;
        public static string oldBiome;
        public static bool changeBiome = false;
        public static bool flag = false;
        public static double transitionVol = 1f;

        public static String[] biomeSounds = {"MurphysMod/Assets/Audio/AmbientSounds/Purity/BackgroundLoops/ForestDay", "MurphysMod/Assets/Audio/AmbientSounds/Purity/BackgroundLoops/ForestNight",
         "MurphysMod/Assets/Audio/AmbientSounds/Jungle/JungleDay", "MurphysMod/Assets/Audio/AmbientSounds/Jungle/JungleNight"};
        public static String[] thunderSounds = { "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder1", "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder2", "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder3", "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder4", "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder5", "MurphysMod/Assets/Audio/AmbientSounds/Rain/Thunder6" };
        public static String[] puritySounds = {"MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound1", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound2",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound3", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound4",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound5", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound6",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound7", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound8",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound9"};

        //day equation: 2\sin\left(\frac{\pi x}{24}\right)-1
        //night equation: \frac{\left(\left(\sin\left(\frac{\pi x}{24}\right)\ \left(-1\right)\right)+.8\right)}{.8}\cdot1.5
        public override void PostUpdate()
        {
            if (Main.gameMenu || Main.LocalPlayer == null)
                return;

            LuckHandler luckHandler = Player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();

            float time = Utils.GetDayTimeAs24FloatStartingFromMidnight();
            float vol = time;
            float nightVol = (float)(Utils.Clamp(((((Math.Sin((Math.PI * time) / 24) * -1) + .8f) / .8) * 1.5f) - (Main.oldMaxRaining * (1 + Main.windSpeedCurrent)), 0f, 1f)) * volume;
            float dayVol = (float)(Utils.Clamp((2 * Math.Sin((Math.PI * time) / 24) - 1) - (Main.oldMaxRaining * (1 + Main.windSpeedCurrent)), 0f, 1f)) * volume;

            dayVolSmooth = (float)Math.Round(Utils.Clamp(MathHelper.Lerp(dayVolSmooth, (float)(dayVol - (Utils.Clamp(luckVal, 0f, .5f))), .05f), 0f, 1f) * (float)transitionVol, 2);
            nightVolSmooth = (float)Math.Round(Utils.Clamp(MathHelper.Lerp(nightVolSmooth, (float)(nightVol - (Utils.Clamp(luckVal, 0f, .5f))), .05f), 0f, 1f) * (float)transitionVol, 2); //fix the lerped values

            if (mainBiomeSoundNight.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundNight, out ActiveSound mainBiomeSoundNightSound) && Player.position.Y * 16f > Main.worldSurface)
            {
                mainBiomeSoundNightSound.Volume = nightVolSmooth;
            }

            if (mainBiomeSoundDay.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundDay, out ActiveSound mainBiomeSoundDaySound) && Player.position.Y * 16f > Main.worldSurface)
            {
                mainBiomeSoundDaySound.Volume = dayVolSmooth;
            }

            if (SoundEngine.TryGetActiveSound(badLuckRumble, out ActiveSound badLuckSound))
            {
                badluckVol = MathHelper.Lerp(badluckVol, (float)Utils.Clamp(luckVal - .9, 0f, .1f) * 7.5f, .05f);
                badLuckSound.Volume = badluckVol;
            }

            if (SoundEngine.TryGetActiveSound(wind, out ActiveSound windSound))
            {
                windSound.Volume = Utils.Clamp(((Math.Abs(Main.windSpeedCurrent) / .8f) * 2f * volume), .2f, .9f);
            }
            else if (Player.position.Y > Main.worldSurface)
            {
                wind = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/AmbientSounds/backgroundWind")
                {
                    IsLooped = true,
                    Volume = Utils.Clamp(((Math.Abs(Main.windSpeedCurrent) / .95f) * 2f * volume), .2f, .7f) //to get a .2 to .7 volume ratio
                });
            }

            if (Main.rand.Next(0, 100) == 1)
                playRandomSound(luckVal);

            if (Main.maxRaining >= .6f && Main.rand.Next(0, (int)((100 - ((Main.oldMaxRaining * 10) * (1 + (Main.windSpeedCurrent / .8f)))) * 10)) == 1)
                rumblingThunder();


            selectedBiome(false);

            if (currentBiome != oldBiome)
                changeBiome = true;

            if (initState == 0)
                startSounds();
            if (changeBiome)
                soundTransition();
            oldBiome = currentBiome;
        }

        public static void soundTransition()
        {
            if (changeBiome && !flag)
            {
                transitionVol = Utils.Clamp(transitionVol - .025, 0, 1);
                if (transitionVol == 0)
                {
                    flag = true;
                    initState = 0;
                    stopSounds();
                }
            }
            else if (changeBiome && flag)
            {
                transitionVol = Utils.Clamp(transitionVol + .025, 0, 1);
                if (transitionVol == 1f)
                {
                    changeBiome = false;
                    flag = false;
                }
            }
        }

        public static void startSounds()
        {
            mainBiomeSoundNight = SoundEngine.PlaySound(new SoundStyle(selectedBiome(true))
            {
                IsLooped = true,
                Volume = .5f, //volume for night sounds
                MaxInstances = 1
            });
            initState = 1;

            if (initState == 1)
            {
                mainBiomeSoundDay = SoundEngine.PlaySound(new SoundStyle(selectedBiome(false))
                {
                    IsLooped = true,
                    Volume = .5f, //volume for day sounds
                    MaxInstances = 1
                });
                initState = 2;
            }

            else if (initState == 2)
            {
                badLuckRumble = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/AmbientSounds/UnluckyRumble")
                {
                    IsLooped = true,
                    Volume = .1f,
                    MaxInstances = 1
                });
                initState = 3;
            }
        }

        public static void stopSounds()
        {
            if (mainBiomeSoundDay.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundDay, out var sound1)) sound1.Stop();
            if (mainBiomeSoundNight.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundNight, out var sound2)) sound2.Stop();
        }
        public static void playRandomSound(double luckVal)
        {
            SoundEngine.PlaySound(new SoundStyle(puritySounds[Main.rand.Next(0, puritySounds.Length)])
            {
                IsLooped = false,
                Volume = (Main.rand.Next(15, 51) / 100) * (float)((1 - Utils.Clamp(luckVal, 0f, .5f))),
                Pitch = Main.rand.Next(0, 11) / 100
            });
        }

        public static string selectedBiome(bool nightTime)
        {
            int x = 0;
            if (nightTime)
                x = 1;
            /*
            bool one = ZoneBeach || ZoneCorrupt || ZoneCrimson || ZoneDesert || ZoneDungeon || ZoneGemCave;
		bool two = ZoneGlowshroom || ZoneGranite || ZoneGraveyard || ZoneHallow || ZoneHive || ZoneJungle;
		bool three = ZoneLihzhardTemple || ZoneMarble || ZoneMeteor || ZoneSnow || ZoneUnderworldHeight;
            */
            if (Main.LocalPlayer.ZoneForest) //even indexes select the biome sound for the day, odd indexes are for night biome sounds are added later
            {
                currentBiome = "forest";
                return biomeSounds[0 + x];
            }

            else if (Main.LocalPlayer.ZoneJungle)
            {
                currentBiome = "jungle";
                return biomeSounds[2 + x];
            }
            else if (Main.LocalPlayer.ZoneBeach)
            {
                currentBiome = "beach";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneCrimson)
            {
                currentBiome = "evil";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneDesert)
            {
                currentBiome = "desert";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneDungeon)
            {
                currentBiome = "dungeon";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneLihzhardTemple)
            {
                currentBiome = "temple";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneHallow)
            {
                currentBiome = "hallow";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneSnow)
            {
                currentBiome = "snow";
                return biomeSounds[0 + x];
            }
            else if (Main.LocalPlayer.ZoneUnderworldHeight)
            {
                currentBiome = "hell";
                return biomeSounds[0 + x];
            }
            else
                return biomeSounds[0];

            //TODO: add a check for underground sounds
        }

        public static void rumblingThunder() //TODO: pan the thunder sounds, add rain noises and other noises
        {
            Main.NewText("hit");
            SoundEngine.PlaySound(new SoundStyle(thunderSounds[Main.rand.Next(0, thunderSounds.Length)])
            {
                IsLooped = false,
                Volume = (float)Main.rand.Next(0, 61) / 100,
                Pitch = Main.rand.Next(-10, 11) / 100
            });
        }
        public override void OnEnterWorld()
        {
            stopSounds();
            selectedBiome(false);
            oldBiome = currentBiome;
        }
    }
}