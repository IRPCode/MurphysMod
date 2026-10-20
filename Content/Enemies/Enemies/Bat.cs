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
    public class Bat : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void PostAI(NPC npc)
        {
            Player player = Main.player[npc.target];

            if (!BookUsed.isPlayerCursed)
                return;
            if (!npc.active && !player.active)
                return;
            if (npc.aiStyle != NPCAIStyleID.Bat)
                return;

            LuckHandler luckHandler = player.GetModPlayer<LuckHandler>();
            double luckValue = luckHandler.luckValue();
            Vector2 vectorLuck = new Vector2((float)Math.Pow(1 + luckValue, 2), (float)Math.Pow(1 + luckValue, 2));
            Vector2 playerLoc = npc.DirectionTo(player.Center);
            playerLoc.Normalize();

            if(luckValue >= .25 && Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(player.Center) * vectorLuck, .0075f);

            if (npc.ai[1] == 150 && luckValue >= .5)
            {
                double radius = ((1 + luckValue) * (15f * 16f));

                if (Vector2.Distance(player.Center, npc.Center) <= radius && Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                {
                    npc.velocity += playerLoc;
                    npc.velocity = Vector2.Normalize(npc.velocity) * 7;
                    npc.velocity *= (float)(1 + luckValue);
                } 
            }

            if (npc.ai[0] == -300 && Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height) && luckValue >= .75)
            {
                int projID = Projectile.NewProjectile(
                           npc.GetSource_FromAI(),
                           npc.Center,
                           playerLoc * 10f,
                           ModContent.ProjectileType<RabidSpit>(),
                           15, //for damage balancing
                           default,
                           -1);

                Projectile proj = Main.projectile[projID];
                proj.hostile = true;
                proj.friendly = false;
                proj.tileCollide = true;
                proj.netUpdate = true;
                npc.ai[0] = 0;
                
            }       
            npc.ai[0]--;
        }
    }
}