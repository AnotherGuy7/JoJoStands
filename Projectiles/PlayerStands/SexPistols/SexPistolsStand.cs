using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SexPistols
{
    public class SexPistolsStand : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/PlayerStands/SexPistols/SexPistol";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 2;
            Projectile.friendly = true;
        }

        private const float BaseKickRadius = 24f;
        private const int BaseKickCooldownTime = 120;
        private const int MaxAmountOfDusts = 40;
        private readonly SoundStyle TinkSound = new SoundStyle(SoundID.Tink.SoundPath)
        {
            Pitch = 5f
        };

        private int kickRestTimer = 0;
        private float kickRadius = 0f;
        private int kickCooldownTime = 0;
        private int scanTimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (kickRestTimer > 0)
                kickRestTimer--;

            kickRadius = (BaseKickRadius * mPlayer.sexPistolsTier) + mPlayer.standRangeBoosts / 2f;
            kickCooldownTime = BaseKickCooldownTime - (15 * mPlayer.sexPistolsTier) - (2 * mPlayer.standSpeedBoosts);
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                Projectile.position = player.Center + mPlayer.sexPistolsOffsets[(int)Projectile.ai[0] - 1];

                float cooldownRestProgress = kickRestTimer / (float)kickCooldownTime;
                int amountOfDusts = MaxAmountOfDusts - (int)(MaxAmountOfDusts * cooldownRestProgress);
                for (int i = 0; i < amountOfDusts; i++)
                {
                    float rotation = MathHelper.ToRadians(((360 / MaxAmountOfDusts) * i) - 90f);        //60 since it's the max amount of dusts that is supposed to circle it
                    Vector2 dustPosition = Projectile.Center + (rotation.ToRotationVector2() * kickRadius);
                    int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID.IchorTorch);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].noLight = true;
                    Vector2 velocity = Projectile.Center + ((rotation + 0.01f).ToRotationVector2() * kickRadius) - dustPosition;
                    velocity.Normalize();
                    velocity *= 2.2f;
                    Main.dust[dustIndex].velocity = velocity;
                    Main.dust[dustIndex].scale = 1f;
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
                        if (otherProjectile.active && otherProjectile.type != Projectile.type && otherProjectile.DamageType == DamageClass.Ranged && otherProjectile.owner == Projectile.owner && Projectile.Distance(otherProjectile.Center) <= kickRadius)
                        {
                            NPC target = null;
                            float closestDistance = 999f;
                            for (int n = 0; n < Main.maxNPCs; n++)
                            {
                                NPC possibleTarget = Main.npc[n];
                                float distance = Vector2.Distance(possibleTarget.Center, Projectile.Center);
                                if (possibleTarget.active && possibleTarget.lifeMax > 5 && !possibleTarget.immortal && !possibleTarget.hide && !possibleTarget.townNPC && distance < closestDistance)
                                {
                                    target = possibleTarget;
                                    closestDistance = distance;
                                }
                            }

                            if (target == null)
                                break;

                            if (!player.HasBuff(ModContent.BuffType<BulletKickFrenzy>()))
                                kickRestTimer += kickCooldownTime;

                            int amountOfKickDusts = 15;
                            for (int i = 0; i < amountOfKickDusts; i++)
                            {
                                float rotation = MathHelper.ToRadians(((360 / 15) * i) - 90f);        //60 since it's the max amount of dusts that is supposed to circle it
                                Vector2 dustPosition = otherProjectile.Center + (rotation.ToRotationVector2() * 4f);
                                int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID.IchorTorch);
                                Main.dust[dustIndex].noGravity = true;
                                Main.dust[dustIndex].noLight = true;
                                Vector2 velocity = otherProjectile.Center + ((rotation + 0.01f).ToRotationVector2() * 4f) - dustPosition;
                                velocity.Normalize();
                                velocity *= 1.6f;
                                Main.dust[dustIndex].velocity = velocity;
                                Main.dust[dustIndex].scale = 1f;
                            }

                            Vector2 redirectionVelocity = target.Center - otherProjectile.Center;
                            redirectionVelocity.Normalize();
                            redirectionVelocity *= 16f;
                            otherProjectile.velocity = redirectionVelocity;
                            otherProjectile.penetrate += 1 + (mPlayer.sexPistolsTier / 2);
                            otherProjectile.GetGlobalProjectile<JoJoGlobalProjectile>().kickedBySexPistols = true;
                            otherProjectile.netUpdate = true;
                            SoundEngine.PlaySound(TinkSound, otherProjectile.Center);
                            if (JoJoStands.SoundsLoaded)
                                SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/SexPistolsCheer_" + Main.rand.Next(1, 3 + 1)));
                            break;
                        }
                    }
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                int direction = 1;
                int ySubtraction = 0;
                if (Projectile.ai[0] > 3)
                {
                    direction = -1;
                    ySubtraction = 15;
                }

                Projectile.position = player.Center + new Vector2(9f * direction, (-5f + (5f * Projectile.ai[0])) - ySubtraction);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}