using JoJoStands.NPCs;
using System;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.Debuffs
{
    public class MissingOrgans : JoJoBuff
    {
        private float savedVelocityX = -1f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Missing Organs!");
            // Description.SetDefault("Parts of your body have been scraped away!");
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 0;
            player.lifeRegen -= 30;
            player.moveSpeed *= 0.6f;
            Dust.NewDust(player.position, player.width, player.height, DustID.Blood, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
        }

        public override void OnApply(NPC npc)
        {
            savedVelocityX = Math.Abs(npc.velocity.X) / 2f;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            MyPlayer mPlayer = Main.player[npc.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner].GetModPlayer<MyPlayer>();
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.lifeRegenExpectedLossPerSecond = 10 + (5 * mPlayer.standTier);
            npc.lifeRegen -= 20 + (10 * mPlayer.standTier);
            if (Math.Abs(npc.velocity.X) > savedVelocityX)
                npc.velocity.X *= 0.9f;
        }
    }
}