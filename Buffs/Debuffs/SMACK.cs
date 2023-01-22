using JoJoStands.Buffs.PlayerBuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class SMACK : JoJoBuff
    {
        public override string Texture => "Terraria/Images/Buff_" + BuffID.Horrified;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SMACK!");
            Description.SetDefault("It's just unbearable! The sounds in your head is getting louder!");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.AddBuff(ModContent.BuffType<Tinnitus>(), 6 * 60);
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            npc.AddBuff(ModContent.BuffType<Tinnitus>(), 10 * 60);
        }
    }
}