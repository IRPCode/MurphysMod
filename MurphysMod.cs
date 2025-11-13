using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MurphysMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

	enum MessageType : byte
    {
        totalLuckPacket
    }
	public class MurphysMod : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
			MessageType type = (MessageType)reader.ReadByte();

            if (type == MessageType.totalLuckPacket)
            {
				float multiplayerLuckPacket = reader.ReadSingle();
				

				for(int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
                    if (player.active)
                    {
						player.GetModPlayer<Content.LuckHandlers.LuckHandler>().multiplayerLuckPacket = multiplayerLuckPacket;
                    }
                }
            }
        }
    }
}
