using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Anubis
{
    public class AnubisStandT1 : AnubisStand
    {
        public override int MaxAdaptationStacks => 25;
        protected override string TextureRoot => "JoJoStands/Projectiles/PlayerStands/Anubis/Anubis";
        public override float DashDamageMultiplier => 1.7f;
        public override float BaseMeleeDamageBonusValue => 0.10f;
        public override float BaseMeleeSpeedBonusValue => 0.10f;
        public override float BaseCritBonusValue => 6f;
    }

    public class AnubisStandT2 : AnubisStand
    {
        public override int MaxAdaptationStacks => 50;
        protected override string TextureRoot => "JoJoStands/Projectiles/PlayerStands/Anubis/Anubis";
        public override float DashDamageMultiplier => 1.9f;
        public override float BaseMeleeDamageBonusValue => 0.12f;
        public override float BaseMeleeSpeedBonusValue => 0.20f;
        public override float BaseCritBonusValue => 8f;
    }

    public class AnubisStandT3 : AnubisStand
    {
        public override int MaxAdaptationStacks => 75;
        protected override string TextureRoot => "JoJoStands/Projectiles/PlayerStands/Anubis/Anubis";
        public override float DashDamageMultiplier => 2.1f;
        public override float BaseMeleeDamageBonusValue => 0.14f;
        public override float BaseMeleeSpeedBonusValue => 0.30f;
        public override float BaseCritBonusValue => 10f;
    }

    public class AnubisStandT4 : AnubisStand
    {
        public override int MaxAdaptationStacks => 100;
        protected override string TextureRoot => "JoJoStands/Projectiles/PlayerStands/Anubis/Anubis";
        public override float DashDamageMultiplier => 2.5f;
        public override float BaseMeleeDamageBonusValue => 0.16f;
        public override float BaseMeleeSpeedBonusValue => 0.40f;
        public override float BaseCritBonusValue => 12f;
    }
}