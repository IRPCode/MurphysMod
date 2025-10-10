using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MurphysMod.Systems
{
    public class BookUsed : ModSystem
    {
        public static bool isPlayerCursed;

        public override void OnWorldLoad()
        {
            isPlayerCursed = false;
        }

        //redundancy to prevent luck system bugs

        public override void OnWorldUnload()
        {
            isPlayerCursed = false;
        }

        public override void OnModLoad()
        {
            isPlayerCursed = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if (isPlayerCursed)
            {
                tag["IsPlayerCursed"] = isPlayerCursed;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            isPlayerCursed = tag.GetBool("IsPlayerCursed");
        }
    }
}
