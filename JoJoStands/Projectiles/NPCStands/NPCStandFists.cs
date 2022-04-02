using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles.Minions
{
    public class NPCStandFists : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/Minions/Fists"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 6;
            projectile.alpha = 255;     //completely transparent
        }
    }
}