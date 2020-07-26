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
            player.allDamage *= 2f;
            player.moveSpeed *= 2f;
            player.jumpBoost = true;
            player.manaRegen *= 2;
            player.meleeSpeed *= 2f;
            player.noFallDmg = true;
            player.lifeRegenCount += 5;
            player.arrowDamage *= 2f;
            player.statLifeMax2 = player.statLifeMax + 100;

            if (player.ZoneSkyHeight && MyPlayer.SecretReferences)
            {
                player.AddBuff(mod.BuffType("SpaceFreeze"), 2, true);
            }
            if (player.HasBuff(mod.BuffType("Vampire")))
            {
                player.ClearBuff(mod.BuffType("Vampire"));
            }
        }
    }
}