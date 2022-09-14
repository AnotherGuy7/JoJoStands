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
        public override StandAttackType StandType => StandAttackType.Melee;
        public override int PunchTime => 12;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 7;
        public override int TierNumber => 2;
        public override string PoseSoundName => "NobodyCanFoolMeTwice";
        public override string SpawnSoundName => "The Hand";

        private bool scrapeFrames = false;
        private int chargeTimer = 0;

        public override void AI()
        {
            if (scrapeFrames)       //seems to not actually do this in SelectAnim
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
            }
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
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbility && !scrapeFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && player.whoAmI == Main.myPlayer)
                {
                    secondaryAbilityFrames = true;
                    if (chargeTimer < 150f)
                    {
                        chargeTimer++;
                    }
                }
                if (!Main.mouseRight && chargeTimer != 0 && Projectile.owner == Main.myPlayer)
                {
                    scrapeFrames = true;
                }
                if (!Main.mouseRight && chargeTimer != 0 && Projectile.owner == Main.myPlayer && scrapeFrames && Projectile.frame == 1)
                {
                    SoundEngine.PlaySound(new SoundStyle("JoJoStands/Sounds/GameSounds/BRRR"));
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    /*distanceToTeleport *= 98f * (chargeTimer / 60f);
                    player.position = player.Center + distanceToTeleport;*/
                    distanceToTeleport *= chargeTimer / 60f;
                    player.velocity += distanceToTeleport * 5f;

                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(chargeTimer / 10));       //15s max cooldown
                    chargeTimer = 0;
                }
                /*if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()
                {
                    chargingScrape = true;
                    secondaryAbilityFrames = true;
                }
                if (!Main.mouseRight && chargingScrape)
                {
                    chargingScrape = false;
                    scrapeFrames = true;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<TheHandMouse>(), 0, 0f, Main.myPlayer, Projectile.whoAmI);
                }
                if (Projectile.ai[0] == 1f)     //teleport in that direction
                {
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    distanceToTeleport *= 98f;
                    player.position = player.Center + distanceToTeleport;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                    Projectile.ai[0] = 0f;
                }
                if (Projectile.ai[0] == 2f)     //pull the enemy
                {
                    Projectile.ai[0] = 0f;
                }
                if (Projectile.ai[0] == 3f)     //break tiles
                {
                    Projectile.ai[0] = 0f;
                }*/
                if (!attackFrames)
                {
                    if (!scrapeFrames && !secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override bool PreDrawExtras()
        {
            Player player = Main.player[Projectile.owner];
            if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && MyPlayer.RangeIndicators && chargeTimer != 0)
            {
                Texture2D positionIndicator = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/PositionIndicator");
                Vector2 distanceToTeleport = Vector2.Zero;
                if (Projectile.owner == Main.myPlayer)      //so that other players cursors don't mix in
                    distanceToTeleport = Main.MouseWorld - player.position;
                distanceToTeleport.Normalize();
                distanceToTeleport *= 98f * (chargeTimer / 60f);
                Main.EntitySpriteDraw(positionIndicator, (player.Center + distanceToTeleport) - Main.screenPosition, null, Color.White * MyPlayer.RangeIndicatorAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            if (scrapeFrames)
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
            writer.Write(scrapeFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            scrapeFrames = reader.ReadBoolean();
        }

        private bool resetFrame = false;

        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Charge");
            }
            if (scrapeFrames)
            {
                if (!resetFrame)
                {
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    resetFrame = true;
                }
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Scrape");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                scrapeFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void AnimationCompleted(string animationName)
        {
            if (resetFrame && animationName == "Scrape")
            {
                idleFrames = true;
                scrapeFrames = false;
                resetFrame = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TheHand/TheHand_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Charge")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Scrape")
            {
                AnimateStand(animationName, 2, 10, false);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }
    }
}