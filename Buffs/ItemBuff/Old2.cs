using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class Old2 : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Old");
            Description.SetDefault("You can feel your entire life energy leaving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public bool alreadyChangeApplied = false;

        public override void Update(Player player, ref int buffIndex)
        {
            {
                if (player.lifeRegen > 0)
                {
                    player.lifeRegen = 0;
                }
                player.lifeRegenTime = 120;
                player.lifeRegen -= 16;
            }
            player.moveSpeed *= 0.8f;
            player.meleeDamage *= 0.75f;
            player.rangedDamage *= 0.75f;
            player.magicDamage *= 0.75f;
            player.meleeSpeed *= 0.5f;
            player.statDefense = (int)(player.statDefense * 0.8f);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (npc.boss)
            {
                npc.lifeRegen = -20;
                npc.damage -= 15;
            }
            else
            {
                npc.lifeRegen = -250;
                npc.velocity.X *= 0.8f;
                if (!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.2f);
                }
            }
            if (modPlayer.gratefulDeadTier == 3)
            {
                npc.lifeRegen = -350;
            }
            if (modPlayer.gratefulDeadTier == 4)
            {
                npc.lifeRegen = -500;
            }
        }
    }
}