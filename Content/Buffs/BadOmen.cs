using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace MurphysMod.Content.Buffs
{
	public class BadOmen : ModBuff
	{
				public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/BadOmen";
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}


		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "Adds a moderate amount of bad luck.";
		}
	}
}