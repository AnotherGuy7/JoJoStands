using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheHand
{
    public class TheHandStandFinal : StandClass
    {
        public override float MaxDistance => 98f;
        public override float MaxAltDistance => 490f;
        public override int PunchDamage => 78;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 7;
        public override int TierNumber => 4;
        public override string PoseSoundName => "NobodyCanFoolMeTwice";
        public override string SpawnSoundName => "The Hand";
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool scraping = false;
        private bool scrapeBarraging = false;
        private int chargeTimer = 0;
        private int specialScrapeTimer = 0;
        private bool scrapeMode = false;
        public static readonly SoundStyle ScrapeSoundEffect = new SoundStyle("JoJoStands/Sounds/GameSounds/BRRR")
        {
            Volume = JoJoStands.ModSoundsVolume
        };

        public new enum AnimationState
        {
            Idle,
            Attack,
            Charge,
            Scrape,
            ScrapeBarrage,
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

            float overHeavenDamageBoost = 1f;
            if (mPlayer.overHeaven)
                overHeavenDamageBoost = 2f;

            Rectangle rectangle = Rectangle.Empty;
            if (Projectile.owner == player.whoAmI)
                rectangle = new Rectangle((int)(Main.MouseWorld.X - 10), (int)(Main.MouseWorld.Y - 10), 20, 20);

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (SpecialKeyPressed(false))
                {
                    scrapeMode = !scrapeMode;
                    shootCount = 0;
                    scrapeBarraging = false;
                    if (scrapeMode)
                        Main.NewText("Scrape Mode: Active");
                    else
                        Main.NewText("Scrape Mode: Disabled");
                }

                if (!scrapeMode)
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (Main.mouseLeft && !secondaryAbility && !scraping)
                        {
                            currentAnimationState = AnimationState.Attack;
                            Punch();
                        }
                        else
                        {
                            attacking = false;
                            currentAnimationState = AnimationState.Idle;
                        }

                        if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            secondaryAbility = true;
                            currentAnimationState = AnimationState.Charge;
                            Projectile.netUpdate = true;
                            if (chargeTimer < 150)
                                chargeTimer++;
                        }
                        if (!Main.mouseRight)
                        {
                            if (chargeTimer != 0)
                            {
                                scraping = true;
                                currentAnimationState = AnimationState.Scrape;
                                Projectile.netUpdate = true;
                                if (Projectile.frame == 1)
                                {
                                    SoundEngine.PlaySound(ScrapeSoundEffect);
                                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                                    distanceToTeleport.Normalize();
                                    distanceToTeleport *= chargeTimer / 30f;
                                    player.velocity += distanceToTeleport * 6f;
                                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(chargeTimer / 24));       //10s max cooldown
                                    chargeTimer = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        if (Main.mouseLeft && !secondaryAbility)
                        {
                            if (!player.GetModPlayer<MyPlayer>().canStandBasicAttack)
                            {
                                scrapeBarraging = false;
                                currentAnimationState = AnimationState.Idle;
                                return;
                            }

                            scrapeBarraging = true;
                            currentAnimationState = AnimationState.ScrapeBarrage;
                            float rotaY = Main.MouseWorld.Y - Projectile.Center.Y;
                            Projectile.rotation = MathHelper.ToRadians((rotaY * Projectile.spriteDirection) / 6f);

                            if (mouseX > player.position.X)
                                player.direction = 1;
                            else
                                player.direction = -1;

                            Vector2 velocityAddition = Main.MouseWorld - Projectile.position;
                            velocityAddition.Normalize();
                            velocityAddition *= 5f;
                            float mouseDistance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
                            if (mouseDistance > 40f)
                                Projectile.velocity = player.velocity + velocityAddition;
                            else
                                Projectile.velocity = Vector2.Zero;

                            if (shootCount <= 0 && (Projectile.frame == 1 || Projectile.frame == 4))
                            {
                                shootCount += (int)(newPunchTime * 1.2);
                                Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                                shootVel.Normalize();
                                shootVel *= ProjectileSpeed;

                                int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 2.5f * overHeavenDamageBoost), PunchKnockback, Projectile.owner, FistWhoAmI);
                                Main.projectile[projIndex].netUpdate = true;
                                SoundStyle theHandScrapeSound = ScrapeSoundEffect;
                                theHandScrapeSound.Pitch = Main.rand.NextFloat(0, 0.6f + 1f);
                                theHandScrapeSound.Volume = JoJoStands.ModSoundsVolume;
                                SoundEngine.PlaySound(theHandScrapeSound, Projectile.Center);
                            }
                            Projectile.netUpdate = true;
                            LimitDistance();
                        }
                        else
                        {
                            currentAnimationState = AnimationState.Idle;
                            scrapeBarraging = false;
                        }
                        if (Main.mouseRight && !playerHasAbilityCooldown)
                        {
                            if (mPlayer.playerJustHit)
                            {
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(3));
                                specialScrapeTimer = 0;
                            }

                            specialScrapeTimer++;
                            for (int i = 0; i < Main.maxNPCs; i++)
                            {
                                NPC npc = Main.npc[i];
                                if (!npc.active)
                                    continue;

                                if (npc.Hitbox.Intersects(rectangle) && Vector2.Distance(npc.Center, Projectile.Center) <= 250f && !npc.immortal && !npc.hide && !npc.townNPC)
                                    npc.GetGlobalNPC<NPCs.JoJoGlobalNPC>().highlightedByTheHandMarker = true;
                            }
                            Projectile.netUpdate = true;
                        }
                        if (!Main.mouseRight && specialScrapeTimer != 0)
                        {
                            scraping = true;
                            currentAnimationState = AnimationState.Scrape;
                            if (specialScrapeTimer <= 60)
                            {
                                SoundEngine.PlaySound(ScrapeSoundEffect);
                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    NPC npc = Main.npc[i];
                                    if (!npc.active)
                                        continue;

                                    if (npc.Hitbox.Intersects(rectangle) && Vector2.Distance(npc.Center, Projectile.Center) <= 250f && !npc.immortal && !npc.hide && !npc.townNPC)
                                    {
                                        Vector2 difference = player.position - npc.position;
                                        npc.position = player.Center + (-difference / 2f);
                                    }
                                }
                                for (int p = 0; p < Main.maxPlayers; p++)
                                {
                                    Player otherPlayer = Main.player[p];
                                    if (otherPlayer.active)
                                    {
                                        if (otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), Projectile.Center, Main.MouseWorld))
                                        {
                                            Vector2 difference = player.position - otherPlayer.position;
                                            otherPlayer.position = player.Center + (-difference / 2f);
                                        }
                                    }
                                }
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                            }
                            else
                            {
                                SoundEngine.PlaySound(ScrapeSoundEffect);
                                for (int i = 0; i < Main.maxNPCs; i++)
                                {
                                    NPC npc = Main.npc[i];
                                    if (!npc.active)
                                        continue;

                                    if (npc.Hitbox.Intersects(rectangle) && Vector2.Distance(npc.Center, Projectile.Center) <= 250f && !npc.immortal && !npc.hide && !npc.townNPC)
                                    {
                                        bool crit = false;
                                        float manifestedWillEmblemDamageBoost = 1f;
                                        if (Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                                            crit = true;
                                        if (mPlayer.manifestedWillEmblem && crit)
                                            manifestedWillEmblemDamageBoost = 1.5f;
                                        NPC.HitInfo hitInfo = new NPC.HitInfo()
                                        {
                                            Damage = (int)(210 * (specialScrapeTimer / 30) * mPlayer.standDamageBoosts * manifestedWillEmblemDamageBoost * overHeavenDamageBoost),
                                            HitDirection = player.direction,
                                            Crit = crit
                                        };
                                        npc.StrikeNPC(hitInfo);     //damage goes up at a rate of 210dmg/0.5s
                                        npc.AddBuff(ModContent.BuffType<MissingOrgans>(), 12 * 60);
                                    }
                                }
                                for (int p = 0; p < Main.maxPlayers; p++)
                                {
                                    Player otherPlayer = Main.player[p];
                                    if (otherPlayer.active)
                                    {
                                        if (otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), Projectile.Center, Main.MouseWorld))
                                        {
                                            otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " was scraped out of existence by " + player.name + "."), (int)(60 * (specialScrapeTimer / 60) * mPlayer.standDamageBoosts), 1);
                                            otherPlayer.AddBuff(ModContent.BuffType<MissingOrgans>(), 12 * 60);
                                        }
                                    }
                                }
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(12));
                            }
                            specialScrapeTimer = 0;
                        }
                    }
                }
                if (!attacking)
                {
                    if (!scraping && !secondaryAbility && !scrapeBarraging)
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
                if (Projectile.owner == Main.myPlayer)
                    distanceToTeleport = Main.MouseWorld - player.position;
                distanceToTeleport.Normalize();
                distanceToTeleport *= 98f * (chargeTimer / 30f);
                Main.EntitySpriteDraw(positionIndicator, (player.Center + distanceToTeleport) - Main.screenPosition, null, Color.White * JoJoStands.RangeIndicatorAlpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            if (scraping)
            {
                Texture2D scrapeTrail = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/ScrapeTrail");
                //Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition, new Rectangle(0, 2 - Projectile.frame, scrapeTrail.Width, scrapeTrail.Height / (Projectile.frame + 1)), Color.White);
                int frameHeight = standTexture.Height / 2;
                Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), Color.White, 0f, new Vector2(scrapeTrail.Width / 2f, frameHeight / 2f), 1f, effects, 0);
            }
            if (scrapeBarraging)
            {
                Texture2D scrapeTrail = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TheHand/ScrapeBarrage_Scrape");
                //Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition, new Rectangle(0, 2 - Projectile.frame, scrapeTrail.Width, scrapeTrail.Height / (Projectile.frame + 1)), Color.White);
                int frameHeight = standTexture.Height / 7;
                Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), Color.White, 0f, new Vector2(scrapeTrail.Width / 2f, frameHeight / 2f), 1f, effects, 0);
            }
            return true;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(scraping);
            writer.Write(scrapeBarraging);
            writer.Write(scrapeMode);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            scraping = reader.ReadBoolean();
            scrapeBarraging = reader.ReadBoolean();
            scrapeMode = reader.ReadBoolean();
        }

        public override byte SendAnimationState() => (byte)currentAnimationState;
        public override void ReceiveAnimationState(byte state) => currentAnimationState = (AnimationState)state;

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
            else if (currentAnimationState == AnimationState.ScrapeBarrage)
                PlayAnimation("ScrapeBarrage");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Scrape")
            {
                scraping = false;
                secondaryAbility = false;
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
            else if (animationName == "ScrapeBarrage")
                AnimateStand(animationName, 7, (int)(newPunchTime * 1.2), true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 12, true);
        }
    }
}