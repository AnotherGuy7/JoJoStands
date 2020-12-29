using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Data.SqlTypes;

namespace JoJoStands.Buffs.Debuffs
{
    public class Exposing : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Take a peek");
            Description.SetDefault("You’re vulnerable to an enemy attack!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("Exposing")))
            {
                MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                player.noFallDmg = true;
                player.controlUseItem = false;
                player.controlUseTile = false;
                if (player.HasBuff(BuffID.Suffocation))
                {
                    player.ClearBuff(BuffID.Suffocation);
                }
            }
        }

    }
}