using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using MurphysMod.Content.Ambience;

namespace MurphysMod.Content.Ambience.Dusts
{

    public class UndergroundDust : ModDust
    {
        public override string Texture => "MurphysMod/Content/Ambience/Dusts/UndergroundDust";

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.scale = .25f;
            dust.fadeIn = 1f;
        }

        public override bool Update(Dust dust)
        {
            int x = Tick.globalTick;
            dust.position += dust.velocity;

            if (x % 30 == 0)
            {
                return true;
            }

            else {
                return false;
            }


        }
    }
}