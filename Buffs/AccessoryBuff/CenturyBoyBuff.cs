using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class CenturyBoyBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("20th Century Boy");
            Description.SetDefault("You are being protected!");
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.controlUseItem = false;
            player.moveSpeed = 0;
            player.lifeRegen = 0;
            player.immune = true;
            player.manaRegen = 0;
        }
    }
}