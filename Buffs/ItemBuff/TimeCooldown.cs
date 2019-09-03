using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class TimeCooldown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Time Ability Cooldown");
            Description.SetDefault("You can no longer use abilities related to time...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}