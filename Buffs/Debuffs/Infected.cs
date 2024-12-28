using Terraria;

namespace JoJoStands.Buffs.Debuffs
{
    public class Infected : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Infected");
            // Description.SetDefault("Some sort of otherworldly virus is spreading inside your body.");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 120;
            player.lifeRegen -= 4;
            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(player.position, player.width, player.height, 232);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            npc.lifeRegen = -12;
            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 232);
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}