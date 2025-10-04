using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MurphysMod.Content.LuckHandlers;
using rail;

namespace MurphysMod.Content.Enemies
{
    public class RustySwordProjectile : ModProjectile //Idea canned due to complexity
    {
        public override String Texture => "MurphysMod/Assets/Textures/Projectiles/BoneSword";
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.timeLeft = Main.rand.Next(100, 300);
            Projectile.aiStyle = 0;
            Projectile.alpha = default;
            Projectile.friendly = false;
            Projectile.hostile = true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return true;
        }

        private bool flag;
        private bool NPCDirection;
        public override void AI()
        {
            int npcIndex = (int)Projectile.ai[0];
            if (npcIndex >= 0 && npcIndex < Main.maxNPCs && Main.npc[npcIndex].active)
            {
                NPC npc = Main.npc[npcIndex];

                if (Main.npc[npcIndex].active)
                {
                    if (!flag && npc.direction == 1)
                    {
                        Projectile.rotation = -1.57f;
                        flag = true;
                        NPCDirection = true;
                    }
                    else if (!flag && npc.direction == 0)
                    {
                        Projectile.rotation = 1.57f;
                        flag = true;
                        NPCDirection = false;
                    }


                    //Projectile.rotation += .1f;
                    //Vector2 offset = new Vector2(-50f, 1000f);
                    //offset = offset.RotatedBy(Projectile.rotation);
                    //Projectile.Center = (npc.Center + offset) * npc.direction;
                    Projectile.position = npc.Center + new Vector2(8 * npc.direction, -12);

                    if (NPCDirection)
                        Projectile.rotation += .1f;
                    else
                        Projectile.rotation -= .1f;

                    Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt() * -1; //... * -1 ???
                    //Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);

                    if (npc.direction == 0)
                    {
                        DrawOriginOffsetX = -27;
                        DrawOriginOffsetY = -40;
                    }
                    else
                    {
                        //DrawOffsetX = 0;
                        DrawOriginOffsetX = -27;
                        DrawOriginOffsetY = -40;
                        Projectile.spriteDirection = 1; //modify all instances of spriteDirection to fix the sword's blade alignment 
                        //you may need to inverse the sword's direction.
                    }

                    Main.NewText(Projectile.spriteDirection + ", " + Projectile.rotation);

                }

                if (Projectile.rotation < 1.57f == false)
                {
                    Projectile.Kill();
                }
                else if (Projectile.rotation < -2.79f == true ) //TODO: fix the sword not properly dying on left swings, reposition the sword on left swings,
                //and mirror the projectile's sprite properly.
                {
                    Projectile.Kill();
                }
            }
        }

#pragma warning disable CS0672 // Member overrides obsolete member
        public override void Kill(int timeLeft)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position); //fix this to find the real sound needed for arrows hitting the ground
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}