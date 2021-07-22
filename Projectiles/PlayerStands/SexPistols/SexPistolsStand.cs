using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SexPistols
{
    public class SexPistolsStand : ModProjectile
    {
        public override string Texture => mod.Name + "/Projectiles/PlayerStands/SexPistols/SexPistol";

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 2;
            projectile.friendly = true;
        }

        private const float BaseKickRadius = 16f;
        private const int BaseKickCooldownTime = 120;
        private const int MaxAmountOfDusts = 20;

        public int updateTimer = 0;
        private int kickRestTimer = 0;
        private float kickRadius = 0f;
        private int kickCooldownTime = 0;
        private int scanTimer = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }

            updateTimer++;
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (kickRestTimer > 0)
            {
                kickRestTimer--;
            }
            kickRadius = (BaseKickRadius * mPlayer.sexPistolsTier) + mPlayer.standRangeBoosts / 2f;
            kickCooldownTime = BaseKickCooldownTime - (15 * mPlayer.sexPistolsTier);

            if (!mPlayer.StandAutoMode)
            {
                projectile.position = player.Center + mPlayer.sexPistolsOffsets[(int)projectile.ai[0] - 1];

                float cooldownRestProgress = kickRestTimer / (float)kickCooldownTime;
                int amountOfDusts = MaxAmountOfDusts - (int)(MaxAmountOfDusts * cooldownRestProgress);
                for (int i = 0; i < amountOfDusts; i++)
                {
                    float rotation = MathHelper.ToRadians(((360 / MaxAmountOfDusts) * i) - 90f);        //60 since it's the max amount of dusts that is supposed to circle it
                    Vector2 dustPosition = projectile.Center + (rotation.ToRotationVector2() * kickRadius);
                    int dustIndex = Dust.NewDust(dustPosition, 1, 1, 169);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                }

                if (kickRestTimer > 0)
                    return;

                scanTimer--;
                if (scanTimer <= 0)
                {
                    scanTimer += 2;
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProjectile = Main.projectile[p];
                        if (otherProjectile.active && otherProjectile.type != projectile.type && otherProjectile.owner == projectile.owner && projectile.Distance(otherProjectile.Center) <= kickRadius)
                        {
                            NPC target = null;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC possibleTarget = Main.npc[n];
                                if (possibleTarget.active && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && !possibleTarget.hide && !possibleTarget.townNPC)
                                {
                                    target = possibleTarget;
                                    break;
                                }
                            }

                            if (target == null)
                                break;

                            if (!player.HasBuff(mod.BuffType("BulletKickFrenzy")))
                                kickRestTimer += kickCooldownTime;

                            Vector2 redirectionVelocity = target.Center - projectile.Center;
                            redirectionVelocity.Normalize();
                            redirectionVelocity *= 16f;
                            otherProjectile.velocity = redirectionVelocity;
                            otherProjectile.penetrate += 1 + (mPlayer.sexPistolsTier / 2);
                            otherProjectile.GetGlobalProjectile<JoJoGlobalProjectile>().kickedBySexPistols = true;
                            Main.PlaySound(SoundID.Tink, (int)otherProjectile.Center.X, (int)otherProjectile.Center.Y, 1, 1f, 5f);
                            break;
                        }
                    }
                }
            }

            if (mPlayer.StandAutoMode)
            {
                int direction = 1;
                int ySubtraction = 0;
                if (projectile.ai[0] > 3)
                {
                    direction = -1;
                    ySubtraction = 15;
                }

                projectile.position = player.Center + new Vector2(9f * direction, (-5f + (5f * projectile.ai[0])) - ySubtraction);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}