using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class LifePunch : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Life Punched");
            Description.SetDefault("Your senses are accellarated and your body can't keep up with you!");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        private bool appliedChange = false;

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.X *= 0.5f;
            player.statDefense -= 5;
            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(player.position, player.width, player.height, 111);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Math.Abs(npc.velocity.X) > 0.5f)
            {
                npc.velocity.X *= 0.7f;
            }
            if (!appliedChange)
            {
                npc.defense -= 5;
                appliedChange = true;
            }
            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 111);
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}