using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs
{
    public class JoJoBuff : ModBuff
    {
        /// <summary>
        /// Gets called when this buff is applied to a player.
        /// </summary>
        public virtual void OnApply(Player player)
        { }

        /// <summary>
        /// Gets called when this buff is applied to an NPC.
        /// </summary>
        public virtual void OnApply(NPC npc)
        { }

        public int buffIndex = -1;

        public override sealed void Update(Player player, ref int buffIndex)
        {
            if (this.buffIndex != buffIndex)
            {
                this.buffIndex = buffIndex;
                OnApply(player);
            }

            UpdateBuffOnPlayer(player);
            if (!player.HasBuff(Type))
                OnBuffEnd(player);
        }

        public virtual void UpdateBuffOnPlayer(Player player)
        { }

        public Player GetDebuffOwner(NPC npc) => Main.player[npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().standDebuffEffectOwner];
        public MyPlayer GetDebuffOwnerModPlayer(NPC npc) => Main.player[npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().standDebuffEffectOwner].GetModPlayer<MyPlayer>();

        public override sealed void Update(NPC npc, ref int buffIndex)
        {
            if (this.buffIndex != buffIndex)
            {
                this.buffIndex = buffIndex;
                OnApply(npc);
            }

            UpdateBuffOnNPC(npc);
            if (!npc.HasBuff(Type))
                OnBuffEnd(npc);
        }

        public virtual void UpdateBuffOnNPC(NPC npc)
        { }

        public virtual void OnBuffEnd(Player player)
        { }

        public virtual void OnBuffEnd(NPC npc)
        { }
    }
}
