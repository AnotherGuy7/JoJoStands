using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class Dead : JoJoBuff
    {
        public override void UpdateBuffOnNPC(NPC npc)
        {
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.lifeRegenExpectedLossPerSecond = npc.lifeMax + 1;
            npc.lifeRegen -= npc.lifeMax + 1;
        }
    }
}