using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class ControllableNail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 202, projectile.velocity.X * -0.5f, projectile.velocity.Y * -0.5f);

            if (Main.myPlayer == projectile.owner && projectile.ai[0] == 0f)
            {
                projectile.rotation += (float)projectile.direction * 0.8f;
                if (Main.player[projectile.owner].channel)
                {
                    projectile.rotation += (float)projectile.direction * 0.8f;
                    float num146 = 12f;
                    Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num147 = (float)Main.mouseX + Main.screenPosition.X - vector10.X;
                    float num148 = (float)Main.mouseY + Main.screenPosition.Y - vector10.Y;
                    if (Main.player[projectile.owner].gravDir == -1f)
                    {
                        num148 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector10.Y;
                        projectile.rotation += (float)projectile.direction * 0.8f;
                    }
                    float num149 = (float)Math.Sqrt((double)(num147 * num147 + num148 * num148));
                    num149 = (float)Math.Sqrt((double)(num147 * num147 + num148 * num148));
                    if (num149 > num146)
                    {
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        num149 = num146 / num149;
                        num147 *= num149;
                        num148 *= num149;
                        int num150 = (int)(num147 * 1000f);
                        int num151 = (int)(projectile.velocity.X * 1000f);
                        int num152 = (int)(num148 * 1000f);
                        int num153 = (int)(projectile.velocity.Y * 1000f);
                        if (num150 != num151 || num152 != num153)
                        {
                            projectile.rotation += (float)projectile.direction * 0.8f;
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num147;
                        projectile.velocity.Y = num148;
                    }
                    else
                    {
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        int num154 = (int)(num147 * 1000f);
                        int num155 = (int)(projectile.velocity.X * 1000f);
                        int num156 = (int)(num148 * 1000f);
                        int num157 = (int)(projectile.velocity.Y * 1000f);
                        if (num154 != num155 || num156 != num157)
                        {
                            projectile.rotation += (float)projectile.direction * 0.8f;
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num147;
                        projectile.velocity.Y = num148;
                    }
                }
                else
                {
                    projectile.rotation += (float)projectile.direction * 0.8f;
                    if (projectile.ai[0] == 0f)
                    {
                        projectile.ai[0] = 1f;
                        projectile.rotation += (float)projectile.direction * 0.8f;
                        projectile.netUpdate = true;
                        float num158 = 12f;
                        Vector2 vector11 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                        float num159 = (float)Main.mouseX + Main.screenPosition.X - vector11.X;
                        float num160 = (float)Main.mouseY + Main.screenPosition.Y - vector11.Y;
                        if (Main.player[projectile.owner].gravDir == -1f)
                        {
                            num160 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector11.Y;
                            projectile.rotation += (float)projectile.direction * 0.8f;
                        }
                        float num161 = (float)Math.Sqrt((double)(num159 * num159 + num160 * num160));
                        if (num161 == 0f)
                        {
                            vector11 = new Vector2(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2), Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2));
                            projectile.rotation += (float)projectile.direction * 0.8f;
                            num159 = projectile.position.X + (float)projectile.width * 0.5f - vector11.X;
                            num160 = projectile.position.Y + (float)projectile.height * 0.5f - vector11.Y;
                            num161 = (float)Math.Sqrt((double)(num159 * num159 + num160 * num160));
                        }
                        num161 = num158 / num161;
                        num159 *= num161;
                        num160 *= num161;
                        projectile.velocity.X = num159;
                        projectile.velocity.Y = num160;
                        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                        {
                            projectile.Kill();
                        }
                    }
                }
			}
        }
    }
}