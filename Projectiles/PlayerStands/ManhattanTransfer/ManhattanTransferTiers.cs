using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.ManhattanTransfer
{
    public class ManhattanTransferStandT1 : ManhattanTransferStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";
        protected override string DeflectTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";

        protected override float TierRange => 150f;
        protected override bool CanLockTarget => false;
        protected override bool CanDeflect => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 38;
        }
    }

    public class ManhattanTransferStandT2 : ManhattanTransferStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";
        protected override string DeflectTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";

        protected override float TierRange => 220f;
        protected override bool CanLockTarget => true;
        protected override bool CanDeflect => false;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 44;
        }
    }

    public class ManhattanTransferStandT3 : ManhattanTransferStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";
        protected override string DeflectTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_DeflectIdle";

        protected override float TierRange => 280f;
        protected override bool CanLockTarget => true;
        protected override bool CanDeflect => true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 50;
        }
    }

    public class ManhattanTransferStandFinal : ManhattanTransferStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_Idle";
        protected override string DeflectTexture => Mod.Name + "/Projectiles/PlayerStands/ManhattanTransfer/ManhattanTransfer_DeflectIdle";

        protected override float TierRange => 380f;
        protected override bool CanLockTarget => true;
        protected override bool CanDeflect => true;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 40;
            Projectile.height = 56;
        }
    }
}