using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.MinionBuff
{
    public class StandControlMouse : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Stand Control: Mouse");
            Description.SetDefault("You are currently in control of your Stand! \nControl your stand using left-click to move and right-click to shoot!");
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