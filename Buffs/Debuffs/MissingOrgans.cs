using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class MissingOrgans : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Missing Organs!");
            Description.SetDefault("Parts of your body have been scraped away!");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= 20;
            player.moveSpeed *= 0.6f;
            Dust.NewDust(player.position, player.width, player.height, DustID.Blood, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.lifeRegenExpectedLossPerSecond = 20;
            npc.lifeRegen -= 60;
            npc.velocity *= 0.6f;
        }
    }
}