using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using MurphysMod.Content.Projectiles;
using System;
using MurphysMod.Content.LuckHandlers;
using System.Linq;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using MurphysMod.Systems;

namespace MurphysMod.Content.Enemies
{
    //When 1.4.5 gets to tmodloader, add code for the moss zombie here
    public class DemonEye : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int[] acceptedNPCs = { 2, -43, 190, -38, 191, -39, 192, -40, 193, -41, 194, -42, 317, 318, 133 };
        public Vector2 savedVel;

        public override void PostAI(NPC npc)
        {
            if (!BookUsed.isPlayerCursed)
                return;
            if (!acceptedNPCs.Contains(npc.type) && !npc.active)
                return;
            Player target = Main.player[npc.target];
            LuckHandler luckHandler = target.GetModPlayer<LuckHandler>();
            Vector2 vectorLuck = new Vector2((float)Math.Pow(1 + luckHandler.luckValue(), 3.75), (float)Math.Pow(1 + luckHandler.luckValue(), 3.75));
            if (!Main.dayTime)
                npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(target.Center) * vectorLuck, .0075f);

            if (npc.type == NPCID.WanderingEye && npc.life < npc.lifeMax / 2 && luckHandler.luckValue() >= .5)
                npc.noTileCollide = true;
        }
    }
}