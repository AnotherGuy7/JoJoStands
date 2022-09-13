using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond
{
    public class SoftAndWetGoBeyondStand : StandClass
    {

        public override float maxDistance => 98f;
        public override int punchDamage => 116;
        public override int punchTime => 9;
        public override int halfStandHeight => 48;
        public override int altDamage => 320;
        public override int standOffset => 0;
        public override float fistWhoAmI => 0f;
        public override float tierNumber => 5f;
        public override StandType standType => StandType.Ranged;

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


            if (!mPlayer.standAutoMode)
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
                        {
                            Projectile.direction = -1;
                        }
                        Projectile.spriteDirection = Projectile.direction;
                        secondaryAbilityFrames = false;
                    }
                }
                if (Main.mouseRight && Projectile.owner == Main.myPlayer)
                {

                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    if (Projectile.frame == 4 && shootCount <= 0)
                    {

                        shootCount += 38;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;

                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 6f;
                        Vector2 shootPosition = Projectile.position + new Vector2(40f, -10f);
                        int hvb = Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootPosition, shootVel, ModContent.ProjectileType<HighVelocityBubble>(), (int)(altDamage * mPlayer.standDamageBoosts), 8f, Projectile.owner, Projectile.whoAmI); ;
                        shootCount += 1;
                        SoundEngine.PlaySound(SoundID.Item130);
                        Main.projectile[hvb].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
    

                }
                if (SpecialKeyPressed() && Projectile.owner == Main.myPlayer)
                {
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    int dustIndex = Dust.NewDust(player.position, player.width, player.height, DustID.t_Martian, Scale: Main.rand.NextFloat(1f, 2f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= 2f;
                    int expspin = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<ExplosiveSpin>(), (int)(500 * mPlayer.standDamageBoosts), 0f, Projectile.owner, Projectile.whoAmI);
                    SoundEngine.PlaySound(SoundID.Item165);
                    Main.projectile[expspin].netUpdate = true;
                    Projectile.netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(20));


                }
            }
                if (mPlayer.standAutoMode)
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

            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
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
                AnimateStand(animationName, 5, 24, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}