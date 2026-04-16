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
    public class BackgroundAmbience : ModPlayer
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
        String[] biomeSounds = {"MurphysMod/Assets/Audio/AmbientSounds/Purity/BackgroundLoops/ForestDay", "MurphysMod/Assets/Audio/AmbientSounds/Purity/BackgroundLoops/ForestNight",
         "MurphysMod/Assets/Audio/AmbientSounds/Jungle/JungleDay", "MurphysMod/Assets/Audio/AmbientSounds/Jungle/JungleNight"};

        public static String[] puritySounds = {"MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound1", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound2",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound3", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound4",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound5", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound6",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound7", "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound8",
        "MurphysMod/Assets/Audio/AmbientSounds/Purity/Birds/BirdSound9"};

        public override void OnEnterWorld()
        {
            if (mainBiomeSoundDay.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundDay, out var sound1)) sound1.Stop();
            if (mainBiomeSoundNight.IsValid && SoundEngine.TryGetActiveSound(mainBiomeSoundNight, out var sound2)) sound2.Stop();
        }

        //day equation: 2\sin\left(\frac{\pi x}{24}\right)-1
        //night equation: \frac{\left(\left(\sin\left(\frac{\pi x}{24}\right)\ \left(-1\right)\right)+.8\right)}{.8}\cdot1.5
        public override void PostUpdate()
        {
            LuckHandler luckHandler = Player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();

            float time = Utils.GetDayTimeAs24FloatStartingFromMidnight();
            float vol = time;
            float nightVol = (float)Utils.Clamp(((((Math.Sin((Math.PI * time) / 24) * -1) + .8f) / .8) * 1.5f), 0f, 1f) * volume;
            float dayVol = (float)Utils.Clamp((2 * Math.Sin((Math.PI * time) / 24) - 1), 0f, 1f) * volume;

            dayVolSmooth = (float)Utils.Clamp(MathHelper.Lerp(dayVolSmooth, (float)(dayVol - (Utils.Clamp(luckVal, 0f, .8f))), .05f), 0f, 1f);
            nightVolSmooth = (float)Utils.Clamp(MathHelper.Lerp(nightVolSmooth, (float)(nightVol - (Utils.Clamp(luckVal, 0f, .8f))), .05f), 0f, 1f); //fix the lerped values

            /*Main.NewText("Day: " + dayVol);
            Main.NewText("Day smoothed: " + dayVolSmooth);
            Main.NewText("Night: " + nightVol);
            Main.NewText("Night smoothed: " + nightVolSmooth);*/


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
                badluckVol = MathHelper.Lerp(badluckVol, (float)Utils.Clamp(luckVal - .75, 0f, .25f) * 3f, .05f);
                badLuckSound.Volume = badluckVol;
            }


            if (SoundEngine.TryGetActiveSound(wind, out ActiveSound windSound))
            {
                windSound.Volume = Utils.Clamp(((Math.Abs(Main.windSpeedCurrent) / .8f) * 2f * volume), .2f, .65f);
            }
            else if (Player.position.Y > Main.worldSurface)
            {
                wind = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/AmbientSounds/backgroundWind")
                {
                    IsLooped = true,
                    Volume = Utils.Clamp(((Math.Abs(Main.windSpeedCurrent) / .8f) * 2f * volume), .2f, .65f) //to get a .2 to .6 volume ratio
                });
            }

            if (initState == 0)
            {
                mainBiomeSoundNight = SoundEngine.PlaySound(new SoundStyle(biomeSounds[1])
                {
                    IsLooped = true,
                    Volume = .1f, //volume for night sounds
                    MaxInstances = 1
                });
                initState = 1;
            }

            if (initState == 1)
            {
                mainBiomeSoundDay = SoundEngine.PlaySound(new SoundStyle(biomeSounds[0])
                {
                    IsLooped = true,
                    Volume = .1f, //volume for day sounds
                    MaxInstances = 1
                });
                initState = 2;
            }

            if (initState == 2)
            {
                badLuckRumble = SoundEngine.PlaySound(new SoundStyle("MurphysMod/Assets/Audio/AmbientSounds/UnluckyRumble")
                {
                    IsLooped = true,
                    Volume = .1f,
                    MaxInstances = 1
                });
                initState = 3;
            }

            if (Main.rand.Next(0, 100) == 0)
                playRandomSound();
        }
        public static void playRandomSound()
        {
            SoundEngine.PlaySound(new SoundStyle(puritySounds[Main.rand.Next(0, puritySounds.Length)])
            {
                IsLooped = false,
                Volume = Main.rand.Next(15, 51) / 100,
                Pitch = Main.rand.Next(0, 11) / 100
            });
        }
    }
}