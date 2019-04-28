using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class Zipped : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Zipped!");
            Description.SetDefault("A zipper has been placed on your body, blood is spilling quickly!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= 20;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f, 0, default(Color), 2);
            npc.lifeRegenExpectedLossPerSecond = 20;
            npc.lifeRegen -= 60;
            npc.AddBuff(BuffID.Slow, 5);
        }
    }
}