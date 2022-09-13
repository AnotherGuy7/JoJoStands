using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.NPCs;

namespace JoJoStands.Buffs.Debuffs
{
    public class ParchedDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Parched!");
            Description.SetDefault("You could really use a drink right now...");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.8f;
            player.statDefense = (int)(player.statDefense * 0.85f);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            Player player = Main.player[npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().standDebuffEffectOwner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            npc.velocity.X *= 0.85f;
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Wet, npc.velocity.X);
            if (!npc.GetGlobalNPC<JoJoGlobalNPC>().oneTimeEffectsApplied)
            {
                npc.defense = (int)(npc.defense * (1f - (0.2f * mPlayer.standTier)));
                npc.GetGlobalNPC<JoJoGlobalNPC>().oneTimeEffectsApplied = true;
            }
        }

    }
}
    