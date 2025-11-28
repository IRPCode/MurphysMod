using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Content.Tiles
{
	public class AetherTreeLeaf : ModGore
	{
		public override string Texture => "MurphysMod/Assets/Textures/Tiles/Plants/AetherTreeLeaf";

		public override void SetStaticDefaults() {
			ChildSafety.SafeGore[Type] = true;
			GoreID.Sets.SpecialAI[Type] = 3; 
			GoreID.Sets.PaintedFallingLeaf[Type] = true;
		}
	}
}