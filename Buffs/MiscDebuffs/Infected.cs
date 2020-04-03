using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.MiscDebuffs
{
    public class Infected : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Infected!");
            Description.SetDefault("The virus from the meteorite has infected you!");
            Main.debuff[Type] = true;
            Main.persistentBuff[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegenTime = 0;
            player.lifeRegen = -10;
        }
    }
}