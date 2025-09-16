using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using System;

namespace MurphysMod.Content.Buffs
{
	public class Doomed : ModBuff
	{
		public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/Doomed";
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "Marked for death by the torch god";
		}
	}
}