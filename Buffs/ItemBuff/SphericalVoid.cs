using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SphericalVoid : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spherical Void");
            Description.SetDefault("Almost anything that passes through this sphere is disintegrated into nothingness!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noFallDmg = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.AddBuff(BuffID.Obstructed, 2);
            player.immune = true;
            player.endurance = 1f;
            player.longInvince = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[BuffID.Stoned] = true;
            player.buffImmune[BuffID.Rabies] = true;
            player.buffImmune[BuffID.Webbed] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Ichor] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Silenced] = true;
            if (player.HasBuff(BuffID.Suffocation))
            {
                player.ClearBuff(BuffID.Suffocation);
            }
            if (player.HasBuff(BuffID.OnFire))
            {
                player.ClearBuff(BuffID.OnFire);
            }
            if (player.HasBuff(BuffID.Burning))
            {
                player.ClearBuff(BuffID.Burning);
            }
            if (player.HasBuff(BuffID.Chilled))
            {
                player.ClearBuff(BuffID.Chilled);
            }
        }
    }
}