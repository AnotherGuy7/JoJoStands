using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class WoooshDebuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("WOOOSH!");
            // Description.SetDefault("You are going aganist the wind. Speed is reduced.");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.moveSpeed *= 0.75f;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            if (npc.boss)
            {
                npc.velocity.X *= 0.95f;
                npc.velocity.Y *= 0.95f;
            }
            else
            {
                npc.velocity.X *= 0.85f;
                npc.velocity.Y *= 0.85f;
            }
        }
    }
}