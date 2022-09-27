using Terraria;
using Terraria.ModLoader;
using JoJoStands.Buffs.PlayerBuffs;

namespace JoJoStands.Buffs.Debuffs
{
    public class SMACK : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SMACK!");
            Description.SetDefault("It's just unbearable! The sounds in your head is getting louder!");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.AddBuff(ModContent.BuffType<Sound>(), 600);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.AddBuff(ModContent.BuffType<Sound>(), 360);
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                player.ClearBuff(buffIndex);
        }
    }
}