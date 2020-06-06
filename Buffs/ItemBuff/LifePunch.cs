using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class LifePunch : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Life Punched");
            Description.SetDefault("Your senses are accellarated and your body can't keep up with you!");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity /= 1.5f;
            player.statDefense -= 5;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity /= 1.5f;
            npc.defense -= 5;
        }
    }
}