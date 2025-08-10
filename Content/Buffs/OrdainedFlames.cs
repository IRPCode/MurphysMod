using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Mono.CompilerServices.SymbolWriter;
using MurphysMod.Content.Ambience;
using Microsoft.Xna.Framework;

namespace MurphysMod.Content.Buffs
{
	public class OrdainedFlames : ModBuff
	{
		public override void SetStaticDefaults() //TODO: make this debuff actually take health away
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "You are melting away!";
		}

        public override void Update(Player player, ref int buffIndex)
        {
			if (player.statLife >= 15)
			{
				player.lifeRegen -= 15;
				Dust.NewDust(new Vector2(player.position.X, player.position.Y), 16, 16,  DustID.SpelunkerGlowstickSparkle, 0, 0, 100, default, 1f);
			}
        }   
	}
}