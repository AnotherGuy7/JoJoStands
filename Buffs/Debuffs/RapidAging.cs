using System;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.Debuffs
{
    public class RapidAging : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapid Aging");
            Description.SetDefault("You can feel your entire life energy leaving.");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        private bool oneTimeEffectsApplied = false;
        private float savedVelocityX = -1f;

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.lifeRegen > 0)
                player.lifeRegen = 0;

            player.lifeRegenTime = 120;
            player.lifeRegen -= 16;
            player.moveSpeed *= 0.8f;
            player.GetDamage(DamageClass.Generic) *= 0.75f;
            player.GetAttackSpeed(DamageClass.Generic) *= 0.5f;
            player.statDefense = (int)(player.statDefense * 0.8f);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            /*Player player = Main.player[npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().standDebuffEffectOwner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();*/
            if (savedVelocityX == -1f)
                savedVelocityX = Math.Abs(npc.velocity.X) / 2f;

            if (!oneTimeEffectsApplied)
            {
                npc.defense = (int)(npc.defense * 0.2f);
                oneTimeEffectsApplied = true;
            }

            npc.lifeRegen = -npc.lifeMax / 16;
            if (Math.Abs(npc.velocity.X) > savedVelocityX)
                npc.velocity.X *= 0.8f;
            /*if (mPlayer.standTier == 3)
                npc.lifeRegen = -350;
            if (mPlayer.standTier == 4)
                npc.lifeRegen = -500;*/
        }
    }
}