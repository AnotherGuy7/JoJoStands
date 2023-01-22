using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class LifePunch : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Life Punched");
            Description.SetDefault("Your senses are accellarated and your body can't keep up with you!");
            Main.persistentBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.velocity.X *= 0.5f;
            player.statDefense -= 5;
            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(player.position, player.width, player.height, 111);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void OnApply(NPC npc)
        {
            npc.defense -= 5;
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            if (Math.Abs(npc.velocity.X) > 0.5f)
                npc.velocity.X *= 0.7f;

            if (Main.rand.Next(0, 2 + 1) == 0)
            {
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, 111);
                Main.dust[dustIndex].noGravity = true;
            }
        }
    }
}