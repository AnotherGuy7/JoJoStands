using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
 
namespace JoJoStands.Projectiles.Minions
{
    public class NPCStandFists : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/Minions/Fists"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 6;
            Projectile.alpha = 255;     //completely transparent
        }
    }
}