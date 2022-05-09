using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Infected : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infected");
            Description.SetDefault("Some sort of otherworldly virus is spreading inside your body.");
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
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

        public override void Update(NPC npc, ref int buffIndex)
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