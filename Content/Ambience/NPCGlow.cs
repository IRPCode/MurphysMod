//Objects that should glow faintly:

//Sunflowers at night, gemcorn trees, hellstone brick (ancient too), 

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MurphysMod.Content.Ambience
{

    public class NPCGlow : GlobalNPC
    {
        public float intensity;
        public Boolean flag;
        public override bool InstancePerEntity => true;
        public static int[] acceptedSquirrelSources = {NPCID.GemSquirrelAmber, NPCID.GemSquirrelAmethyst, NPCID.GemSquirrelDiamond,
        NPCID.GemSquirrelEmerald, NPCID.GemSquirrelRuby,  NPCID.GemSquirrelSapphire,  NPCID.GemSquirrelTopaz};

        public static int[] acceptedRabbitSources = { NPCID.GemBunnyAmber, NPCID.GemBunnyAmethyst, NPCID.GemBunnyDiamond,
        NPCID.GemBunnyEmerald, NPCID.GemBunnyRuby, NPCID.GemBunnySapphire, NPCID.GemBunnyTopaz,};

        public static Color[] colors = { Color.Orange, Color.Purple, Color.White, Color.Green, Color.Red, Color.Blue, Color.Yellow };

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            #region gemcorn critters

            if (acceptedSquirrelSources.Contains(npc.type) || acceptedRabbitSources.Contains(npc.type))
            {
                for (int x = 0; x < acceptedSquirrelSources.Length; x++)
                {
                    if (npc.type == acceptedRabbitSources[x] || npc.type == acceptedSquirrelSources[x])
                    {
                         if (colors[x] == Color.Blue)
                            Lighting.AddLight(new Vector2(npc.position.X + .5f, npc.position.Y + .5f), (new Vector3(colors[x].R, colors[x].G * intensity, colors[x].B * intensity) / 255f) * .2f * intensity);

                        else if (colors[x] == Color.Red)
                            Lighting.AddLight(new Vector2(npc.position.X + .5f, npc.position.Y + .5f), (new Vector3(colors[x].R  * intensity, colors[x].G * intensity, colors[x].B) / 255f) * .2f * intensity);
                        else
                            Lighting.AddLight(new Vector2(npc.position.X + .5f, npc.position.Y + .5f), (new Vector3(colors[x].R , colors[x].G * intensity, colors[x].B * intensity) / 255f) * .2f * intensity);

                        strength();
                    }
                }
            }

            #endregion

        }

        public void strength()
        {
            int x = Tick.globalTick;

            if (x % 2 == 0)
            {
                if (flag)
                    intensity += .00025f;
                else
                    intensity -= .00025f;
            }

            if (intensity < .9f)
            {
                flag = true;
                intensity = .9f;
            }
            else if (intensity > 1.5f)
            {
                flag = false;
                intensity = 1.5f;
            }
        }
    }
}