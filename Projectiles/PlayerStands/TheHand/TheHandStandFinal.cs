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
        public override StandAttackType StandType => StandAttackType.Melee;
        public override int PunchDamage => 78;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 7;
        public override int TierNumber => 4;
        public override string PoseSoundName => "NobodyCanFoolMeTwice";
        public override string SpawnSoundName => "The Hand";

        private bool scrapeFrames = false;
        private bool scrapeBarrageFrames = false;
        private int chargeTimer = 0;
        private int specialScrapeTimer = 0;
        private bool scrapeMode = false;
        private int healthAtCharge = 0;
        public static readonly SoundStyle ScrapeSoundEffect = new SoundStyle("JoJoStands/Sounds/GameSounds/BRRR")
        {
            Volume = JoJoStands.ModSoundsVolume
        };

        public override void AI()
        {
            if (scrapeFrames)
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
                    if (scrapeMode)
                        Main.NewText("Scrape Mode: Active");
                    else
                        Main.NewText("Scrape Mode: Disabled");
                }

                if (!scrapeMode)
                {
                    if (scrapeBarrageFrames)
                    {
                        shootCount = 0;
                        scrapeBarrageFrames = false;
                    }
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbility && !scrapeFrames)
                    {
                        Punch();
                    }
                    else
                    {
                        if (player.whoAmI == Main.myPlayer)
                            attackFrames = false;
                    }
                    if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && Projectile.owner == Main.myPlayer)
                    {
                        secondaryAbilityFrames = true;
                        Projectile.netUpdate = true;
                        if (chargeTimer < 150)
                            chargeTimer++;
                    }
                    if (!Main.mouseRight && chargeTimer != 0 && Projectile.owner == Main.myPlayer)
                    {
                        scrapeFrames = true;
                        Projectile.netUpdate = true;
                    }

                    if (!Main.mouseRight && chargeTimer != 0 && scrapeFrames && Projectile.frame == 1 && Projectile.owner == Main.myPlayer)
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
                else
                {
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !secondaryAbility)
                    {
                        if (!player.GetModPlayer<MyPlayer>().canStandBasicAttack)
                        {
                            idleFrames = true;
                            scrapeBarrageFrames = false;
                            attackFrames = false;
                            return;
                        }

                        attackFrames = false;
                        idleFrames = false;
                        scrapeBarrageFrames = true;
                        Projectile.netUpdate = true;

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
                            shootCount += (int)(newPunchTime * 1.4);
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;

                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), (int)(newPunchDamage * 2.5f * overHeavenDamageBoost), PunchKnockback, Projectile.owner, FistWhoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                            SoundStyle theHandScrapeSound = ScrapeSoundEffect;
                            theHandScrapeSound.Pitch = Main.rand.NextFloat(0, 0.8f + 1f);
                            theHandScrapeSound.Volume = JoJoStands.ModSoundsVolume;
                            SoundEngine.PlaySound(theHandScrapeSound, Projectile.Center);
                        }
                        LimitDistance();
                    }
                    else
                    {
                        if (player.whoAmI == Main.myPlayer)
                        {
                            attackFrames = false;
                            scrapeBarrageFrames = false;
                        }
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
                    }
                    if (!Main.mouseRight && specialScrapeTimer != 0)
                    {
                        scrapeFrames = true;
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
                        if (specialScrapeTimer > 60)
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
                                        modifiers.SetCrit();
                                    if (mPlayer.manifestedWillEmblem && crit)
                                        manifestedWillEmblemDamageBoost = 1.5f;
                                    npc.StrikeNPC((int)(210 * (specialScrapeTimer / 30) * mPlayer.standDamageBoosts * manifestedWillEmblemDamageBoost * overHeavenDamageBoost), 0f, player.direction, crit);     //damage goes up at a rate of 210dmg/0.5s
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
                if (!attackFrames)
                {
                    if (!scrapeFrames && !secondaryAbilityFrames && !scrapeBarrageFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                BasicPunchAI();
            }
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
            if (scrapeFrames)
            {
                Texture2D scrapeTrail = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/ScrapeTrail");
                //Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition, new Rectangle(0, 2 - Projectile.frame, scrapeTrail.Width, scrapeTrail.Height / (Projectile.frame + 1)), Color.White);
                int frameHeight = standTexture.Height / 2;
                Main.EntitySpriteDraw(scrapeTrail, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), Color.White, 0f, new Vector2(scrapeTrail.Width / 2f, frameHeight / 2f), 1f, effects, 0);
            }
            if (scrapeBarrageFrames)
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
            writer.Write(scrapeFrames);
            writer.Write(scrapeBarrageFrames);
            writer.Write(scrapeMode);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            scrapeFrames = reader.ReadBoolean();
            scrapeBarrageFrames = reader.ReadBoolean();
            scrapeMode = reader.ReadBoolean();
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
            if (scrapeBarrageFrames)
            {
                idleFrames = false;
                PlayAnimation("ScrapeBarrage");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().posing)
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
            if (animationName == "ScrapeBarrage")
            {
                AnimateStand(animationName, 7, (int)(newPunchTime * 1.3), true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }
    }
}