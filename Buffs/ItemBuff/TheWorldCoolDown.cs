using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class TheWorldCoolDown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("The World Cooldown");
            Description.SetDefault("You can no longer stop time...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}