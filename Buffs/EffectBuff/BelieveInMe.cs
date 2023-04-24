using JoJoStands.Buffs.Debuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.EffectBuff
{
    public class BelieveInMe : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Shine;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Believe in me!");
            // Description.SetDefault("You are ready to stand for such a reliable guy to the end!");
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.noKnockback = true;
            player.longInvince = true;
            player.endurance += 0.1f;
            player.moveSpeed *= 1.25f;
            if (player.HasBuff(ModContent.BuffType<SMACK>()))
                player.ClearBuff(ModContent.BuffType<SMACK>());
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<SMACK>()))
                npc.DelBuff(ModContent.BuffType<SMACK>());
        }
    }
}