using System;
using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class HazeVirus : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spin");
            // Description.SetDefault("You're being infinitely spun... There's no hope in surviving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.lifeRegen = -60;
            player.moveSpeed /= 2;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            int healthASecond = 2 + GetDebuffOwnerModPlayer(npc).standTier;
            npc.lifeRegen -= healthASecond * 2 * 120;
            if (Math.Abs(npc.velocity.X) > 1f)
                npc.velocity.X *= 0.5f;
        }
    }
}