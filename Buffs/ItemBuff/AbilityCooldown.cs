using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class AbilityCooldown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Ability Cooldown");
            Description.SetDefault("You can no longer use any stand abilities...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            canBeCleared = false;
        }
    }
}