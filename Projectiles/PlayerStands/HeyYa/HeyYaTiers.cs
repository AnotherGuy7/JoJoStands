using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace JoJoStands.Projectiles.PlayerStands.HeyYa
{
    public class HeyYaStandT1 : HeyYaStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/HeyYa/HeyYa_Idle";
        protected override int IdleFrameCount => 4;
        protected override int Tier => 0;
        public static readonly Color HeyYaTierColor = new Color(255, 165, 50);

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 28;
            Projectile.height = 38;
        }
    }

    public class HeyYaStandT2 : HeyYaStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/HeyYa/HeyYa_Idle";
        protected override int IdleFrameCount => 4;
        protected override int Tier => 1;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 30;
            Projectile.height = 42;
        }
    }

    public class HeyYaStandT3 : HeyYaStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/HeyYa/HeyYa_Idle";
        protected override int IdleFrameCount => 4;
        protected override int Tier => 2;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 32;
            Projectile.height = 46;
        }
    }

    public class HeyYaStandT4 : HeyYaStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/HeyYa/HeyYa_Idle";
        protected override int IdleFrameCount => 4;
        protected override int Tier => 3;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 36;
            Projectile.height = 50;
        }
    }

    public class HeyYaStandT5 : HeyYaStand
    {
        protected override string IdleTexture => Mod.Name + "/Projectiles/PlayerStands/HeyYa/HeyYa_Idle";
        protected override int IdleFrameCount => 4;
        protected override int Tier => 4;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 40;
            Projectile.height = 56;
        }
    }
}