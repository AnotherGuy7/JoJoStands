using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.AccessoryBuff
{
    public class AjaVampire : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("Aja Vampire");
            Description.SetDefault("You are now an immortal, ultimate being!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            //canBeCleared = false;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(mod.BuffType("AjaVampire"), 2, true);
            player.meleeDamage *= 3f;
            player.thrownDamage *= 3f;
            player.rangedDamage *= 3f;
            player.magicDamage *= 3f;
            player.moveSpeed *= 3f;
            player.jumpBoost = true;
            player.manaRegen = 3;
            player.meleeSpeed *= 3f;
            player.noFallDmg = true;
            player.lifeRegenCount += 5;
            player.arrowDamage *= 3f;
            player.statLifeMax2 = player.statLifeMax + 100;
            if (player.ZoneSkyHeight && MyPlayer.SecretReferences)
            {
                player.AddBuff(mod.BuffType("SpaceFreeze"), 2, true);
            }
        }
    }
}