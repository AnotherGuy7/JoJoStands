using JoJoStands.Buffs.ItemBuff;
using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Aging : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Aging");
            // Description.SetDefault("Your knees are shaking, you feel powerless and tired.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        private bool oneTimeEffectsApplied = false;
        private int damageMultiplication = 1;
        private float savedVelocityX = -1f;

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<CooledOut>()))
                return;

            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 120;
            player.lifeRegen -= 8 * damageMultiplication;
            player.moveSpeed *= 0.94f;
            player.GetDamage(DamageClass.Generic) *= 0.75f;
            player.GetDamage(DamageClass.Generic) *= 0.5f;
            player.statDefense = (int)(player.statDefense * 0.8f);

            if (player.ZoneSnow || player.ZoneSkyHeight)
                damageMultiplication = 0;
            else
                damageMultiplication = 1;
            if (player.ZoneDesert)
                damageMultiplication = 2;
            if (player.ZoneUnderworldHeight)
                damageMultiplication = 3;
        }

        public override void OnApply(NPC npc)
        {
            savedVelocityX = Math.Abs(npc.velocity.X) / GetDebuffOwnerModPlayer(npc).standTier;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            Player player = GetDebuffOwner(npc);
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (player.ZoneSnow || player.ZoneSkyHeight)
                damageMultiplication = 0;
            else
                damageMultiplication = 1;
            if (player.ZoneDesert)
                damageMultiplication = 2;
            if (player.ZoneUnderworldHeight)
                damageMultiplication = 3;

            if (Math.Abs(npc.velocity.X) > savedVelocityX)
                npc.velocity.X *= 0.9f;

            int extraDamage = (int)(npc.life * 0.001f * mPlayer.standTier * damageMultiplication);
            npc.lifeRegen = (-36 * mPlayer.standTier) * damageMultiplication - extraDamage;
            if (!oneTimeEffectsApplied)
            {
                npc.defense = (int)(npc.defense * (1f - 0.2f * mPlayer.standTier));
                npc.damage = (int)((npc.damage * (1f - 0.1f * mPlayer.standTier)) - 10);
                oneTimeEffectsApplied = true;
            }
        }
    }
}