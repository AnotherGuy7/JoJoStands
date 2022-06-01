using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class Sunburn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunburn");
            Description.SetDefault("You're burning in the sunlight!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 60;
            player.lifeRegen -= 60;
            player.moveSpeed *= 0.8f;

            if (Main.rand.Next(0, 2) == 0)
                Dust.NewDust(player.position, player.width, player.height, DustID.IchorTorch, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            npc.lifeRegenExpectedLossPerSecond = 30;
            npc.lifeRegen -= 60;     //losing 30 health
            if (Main.rand.Next(0, 2) == 0)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.IchorTorch, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
        }
    }
}