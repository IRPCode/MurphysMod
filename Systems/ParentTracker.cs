using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;



namespace MurphysMod.Systems
{
    public class NPCParentTracker : GlobalNPC {

        public override bool InstancePerEntity => true;

        public NPC npcParent;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if(source is EntitySource_Parent {Entity : NPC parent})
                npcParent = parent;
        }
    }

    public class projectileParentTracker : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public NPC projectileParent;

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if(source is EntitySource_Parent {Entity : NPC parent})
                projectileParent = parent;
        }
    }
}