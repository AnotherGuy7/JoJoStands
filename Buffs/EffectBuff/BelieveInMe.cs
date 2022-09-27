using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BelieveInMe : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Regeneration; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Believe in me!");
            Description.SetDefault("You are ready to stand for such a reliable guy to the end!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
            player.longInvince = true;
            player.moveSpeed *= 1.5f;
            if (player.HasBuff(ModContent.BuffType<SMACK>()))
                player.ClearBuff(ModContent.BuffType<SMACK>());
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(ModContent.BuffType<SMACK>()))
                npc.DelBuff(ModContent.BuffType<SMACK>());
        }
    }
}