using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.ItemBuff;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond
{
    public class SoftAndWetGoBeyondStand : StandClass
    {
        public override int PunchDamage => 109;
        public override int PunchTime => 9;
        public override int HalfStandHeight => 38;
        public override int AltDamage => 320;
        public override int StandOffset => 54;
        public override int FistWhoAmI => 0;
        public override int TierNumber => 5;
        public override StandAttackType StandType => StandAttackType.Ranged;
        private int highVelocityBubbleChargeUpTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;


            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbilityFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                    {
                        StayBehind();
                        Projectile.direction = Projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        Projectile.direction = 1;
                        if (Main.MouseWorld.X < Projectile.position.X)
                            Projectile.direction = -1;
                        Projectile.spriteDirection = Projectile.direction;
                        secondaryAbilityFrames = false;
                    }
                }
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    highVelocityBubbleChargeUpTimer++;
                    if (highVelocityBubbleChargeUpTimer >= 60)
                    {
                        shootCount += 45;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= 6f;
                        Vector2 shootPosition = Projectile.position + new Vector2(40f * Projectile.direction, -10f);
                        int hvb = Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootPosition, shootVel, ModContent.ProjectileType<HighVelocityBubble>(), (int)(AltDamage * mPlayer.standDamageBoosts), 8f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[hvb].netUpdate = true;
                        Projectile.netUpdate = true;
                        highVelocityBubbleChargeUpTimer = 0;
                        SoundEngine.PlaySound(SoundID.Item130);
                    }
                }
                if (highVelocityBubbleChargeUpTimer > 0 && !Main.mouseRight)
                    highVelocityBubbleChargeUpTimer = 0;
                if (SpecialKeyPressed() && Projectile.owner == Main.myPlayer)
                {
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= 2f;
                    int expspin = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<ExplosiveSpin>(), (int)(500 * mPlayer.standDamageBoosts), 0f, Projectile.owner, Projectile.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item165);
                    Main.projectile[expspin].netUpdate = true;
                    Projectile.netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(20));
                    int dustIndex = Dust.NewDust(player.position, player.width, player.height, DustID.t_Martian, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                    Main.dust[dustIndex].noGravity = true;

                }
                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                {
                    player.AddBuff(ModContent.BuffType<BarrierBuff>(), 600);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(22));
                    Vector2 playerFollow = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, playerFollow, ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
        }

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                PlayAnimation("Idle");
            }

            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWetGoBeyond/SoftAndWetGoBeyond_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 5, 6, true, 0, 3);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}