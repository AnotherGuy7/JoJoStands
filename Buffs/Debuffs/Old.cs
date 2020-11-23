using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Old : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Old");
            Description.SetDefault("Your knees are shaking, you feel powerless and tired.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        private bool oneTimeEffectsApplied = false;
        private int damageMultiplication = 1;
        private float savedVelocityX = -1f;

        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.HasBuff(mod.BuffType("CooledOut")))
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 120;
                player.lifeRegen -= 4 * damageMultiplication;
                player.moveSpeed *= 0.94f;
                player.meleeDamage *= 0.75f;
                player.rangedDamage *= 0.75f;
                player.magicDamage *= 0.75f;
                player.meleeSpeed *= 0.5f;
                player.statDefense = (int)(player.statDefense * 0.8f);

                if (player.ZoneSnow || player.ZoneSkyHeight)
                {
                    damageMultiplication = 0;
                }
                if (player.ZoneUndergroundDesert)
                {
                    damageMultiplication = 1;
                }
                if (player.ZoneDesert)
                {
                    damageMultiplication = 2;
                }
                if (player.ZoneUnderworldHeight)
                {
                    damageMultiplication = 3;
                }
            }

        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (savedVelocityX == -1)
            {
                savedVelocityX = Math.Abs(npc.velocity.X) / modPlayer.gratefulDeadTier;
            }

            if (npc.boss)
            {
                npc.lifeRegen = -2 * damageMultiplication;
                if (!oneTimeEffectsApplied)
                {
                    npc.damage -= 15;
                    oneTimeEffectsApplied = true;
                }
            }
            else
            {
                npc.lifeRegen = -4 * damageMultiplication;
                if (Math.Abs(npc.velocity.X) > savedVelocityX)
                {
                    npc.velocity.X *= 0.9f;
                }
                if (!oneTimeEffectsApplied)
                {
                    npc.defense = (int)(npc.defense * 0.9f);
                    oneTimeEffectsApplied = true;
                }
            }

            if (player.ZoneSnow || player.ZoneSkyHeight)
            {
                damageMultiplication = 0;
            }
            if (!player.ZoneUndergroundDesert)
            {
                damageMultiplication = 1;
            }
            if (player.ZoneDesert)
            {
                damageMultiplication = 2;
            }
            if (player.ZoneUnderworldHeight)
            {
                damageMultiplication = 3;
            }

            if (Math.Abs(npc.velocity.X) > savedVelocityX)
            {
                npc.velocity.X *= 0.9f;
            }
            if (modPlayer.gratefulDeadTier == 2)
            {
                npc.lifeRegen = -4 * damageMultiplication;
                if (!oneTimeEffectsApplied)
                {
                    npc.defense = (int)(npc.defense * 0.8f);
                    oneTimeEffectsApplied = true;
                }
            }
            if (modPlayer.gratefulDeadTier == 3)
            {
                npc.lifeRegen = -8 * damageMultiplication;
                if (!oneTimeEffectsApplied)
                {
                    npc.defense = (int)(npc.defense * 0.6f);
                    oneTimeEffectsApplied = true;
                }
            }
            if (modPlayer.gratefulDeadTier == 4)
            {
                npc.lifeRegen = -16 * damageMultiplication;
                if (!oneTimeEffectsApplied)
                {
                    npc.defense = (int)(npc.defense * 0.4f);
                    oneTimeEffectsApplied = true;
                }
            }
        }
    }
}