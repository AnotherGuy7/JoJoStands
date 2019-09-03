using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class GERAbilityCooldown : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Gold Experience Requiem Ability Cooldown");
            Description.SetDefault("You can no longer use requiem abilites...");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }
    }
}