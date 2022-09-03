using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JoJoStands.Buffs.Debuffs;

namespace JoJoStands.Buffs.EffectBuff
{
    public class Restoration : ModBuff
    {
        public override string Texture { get { return "Terraria/Images/Buff_" + BuffID.Regeneration; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Restoration");
            Description.SetDefault("Your wounds are being healed right before your eyes!");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 20;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            Dust.NewDust(player.position, player.width, player.height, 169, player.velocity.X * -0.5f, player.velocity.Y * -0.5f);
            if (player.HasBuff(ModContent.BuffType<MissingOrgans>()))
                player.ClearBuff(buffIndex);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {

            npc.lifeRegen += 20;
            Dust.NewDust(npc.position + npc.velocity, npc.width, npc.height, 169, npc.velocity.X * -0.5f, npc.velocity.Y * -0.5f);
            if (npc.HasBuff(ModContent.BuffType<MissingOrgans>()))
                npc.DelBuff(buffIndex);
        }
    }
}