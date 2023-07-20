using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheHand
{
    public class TheHandStandT2 : StandClass
    {
        public override float MaxDistance => 98f;
        public override float MaxAltDistance => 245f;
        public override int PunchDamage => 34;
        public override int PunchTime => 13;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 7;
        public override int TierNumber => 2;
        public override string PoseSoundName => "NobodyCanFoolMeTwice";
        public override string SpawnSoundName => "The Hand";
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool scraping = false;
        private int chargeTimer = 0;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Charge,
            Scrape,
            Pose
        }

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
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbility && !scraping)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && player.whoAmI == Main.myPlayer)
                {
                    secondaryAbility = true;
                    Projectile.netUpdate = true;
                    if (chargeTimer < 150)
                        chargeTimer++;
                }
                if (!Main.mouseRight && chargeTimer != 0 && Projectile.owner == Main.myPlayer)
                {
                    scraping = true;
                    Projectile.netUpdate = true;
                }
                if (!Main.mouseRight && chargeTimer != 0 && scraping && Projectile.frame == 1 && Projectile.owner == Main.myPlayer)
                {
                    SoundEngine.PlaySound(TheHandStandFinal.ScrapeSoundEffect);
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    distanceToTeleport *= chargeTimer / 60f;
                    player.velocity += distanceToTeleport * 5f;

                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(chargeTimer / 10));       //15s max cooldown
                    chargeTimer = 0;
                }
                if (!attacking)
                {
                    if (!scraping && !secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
            if (scraping)
                currentAnimationState = AnimationState.Scrape;
        }

        public override bool PreDrawExtras()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && JoJoStands.RangeIndicators && chargeTimer != 0)
            {
                Texture2D positionIndicator = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/PositionIndicator");
                Vector2 distanceToTeleport = Vector2.Zero;
                if (Projectile.owner == Main.myPlayer)      //so that other players cursors don't mix in
                    distanceToTeleport = Main.MouseWorld - player.position;
                distanceToTeleport.Normalize();
                distanceToTeleport *= 98f * (chargeTimer / 60f);
                Main.EntitySpriteDraw(positionIndicator, (player.Center + distanceToTeleport) - Main.screenPosition, null, Color.White * JoJoStands.RangeIndicatorAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            if (scraping)
            {
                Texture2D scrapeTrail = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/ScrapeTrail");
                //Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition, new Rectangle(0, 2 - Projectile.frame, scrapeTrail.Width, scrapeTrail.Height / (Projectile.frame + 1)), Color.White);
                int frameHeight = standTexture.Height / 2;
                Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), Color.White, 0f, new Vector2(scrapeTrail.Width / 2f, frameHeight / 2f), 1f, effects, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(scraping);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            scraping = reader.ReadBoolean();
        }

        private bool resetFrame = false;

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Charge)
                PlayAnimation("Charge");
            else if (currentAnimationState == AnimationState.Scrape)
                PlayAnimation("Scrape");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Scrape")
            {
                scraping = false;
                currentAnimationState = AnimationState.Idle;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TheHand/TheHand_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Charge")
                AnimateStand(animationName, 4, 15, true);
            else if (animationName == "Scrape")
                AnimateStand(animationName, 2, 10, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }
    }
}