using Terraria;
using Terraria.ModLoader;

namespace MurphysMod.Content.Ambience {

    public class Tick : ModSystem {
        
        public static int globalTick;

        public override void PostUpdateEverything()
        {
            globalTick++;
        }
    }
}