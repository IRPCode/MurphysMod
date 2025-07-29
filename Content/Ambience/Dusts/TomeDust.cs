using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class TomeDust : ModDust
    {
        public override string Texture => "MurphysMod/Content/Ambience/Dusts/TomeDust";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = .5f;
            dust.fadeIn = 1f;
            dust.alpha = 0;
        }

        public override bool Update(Dust dust)
        {
            int x = Tick.globalTick;
            dust.position += dust.velocity;

            return true;


        }
    }
}