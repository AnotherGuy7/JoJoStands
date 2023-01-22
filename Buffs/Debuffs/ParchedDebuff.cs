using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class ParchedDebuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parched!");
            Description.SetDefault("You could really use a drink right now...");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.moveSpeed *= 0.8f;
            player.statDefense = (int)(player.statDefense * 0.85f);
        }

        public override void OnApply(NPC npc)
        {
            npc.defense = (int)(npc.defense * (1f - (0.2f * GetDebuffOwnerModPlayer(npc).standTier)));
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Wet, npc.velocity.X);
        }
    }
}
