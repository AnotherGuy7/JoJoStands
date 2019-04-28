using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Items;

namespace JoJoStands.NPCs
{
    public class JoJoGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore && Main.rand.NextFloat() < 0.0574f) //5.74% is .0574f chance
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RequiemArrow"), 1);
            }

            if(Main.hardMode == true && Main.rand.NextFloat() < 0.0438f) //should be a 4.38% chance of dropping from any enemy in hardmode
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SoulofTime"), Main.rand.Next(1,3));      //mininum amount = 1, maximum amount = 3
            }

            if(Main.rand.NextFloat() < 0.0327f)     //should be a 3.27% chance of dropping from an enemy
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SunDroplet"), Main.rand.Next(1,3));     //this could drop either 0, 1, or 2. Main.rand.Next counts zeroes as well. Now changed to a minimum value of 1 and a maximum value of 3.
            }
            if (npc.type == NPCID.Bird || npc.type == NPCID.BirdBlue || npc.type == NPCID.BirdRed || npc.type == NPCID.GoldBird && Main.rand.NextFloat() < 0.0574f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WrappedPicture"), 1);
            }
            if (Main.rand.NextFloat() < 0.0286f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("Hand"), 1);
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (MyPlayer.TheWorldEffect)
            {
                npc.velocity.X *= 0f;
                npc.velocity.Y *= 0f;               //negative X is to the left, negative Y is UP
                npc.frameCounter = 1;
                npc.HitSound = mod.GetLegacySoundSlot(SoundType.NPCHit, "Sounds/NPCHit/This_is_intended_to_be_a_silent_sound");     //since in stopped time, they aren't supposed to notice
                if (npc.noGravity == false)
                {
                    npc.velocity.Y -= 0.3f;     //the default gravity value, so that if enemies have gravity enabled, this velocity counters that gravity
                }
                return false;
            }
            return true;
        }
    }
}