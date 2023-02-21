using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
using static Terraria.ModLoader.ModContent;

namespace JoJoStands.Projectiles
{
    public class MeltYourHeart : ModProjectile
    {
        private int dripTimer;
        private int checkNumber = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.maxPenetrate = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[0] != 0f)
            {
                dripTimer++;
                DrawOriginOffsetY = -4;
                if (Projectile.ai[0] == 4f)     //stuck to the top
                {
                    Projectile.rotation = 0f;
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 22.5f)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                        if (Projectile.frame >= 5)
                        {
                            int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 3f, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[drip].netUpdate = true;
                            Projectile.netUpdate = true;
                            Projectile.frame = 1;
                        }
                    }
                }
                if (Projectile.ai[0] == 3f)     //stuck to the right
                {
                    DrawOffsetX = -8;
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(270f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        Projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                if (Projectile.ai[0] == 2f)     //stuck to the bottom
                {
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(180f);
                }
                if (Projectile.ai[0] == 1f)     //stuck to the left
                {
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(90f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        Projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.frame = 0;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(0, 101) < 20)
                target.AddBuff(BuffID.Confused, 120);
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
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
                Tile tileTarget = Main.tile[(int)(Projectile.Center.X /16f) + Xadd, (int)(Projectile.Center.Y / 16f) + Yadd];
                if (tileTarget.HasTile)
                {
                    Projectile.ai[0] = checkNumber + 1;
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}