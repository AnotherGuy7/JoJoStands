using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class BitesTheDustCoolDown : ModBuff     //make the icon a sun going normally
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Bite the Dust Cooldown");
            Description.SetDefault("You can no longer bite the dust...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}