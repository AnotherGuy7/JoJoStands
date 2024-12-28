using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class TimeSkipConfusion : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Time has skipped?");
            // Description.SetDefault("What you just saw was your future self.");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            npc.velocity = Vector2.Zero;
            npc.AddBuff(BuffID.Confused, 2);
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.velocity = Vector2.Zero;
            player.AddBuff(BuffID.Confused, 2);
        }
    }
}