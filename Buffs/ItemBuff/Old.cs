using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Old : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Old");
            Description.SetDefault("Your knees are shaking, you feel powerless and tired.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }
        public bool alreadyChangeApplied = false;
        public int damageMultiplication = 1;

        public override void Update(Player player, ref int buffIndex)
        {
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 120;
                player.lifeRegen -= 4 * damageMultiplication;
            }
            player.moveSpeed *= 0.8f;
            player.meleeDamage *= 0.75f;
            player.rangedDamage *= 0.75f;
            player.magicDamage *= 0.75f;
            player.meleeSpeed *= 0.5f;
            player.statDefense = (int)(player.statDefense * 0.8f);
            if (player.ZoneSnow || player.ZoneSkyHeight)
            {
                damageMultiplication = 0;
            }
            if (player.ZoneUnderworldHeight)
            {
                damageMultiplication = 3;
            }
            if (player.ZoneDesert || player.ZoneUndergroundDesert)
            {
                damageMultiplication = 2;
            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (npc.boss)
            {
                npc.lifeRegen = -2 * damageMultiplication;
                npc.damage -= 15;
            }
            else
            {
                npc.lifeRegen = -4 * damageMultiplication;
                npc.velocity.X *= 0.9f;
                if(!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.9f);
                }
            }
            if (player.ZoneSnow || player.ZoneSkyHeight)
            {
                damageMultiplication = 0;
            }
            if (player.ZoneUnderworldHeight)
            {
                damageMultiplication = 3;
            }
            if (player.ZoneDesert || player.ZoneUndergroundDesert)
            {
                damageMultiplication = 2;
            }
            if (modPlayer.gratefulDeadTier == 2)
            {
                npc.lifeRegen = -4 * damageMultiplication;
                npc.velocity.X *= 0.8f;
                if (!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.8f);
                }
            }
            if (modPlayer.gratefulDeadTier == 3)
            {
                npc.lifeRegen = -8 * damageMultiplication;
                npc.velocity.X *= 0.6f;
                if (!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.6f);
                }
            }
            if (modPlayer.gratefulDeadTier == 4)
            {
                npc.lifeRegen = -16 * damageMultiplication;
                npc.velocity.X *= 0.4f;
                if (!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.4f);
                }
            }
        }
    }
}