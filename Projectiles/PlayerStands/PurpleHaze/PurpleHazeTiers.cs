using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.PurpleHaze
{
    public class PurpleHazeStandT1 : PurpleHazeStand
    {
        public override int TierNumber => 1;
        public override int PunchDamage => 9;
        public override int PunchTime => 16;
        public override float MaxDistance => 160f;

        protected override bool CanThrowCapsule => false;
        protected override bool CanReleaseVirus => false;
        protected override bool CanInfectOnHit => false;
        protected override bool CanAOEBurst => false;
        protected override bool CanRampage => false;

        protected override bool HasJitter => true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 56;
        }
    }

    public class PurpleHazeStandT2 : PurpleHazeStand
    {
        public override int TierNumber => 2;
        public override int PunchDamage => 29;
        public override int PunchTime => 15;
        public override float MaxDistance => 160f;

        protected override bool CanThrowCapsule => true;
        protected override bool CanReleaseVirus => true;
        protected override bool CanInfectOnHit => false;
        protected override bool CanAOEBurst => false;
        protected override bool CanRampage => false;
        protected override bool HasJitter => true;
        protected override bool HasReducedJitter => true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 30;
            Projectile.height = 60;
        }
    }

    public class PurpleHazeStandT3 : PurpleHazeStand
    {
        public override int TierNumber => 3;
        public override int PunchDamage => 49;
        public override int PunchTime => 13;
        public override float MaxDistance => 160f;

        protected override bool CanThrowCapsule => true;
        protected override bool CanReleaseVirus => true;
        protected override bool CanInfectOnHit => true;
        protected override bool CanAOEBurst => true;
        protected override bool CanRampage => true;

        protected override bool HasJitter => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 64;
        }
    }

    public class PurpleHazeStandFinal : PurpleHazeStand
    {
        public override int TierNumber => 4;
        public override int PunchDamage => 65;
        public override int PunchTime => 13;
        public override float MaxDistance => 160f;

        protected override bool CanThrowCapsule => true;
        protected override bool CanReleaseVirus => true;
        protected override bool CanInfectOnHit => true;
        protected override bool CanAOEBurst => true;
        protected override bool CanRampage => true;

        // No jitter
        protected override bool HasJitter => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 70;
        }
    }
}