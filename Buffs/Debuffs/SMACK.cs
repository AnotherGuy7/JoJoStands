using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.PlayerBuffs;

namespace JoJoStands.Buffs.Debuffs
{
    public class SMACK : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Horrified; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SMACK!");
            Description.SetDefault("It's just unbearable! The sounds in your head is getting louder!");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.AddBuff(ModContent.BuffType<Tinnitus>(), 600);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(ModContent.BuffType<Tinnitus>(), 360);
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }
    }
}