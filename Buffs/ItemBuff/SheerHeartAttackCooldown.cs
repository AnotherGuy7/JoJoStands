using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class SheerHeartAttackCooldown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Sheer Heart Attack Cooldown");
            Description.SetDefault("You can no longer send out Sheer Heart Attack...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}