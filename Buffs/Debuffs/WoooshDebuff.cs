using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class WhooshDebuff : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Slow; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("WOOOSH!");
            Description.SetDefault("You are going aganist the wind. Speed is reduced.");
            Main.debuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.75f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.boss)
            {
                npc.velocity.X *= 0.95f;
                npc.velocity.Y *= 0.95f;
            }
            if (!npc.boss)
            {
                npc.velocity.X *= 0.85f;
                npc.velocity.Y *= 0.85f;
            }
        }
    }
}