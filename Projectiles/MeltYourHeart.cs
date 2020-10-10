using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class MeltYourHeart : ModProjectile
    {
        private int dripTimer;
        private int checkNumber = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.timeLeft = 1800;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.maxPenetrate = -1;
        }

        public override void AI()
        {
            if (projectile.ai[0] != 0f)
            {
                dripTimer++;
                drawOriginOffsetY = -4;
                if (projectile.ai[0] == 4f)     //stuck to the top
                {
                    projectile.rotation = 0f;
                    projectile.frameCounter++;
                    if (projectile.frameCounter >= 22.5f)
                    {
                        projectile.frame++;
                        projectile.frameCounter = 0;
                        if (projectile.frame >= 5)
                        {
                            int drip = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 3f, 0f, 6f, mod.ProjectileType("MeltYourHeartDrip"), projectile.damage, 2f, Main.myPlayer, projectile.whoAmI);
                            Main.projectile[drip].netUpdate = true;
                            projectile.netUpdate = true;
                            projectile.frame = 1;
                        }
                    }
                }
                if (projectile.ai[0] == 3f)     //stuck to the right
                {
                    drawOffsetX = -8;
                    projectile.frame = 1;
                    projectile.rotation = MathHelper.ToRadians(270f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 6f, mod.ProjectileType("MeltYourHeartDrip"), projectile.damage, 2f, Main.myPlayer, projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                if (projectile.ai[0] == 2f)     //stuck to the bottom
                {
                    projectile.frame = 1;
                    projectile.rotation = MathHelper.ToRadians(180f);
                }
                if (projectile.ai[0] == 1f)     //stuck to the left
                {
                    projectile.frame = 1;
                    projectile.rotation = MathHelper.ToRadians(90f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 6f, mod.ProjectileType("MeltYourHeartDrip"), projectile.damage, 2f, Main.myPlayer, projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                projectile.velocity = Vector2.Zero;
            }
            else
            {
                projectile.frame = 0;
                projectile.rotation = projectile.velocity.ToRotation();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 101) < 20)
                target.AddBuff(BuffID.Confused, 120);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 4; i++)
            {
                checkNumber = i;
                int Xadd = 0;
                int Yadd = 0;
                if (checkNumber == 0)
                {
                    Xadd = 1;
                    Yadd = 0;
                }
                if (checkNumber == 1)
                {
                    Xadd = 0;
                    Yadd = 1;
                }
                if (checkNumber == 2)
                {
                    Xadd = -1;
                    Yadd = 0;
                }
                if (checkNumber == 3)
                {
                    Xadd = 0;
                    Yadd = -1;
                }
                Tile tileTarget = Main.tile[(int)(projectile.Center.X /16f) + Xadd, (int)(projectile.Center.Y / 16f) + Yadd];
                if (tileTarget.active())
                {
                    projectile.ai[0] = checkNumber + 1;
                    projectile.frame = 0;
                }
            }
            projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}