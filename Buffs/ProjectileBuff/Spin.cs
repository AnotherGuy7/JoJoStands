using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ProjectileBuff
{
    public class Spin : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Spin");
            Description.SetDefault("You're being infinitely spun... There's no hope in surviving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 60;
            player.moveSpeed -= 2;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.AddBuff(BuffID.Confused, 95);
            npc.lifeRegen -= 20;
        }
    }
}