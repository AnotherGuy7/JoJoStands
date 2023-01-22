using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class RedBindDebuff : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Bind");
            Description.SetDefault("You are bound by fire...");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.velocity /= 1.5f;
            player.statDefense -= 15;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            npc.velocity.X = 0f;
            npc.AddBuff(BuffID.OnFire, 2);
            npc.AddBuff(BuffID.Suffocation, 2);
        }
    }
}