using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class ControllableNail : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 6;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        private const float MouseDistanceLimit = 12f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.owner == Main.myPlayer && Projectile.ai[0] == 0f)
            {
                float xDist = (float)Main.mouseX + Main.screenPosition.X - Projectile.Center.X;
                float yDist = (float)Main.mouseY + Main.screenPosition.Y - Projectile.Center.Y;
                float mouseDistance = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                Projectile.rotation += (float)Projectile.direction * 0.8f;
                if (player.gravDir == -1f)
                    yDist = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - Projectile.Center.Y;

                if (player.channel)
                {
                    Projectile.rotation += (float)Projectile.direction * 0.8f;

                    if (mouseDistance > MouseDistanceLimit)
                    {
                        Projectile.rotation += (float)Projectile.direction * 0.8f;
                        mouseDistance = MouseDistanceLimit / mouseDistance;
                        xDist *= mouseDistance;
                        yDist *= mouseDistance;
                        Point expectedVelocity = new Point((int)(xDist * 1000f), (int)(yDist * 1000f));
                        Point actualVelocity = new Point((int)(Projectile.velocity.X * 1000f), (int)(Projectile.velocity.Y * 1000f));
                        if (expectedVelocity.X != actualVelocity.X || expectedVelocity.Y != actualVelocity.Y)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity = new Vector2(xDist, yDist);
                    }
                    else
                    {
                        Projectile.rotation += (float)Projectile.direction * 0.8f;
                        Point expectedVelocity = new Point((int)(xDist * 1000f), (int)(yDist * 1000f));
                        Point actualVelocity = new Point((int)(Projectile.velocity.X * 1000f), (int)(Projectile.velocity.Y * 1000f));
                        if (expectedVelocity.X != actualVelocity.X || expectedVelocity.Y != actualVelocity.Y)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity = new Vector2(xDist, yDist);
                    }
                }
                else
                {
                    Projectile.rotation += (float)Projectile.direction * 0.8f;
                    if (Projectile.ai[0] == 0f)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.rotation += (float)Projectile.direction * 0.8f;
                        Projectile.netUpdate = true;

                        if (mouseDistance == 0f)
                        {
                            Projectile.Center = new Vector2(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2));
                            Projectile.rotation += (float)Projectile.direction * 0.8f;
                            xDist = Projectile.position.X + (float)Projectile.width * 0.5f - Projectile.Center.X;
                            yDist = Projectile.position.Y + (float)Projectile.height * 0.5f - Projectile.Center.Y;
                            mouseDistance = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                        }
                        mouseDistance = MouseDistanceLimit / mouseDistance;
                        Projectile.velocity = new Vector2(xDist, yDist) * mouseDistance;
                        if (Projectile.velocity == Vector2.Zero)
                            Projectile.Kill();
                    }
                }
            }
            int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 202, Projectile.velocity.X * -0.3f, Projectile.velocity.Y * -0.3f);
            Main.dust[dustIndex].noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
    }
}