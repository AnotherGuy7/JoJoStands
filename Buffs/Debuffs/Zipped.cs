using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class Zipped : ModBuff
    {
        private Vector2 savedVelocity = Vector2.Zero;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zipped!");
            Description.SetDefault("A zipper has been placed on your body, blood is spilling quickly!");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            player.lifeRegenTime = 0;
            player.lifeRegen -= 20;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (savedVelocity == Vector2.Zero)
            {
                savedVelocity = npc.velocity * 0.5f;
            }

            if (npc.lifeRegen > 0)
            {
                npc.lifeRegen = 0;
            }
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, DustID.Blood, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f, 0, Scale: 2f);
            npc.lifeRegenExpectedLossPerSecond = 20;
            npc.lifeRegen -= 60;

            if (Math.Abs(npc.velocity.X) > savedVelocity.X)
            {
                npc.velocity.X *= 0.9f;
            }
            if (npc.noGravity)
            {
                if (Math.Abs(npc.velocity.Y) > savedVelocity.Y)
                {
                    npc.velocity.Y *= 0.9f;
                }
            }
        }
    }
}