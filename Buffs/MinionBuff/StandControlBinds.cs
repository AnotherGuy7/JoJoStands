using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.MinionBuff
{
    public class StandControlBinds : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Stand Control: Binds");
            Description.SetDefault("You are currently in control of your Stand! \nControl your stand using the binds Up, Down, Left, Right, and Attack");
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlTorch = false;
        }
    }
}