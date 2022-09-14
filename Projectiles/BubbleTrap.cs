using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class BubbleTrap : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Extras/BubbleTrap"; }
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

        }



        public override void Kill(int timeLeft)
        {
            //Bubble explosion effects
            for (int i = 0; i < 30; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Alpha: 100, Scale: 0.6f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
           

           
        }
    }
}