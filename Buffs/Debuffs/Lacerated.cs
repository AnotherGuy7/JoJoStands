using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Lacerated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lacerated");
            Description.SetDefault("You have been wounded deeply.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 30;
            player.lifeRegen -= 26;

            if (Main.rand.Next(0, 2 + 1) == 0)
                Dust.NewDust(player.position, player.width, player.height, DustID.Blood);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            if (Main.rand.Next(0, 2 + 1) == 0)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f, 0, Scale: 2f);

            int lifeLossPerSecond = npc.lifeMax / 56;
            if (Main.hardMode)
                if (lifeLossPerSecond >= 51)
                    lifeLossPerSecond = 51;
                else
                if (lifeLossPerSecond >= 16)
                    lifeLossPerSecond = 16;
            if (npc.boss)
                lifeLossPerSecond /= 2;

            npc.lifeRegenExpectedLossPerSecond = lifeLossPerSecond / 12;
            npc.lifeRegen -= lifeLossPerSecond;
        }
    }
}