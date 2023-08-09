using Terraria;
using Terraria.ID;

namespace JoJoStands.Buffs.ItemBuff
{
    public class SphericalVoid : JoJoBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spherical Void");
            // Description.SetDefault("Almost anything that passes through this sphere is disintegrated into nothingness!");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void UpdateBuffOnPlayer(Player player)
        {
            player.shadowDodge = true;
            player.shadowDodgeCount = -100f;
            player.blind = true;
            player.blackout = true;
            player.noFallDmg = true;
            player.lavaImmune = true;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlLeft = false;
            player.controlJump = false;
            player.controlRight = false;
            player.controlDown = false;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlRight = false;
            player.controlUp = false;
            player.controlMount = false;
            player.gravControl = false;
            player.gravControl2 = false;
            player.controlTorch = false;
            player.preventAllItemPickups = true;
            player.velocity.X = -1f;
            player.velocity.Y = -1f;
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
            player.buffImmune[BuffID.OnFire] = true;
            player.AddBuff(BuffID.Obstructed, 2);

            if (player.HasBuff(BuffID.Suffocation))
                player.ClearBuff(BuffID.Suffocation);
            if (player.HasBuff(BuffID.OnFire))
                player.ClearBuff(BuffID.OnFire);
            if (player.HasBuff(BuffID.Burning))
                player.ClearBuff(BuffID.Burning);
            if (player.HasBuff(BuffID.Chilled))
                player.ClearBuff(BuffID.Chilled);
        }
    }
}