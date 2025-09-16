using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content //TODO: if in water, have a weapon that does a sonar ping that blows up everything, including yourself, 9999 damage to non boss enemies
{
    public class PlayerDeath : ModPlayer
    {
        public bool onFire = false;
        public static readonly SoundStyle deathSound1 = new("MurphysMod/Assets/Audio/noscream")
        {
            Volume = 1f,
            Pitch = 0f,
            PitchVariance = .2f
        };

        public static readonly SoundStyle deathSound2 = new("MurphysMod/Assets/Audio/laugh")
        {
            Volume = 5f,
            Pitch = 0f,
            PitchVariance = .2f
        };

        public static readonly SoundStyle deathSound3 = new("MurphysMod/Assets/Audio/scream")
        {
            Volume = 1f,
            Pitch = 0f,
            PitchVariance = .2f
        };
        public static readonly SoundStyle deathSound4 = new("MurphysMod/Assets/Audio/shotgun")
        {
            Volume = .65f,
            Pitch = 0f,
            PitchVariance = .2f
        };

          public static readonly SoundStyle deathSound5 = new("MurphysMod/Assets/Audio/laughslow")
        {
            Volume = 1f,
            Pitch = 0f,
            PitchVariance = .2f
        };

        public static readonly SoundStyle fireSound = new("MurphysMod/Assets/Audio/OnFireYell") //case all messed up because my IDE refuses to let it be all lowercase >:(
        {
            Volume = 1f,
            Pitch = 0f
        };

        public override void PostUpdate() //TODO: This sound no longer plays. Likely due to the tick class you have implemented.
        {
            if (Player.HasBuff(BuffID.OnFire))
            {
                int totalTime = Player.buffTime[Array.IndexOf(Player.buffType, BuffID.OnFire)];

                if (Player.statLife == 10 && totalTime >= 30)
                {
                    SoundEngine.PlaySound(fireSound, Player.Center);
                    onFire = true;
                }
            }
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            string deathText;

            int r = Main.rand.Next(1, 16);

            switch (r)
            {
                case 1:
                    deathText = "L.";
                    break;
                case 2:
                    deathText = "Bonk!";
                    break;
                case 3:
                    deathText = "!#?*";
                    break;
                case 4:
                    deathText = "Skill Issue";
                    break;
                case 5:
                    deathText = "Unlucky...";
                    break;
                case 6:
                    deathText = "Wow.";
                    break;
                case 7:
                    deathText = ":O";
                    break;
                case 8:
                    deathText = "Hah!";
                    break;
                case 9:
                    deathText = "Andrew Approved.";
                    break;
                case 10:
                    deathText = ":(";
                    break;
                case 11:
                    deathText = "Couldn't be me.";
                    break;
                case 12:
                    deathText = "Nice.";
                    break;
                case 13:
                    deathText = "Pfft.";
                    break;
                case 14:
                    deathText = "Well";
                    break;
                case 15:
                    deathText = "Do better.";
                    break;
                default:
                    deathText = "Oops! This shouldn't happen. If you are seeing this, please report it.";
                    break;
            }

            if (onFire)
            {
                deathText = "AAA-";
                onFire = false;
            }

            r = Main.rand.Next(0, 101);
            if (!Player.HasBuff(BuffID.OnFire)) //prevents fire screaming from overlapping
            {
                switch (r)
                {
                    case 100:
                        SoundEngine.PlaySound(deathSound1, Player.Center);
                        deathText = "NOOOOOO!";
                        break;

                    case 99:
                        SoundEngine.PlaySound(deathSound2, Player.Center);
                        deathText = "AAAAAA!";
                        break;

                    case 98:
                        SoundEngine.PlaySound(deathSound3, Player.Center);
                        deathText = "WAAA!";
                        break;

                    case 97:
                        SoundEngine.PlaySound(deathSound4, Player.Center);
                        deathText = "shotgun.wav";
                        break;

                    case 96:
                        SoundEngine.PlaySound(deathSound5, Player.Center);
                        deathText = "MMHAHAHAHAHAHA!";
                        break;
                }
            }

            CombatText.NewText(
                Player.getRect(),
                Color.IndianRed,
                deathText,
                true
            );
        }
    }
}