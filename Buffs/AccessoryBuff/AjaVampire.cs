using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class AjaVampire : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aja Vampire");
            Description.SetDefault("You are now an immortal, ultimate being!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.moveSpeed *= 2f;
            player.jumpBoost = true;
            player.manaRegen *= 2;
            player.noFallDmg = true;
            player.lifeRegenCount += 5;
            player.arrowDamage *= 2f;
            player.statLifeMax2 += 100;
            player.buffTime[buffIndex] = 2;
            vPlayer.perfectBeing = true;
            vPlayer.anyMaskForm = true;
            player.GetDamage(DamageClass.Generic) *= 2f;
            player.GetAttackSpeed(DamageClass.Generic) *= 2f;

            if (player.ZoneSkyHeight && MyPlayer.SecretReferences)
                player.AddBuff(ModContent.BuffType<SpaceFreeze>(), 2, true);

            if (player.HasBuff(ModContent.BuffType<Vampire>()))
                player.ClearBuff(ModContent.BuffType<Vampire>());

            if (player.HasBuff(ModContent.BuffType<Zombie>()))
                player.ClearBuff(ModContent.BuffType<Zombie>());
        }
    }
}