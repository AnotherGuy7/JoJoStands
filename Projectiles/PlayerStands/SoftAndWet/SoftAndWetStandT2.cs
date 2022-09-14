using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWet
{
    public class SoftAndWetStandT2 : StandClass
    {

        public override float maxDistance => 98f;
        public override int punchDamage => 38;
        public override int punchTime => 10;
        public override int halfStandHeight => 48;
        public override int altDamage => ((int)(tierNumber * 15));
        public override int standOffset => 0;
        public override float fistWhoAmI => 0f;
        public override float tierNumber => 2f;

        public bool bubbleBarrier = false;
        public override StandType standType => StandType.Melee;

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
                secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<PlunderBubble>()] != 0;
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
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
                    StayBehindWithAbility();
                }
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 48;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    SoundEngine.PlaySound(SoundID.SplashWeak);
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<PlunderBubble>(), (int)(altDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, Projectile.whoAmI); ;
                    shootCount += 1;
                    Main.projectile[proj].netUpdate = true;
                    Projectile.netUpdate = true;
                }
                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                    {
                    player.AddBuff(ModContent.BuffType<BarrierBuff>(), 600);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(22));
                    Vector2 playerFollow = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, playerFollow,ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
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
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWet/SoftAndWet_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}