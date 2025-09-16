using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace MurphysMod.Content.Buffs
{
	public class Portent : ModBuff
	{
		public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/Portent";
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}


		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "Adds a lot of bad luck";
		}
	}
}