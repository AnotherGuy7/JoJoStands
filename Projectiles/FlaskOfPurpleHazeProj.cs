using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class FlaskOfPurpleHazeProj : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Items/FlaskOfPurpleHaze"; }
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 10 * 60;
            Projectile.penetrate = 1;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(Terraria.DataStructures.IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.ai[1] = owner.GetTotalDamage(DamageClass.Magic).Multiplicative;
        }

        public override void AI()
        {
            Projectile.rotation += 0.15f;

            Projectile.velocity.Y += 0.24f;

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    ModContent.DustType<Dusts.GratefulDeadCloud>());
                d.noGravity = true;
                d.scale = 0.6f;
                d.velocity *= 0.3f;
            }
        }

        private void SpawnCloud()
        {
            if (Main.netMode != Terraria.ID.NetmodeID.MultiplayerClient)
            {
                int cloudIndex = Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<HazeVirusCloudFlask>(),
                    0,
                    0f,
                    Projectile.owner);

                if (cloudIndex >= 0 && cloudIndex < Main.maxProjectiles)
                    Main.projectile[cloudIndex].ai[0] = Projectile.ai[1];
            }
        }

        public override void OnKill(int timeLeft)
        {
            SpawnCloud();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }
    }
}