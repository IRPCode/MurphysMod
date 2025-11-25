using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using MurphysMod.Content.Ambience.Dusts;
using System.Linq;

namespace MurphysMod.Content.Buffs
{
	public class Oleaginous : ModBuff
	{
		public override String Texture => "MurphysMod/Assets/Textures/BuffIcons/Oleaginous";
		public override void SetStaticDefaults() //Check luckhandler.cs for luck modifications
		{
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = "You feel disgusting and unlucky.";
		}

		public override void Update(Player player, ref int buffIndex)
		{
			int rand = Main.rand.Next(0,11);
			player.AddBuff(BuffID.Darkness, 1);
			player.AddBuff(BuffID.Oiled, 1);


			if (rand == 0)
			{
				Dust.NewDust(new Vector2(player.position.X, player.position.Y), 16, 16, ModContent.DustType<AccursedSapDust>(), 0, 0, 100, default, 1f);
			}

			
			player.GetModPlayer<OleaginousPlayer>().Oleaginous = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.AddBuff(BuffID.Oiled, 1);
			npc.AddBuff(BuffID.Slow, 1);

			int rand = Main.rand.Next(0,11);

			if (rand == 0)
			{
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 16, 16, ModContent.DustType<BlessedWaterDust>(), 0, 0, 100, default, 1f);
			}
        }
	}

	public class OleaginousPlayer : ModPlayer{
		public bool Oleaginous;
		public override void ResetEffects()
		{
			Oleaginous = false;
		}
        public override void UpdateLifeRegen() //slightly boosts healing
		{
			if (Oleaginous)
			{
				Player.lifeRegenTime++;
				Player.lifeRegen += 2;

			}
		}   
	}
}