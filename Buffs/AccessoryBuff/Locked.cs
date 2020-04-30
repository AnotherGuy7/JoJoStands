using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Locked : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Locked");
            Description.SetDefault("Your guilt is increasing and it hurts.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}