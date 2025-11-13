using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.LuckHandlers;

namespace MurphysMod.Content.Enemies
{
    public class MagmaKnifeProjectile : ModProjectile
    {
        public override String Texture => "MurphysMod/Assets/Textures/Items/Weapons/MagmaKnife";
        int dustAmount = 10;
        int dustType = DustID.OrangeTorch;
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 2;
            Projectile.alpha = default;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckVal = luckHandler.luckValue();

            if (luckVal >= .25f)
            {
                dustAmount = 9;
            }
            else if (luckVal >= .5f)
            {
                dustAmount = 7;
            }
            else if (luckVal >= .75f)
            {
                dustAmount = 5;
                dustType = DustID.Lava;
            }
            else if (luckVal >= 1f)
            {
                dustAmount = 1;
                dustType = DustID.RedTorch;
            }

            if (Main.rand.NextBool(dustAmount))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            npc.AddBuff(BuffID.OnFire, 30);
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void OnKill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground

            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.OrangeTorch, Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 100, default, 1f);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("MurphysMod/Assets/Textures/Items/Weapons/MagmaKnifeGlowMask");
            Main.spriteBatch.Draw
            (
                texture,
                Projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                Projectile.rotation,
                texture.Size() * 0.5f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}