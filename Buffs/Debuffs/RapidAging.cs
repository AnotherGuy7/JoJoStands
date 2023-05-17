using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class RapidAging : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rapid Aging");
            // Description.SetDefault("You can feel your entire life energy leaving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        private float savedVelocityX = -1f;

        public override void UpdateBuffOnPlayer(Player player)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 120;
            player.lifeRegen -= 16;
            player.moveSpeed *= 0.8f;
            player.GetDamage(DamageClass.Generic) *= 0.75f;
            player.GetAttackSpeed(DamageClass.Generic) *= 0.5f;
            player.statDefense *= 0.8f;
        }

        public override void OnApply(NPC npc)
        {
            savedVelocityX = Math.Abs(npc.velocity.X) / 2f;
            npc.defense = (int)(npc.defense * 0.2f);
        }

        public override void UpdateBuffOnNPC(NPC npc)
        {
            npc.lifeRegen = -npc.lifeMax / 16;
            if (Math.Abs(npc.velocity.X) > savedVelocityX)
                npc.velocity.X *= 0.8f;
        }
    }
}