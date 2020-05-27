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
            Description.SetDefault("Idk Yet");
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
                player.lifeRegen -= 4;
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
            if (npc.boss)
            {
                npc.lifeRegen = -2;
                npc.damage -= 15;
            }
            else
            {
                npc.lifeRegen = -4;
                npc.velocity.X *= 0.8f;
                if(!alreadyChangeApplied)
                {
                    npc.defense = (int)(npc.defense * 0.8f);
                }
            }
        }
    }
}