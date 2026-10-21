using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MurphysMod.Content.LuckHandlers;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using MurphysMod.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;


namespace MurphysMod.Content.Enemies
{
    public class Swimming : GlobalNPC
    {
        int aiTick;
        bool canJump;
        public Vector2 direction;
        public override bool InstancePerEntity => true;
        public override void PostAI(NPC npc)
        {
            Player player = Main.player[npc.target];

            if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active && !player.active)
                return;
            if (npc.aiStyle != NPCAIStyleID.Piranha && !npc.friendly)
                return;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();
            Vector2 playerLoc = npc.DirectionTo(player.Center);
            playerLoc.Normalize();
            int intX = (int)(npc.Center.X / 16);
            int intY = (int)(npc.Top.Y / 16);
            Tile tile = Main.tile[intX, intY - 3];
            npc.noGravity = true;
            float distX = Math.Abs(npc.Center.X - player.Center.X);
            float distY = player.Center.Y - npc.Center.Y;
            float speed = 3;
            float mod;

            direction = npc.DirectionTo(player.Center);

            if (luckValue >= .25) //TODO: Make the fish go directly underneath the player. It stops short. Try using player.Center.X instead of direction.X.
            {
                if (player.wet)
                {
                    speed = 3f * (float)(1 + luckValue) * 2;
                    mod = .3f;
                }
                else
                {
                    speed = 3f;
                    mod = .05f;
                }

                if (distX <= 30 * 16 && Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) && npc.wet)
                    npc.velocity.X = MathHelper.Lerp(npc.velocity.X, (npc.velocity.X + playerLoc.X * speed) * (float)luckValue, mod);

                if (distX >= -.01f && distX <= .01f)
                    npc.velocity.X = 0f;
            }

            if (luckValue >= .5)
            {
                if (tile.LiquidAmount > 0 && !player.wet)
                {
                    npc.velocity.Y -= .05f;
                }
                if (Main.tile[intX, intY - 2].LiquidAmount < 1 && !player.wet)
                {
                    npc.velocity.Y += .05f;
                }
            }

            if (luckValue >= .75)
            {
                if (distX >= 0 && distX <= 3 * 16 && distY < 0 && Main.tile[intX, intY - 3].LiquidAmount < 1 && !player.wet && npc.wet && canJump)
                {
                    npc.velocity.Y -= 17f * (float)(1 + (luckValue / 4));
                    canJump = false;

                    if (luckValue >= .75)
                        npc.velocity.X += direction.X * 2;
                }

                if (!npc.wet)
                    npc.velocity.Y += .1f;

                aiTick++;
                if (aiTick == 300)
                {
                    aiTick = 0;
                    if (Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                        canJump = true;
                }
            }
            npc.velocity.Y = Utils.Clamp(npc.velocity.Y, -22f, 22f);
            npc.velocity.X = Utils.Clamp(npc.velocity.X, -7.5f, 7.5f);
        }

        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            Player player = Main.player[npc.target];

             if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active && !player.active)
                return;
            if (npc.aiStyle != NPCAIStyleID.Piranha && !npc.friendly)
                return;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();
            if (luckValue >= .75)
            {
                target.AddBuff(BuffID.Bleeding, 300);
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Player player = Main.player[npc.target];
             if (!BookUsed.isPlayerCursed)
                return true;
            if (!npc.active && !player.active)
                return true;
            if (npc.aiStyle != NPCAIStyleID.Piranha && !npc.friendly)
                return true;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();

            if (luckValue < .25)
                return true;

            if (direction.X > .3f)
                npc.spriteDirection = npc.direction = 1;
            else if (direction.X < .3f)
                npc.spriteDirection = npc.direction = -1;
            return true;
        }

        public override bool PreAI(NPC npc)
        {
            Player player = Main.player[npc.target];
             if (!BookUsed.isPlayerCursed)
                return true;
            if (!npc.active && !player.active)
                return true;
            if (npc.aiStyle != NPCAIStyleID.Piranha && !npc.friendly)
                return true;

            
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();

            if (luckValue < .25)
                return true;

            if (direction.X > 0f)
                npc.spriteDirection = npc.direction = 1;
            else if (direction.X < 0f)
                npc.spriteDirection = npc.direction = -1;
            return true;
        }
    }
}