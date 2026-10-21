using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MurphysMod.Content.LuckHandlers;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using MurphysMod.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using MurphysMod.Content.Projectiles;


namespace MurphysMod.Content.Enemies
{
    public class SalamanderAI : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int[] Salamander = {NPCID.Salamander, NPCID.Salamander2, NPCID.Salamander3, NPCID.Salamander4, NPCID.Salamander5,
        NPCID.Salamander6, NPCID.Salamander7, NPCID.Salamander8, NPCID.Salamander9};
        bool part;
        private bool oldPart;
        private float ai2Old;
        public override void PostAI(NPC npc)
        {
            Player player = Main.player[npc.target];

            if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active && !player.active)
                return;
            if (!Salamander.Contains(npc.type))
                return;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();
            Vector2 playerLoc = npc.DirectionTo(player.Center);
            playerLoc.Normalize();

            if ((luckValue >= .25 && Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) || luckValue >= .75) && npc.ai[1] < 1)
            {
                float mult = 1f + (float)luckHandler.luckValue();
                npc.velocity.X = MathHelper.Lerp(npc.oldVelocity.X, npc.direction * mult, .025f);
            }

            if (ai2Old != npc.ai[2] && luckValue >= .75)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == ProjectileID.SalamanderSpit)
                    {
                        int projID = Projectile.NewProjectile(
                          npc.GetSource_FromAI(),
                          npc.Center,
                          Main.projectile[i].velocity,
                          ModContent.ProjectileType<SalamanderSpit>(),
                          Main.projectile[i].damage,
                          Main.projectile[i].knockBack,
                          -1);

                        Projectile proj = Main.projectile[projID];
                        proj.hostile = true;
                        proj.friendly = false;
                        proj.tileCollide = true;
                        proj.netUpdate = true;

                        Main.projectile[i].Kill();
                    }
                }
            }
            ai2Old = npc.ai[2];
        }
        public Color color;
        public Color currentCol;
        public override Color? GetAlpha(NPC npc, Color drawColor)
        {
            if (!Salamander.Contains(npc.type))
                return null;

            Player player = Main.player[npc.target];
            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();

            if (luckValue >= .5)
            {
                float dist = Vector2.Distance(player.Center, npc.Center);

                if (dist >= 25 * 16f)
                    color = drawColor * (float)(.5f - (luckValue * .3));
                else
                    color = drawColor;
                currentCol = Color.Lerp(currentCol, color, .05f);

                return currentCol;
            }
            else
                return drawColor;


        }
    }
}