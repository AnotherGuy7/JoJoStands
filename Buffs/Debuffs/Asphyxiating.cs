using JoJoStands.NPCs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Asphyxiating : ModBuff
    {
        private float savedVelocityX = -1f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asphyxiating");
            Description.SetDefault("The air has been plundered from your lungs...");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 0;
            player.lifeRegen -= 40;
            player.moveSpeed *= 0.7f;
            Dust.NewDust(player.position, player.width, player.height, DustID.Cloud, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            MyPlayer mPlayer = Main.player[npc.GetGlobalNPC<JoJoGlobalNPC>().standDebuffEffectOwner].GetModPlayer<MyPlayer>();
            if (savedVelocityX == -1f)
                savedVelocityX = Math.Abs(npc.velocity.X) / 2f;
            if (npc.lifeRegen > 0)
                npc.lifeRegen = 0;

            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Cloud, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            npc.lifeRegenExpectedLossPerSecond = 20;
            npc.lifeRegen -= 40;
            if (Math.Abs(npc.velocity.X) > savedVelocityX)
                npc.velocity.X *= 0.9f;
        }
    }
}