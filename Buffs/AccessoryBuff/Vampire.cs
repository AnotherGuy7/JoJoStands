using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.AccessoryBuff
{
    public class Vampire : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Vampire");
            Description.SetDefault("You are now a vampire... Stay away from the sun!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.ZoneOverworldHeight)
            {
                player.statLifeMax2 = 150;
            }
            if (player.ZoneSkyHeight)
            {
                player.AddBuff(mod.BuffType("SpaceFreeze"), 1, true);
                player.statLifeMax2 = 150;
            }
            if (Main.dayTime == false)
            {
                player.meleeDamage *= 2f;
                player.thrownDamage *= 2f;
                player.rangedDamage *= 2f;
                player.magicDamage *= 2f;
                player.moveSpeed *= 2f;
                player.jumpBoost = true;
                player.manaRegen = 2;
                player.meleeSpeed *= 2f;
                player.noFallDmg = true;
                player.statLifeMax2 = 150;
                player.lifeRegenCount += 5;
            }
            if (Main.dayTime && player.ZoneOverworldHeight)
            {
                player.meleeDamage *= 2f;
                player.thrownDamage *= 2f;
                player.rangedDamage *= 2f;
                player.magicDamage *= 2f;
                player.moveSpeed *= 2f;
                player.jumpBoost = true;
                player.meleeSpeed *= 2f;
                player.noFallDmg = true;
                player.statLifeMax2 = 150;
                player.AddBuff(mod.BuffType("Sunburn"), 1, true);
            }
        }
    }
}