using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheHand
{
    public class TheHandStandFinal : StandClass
    {
        public override float maxDistance => 98f;
        public override float maxAltDistance => 490f;
        public override int standType => 1;
        public override int punchDamage => 78;
        public override int punchTime => 10;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 7f;
		public override string poseSoundName => "NobodyCanFoolMeTwice";
        public override string spawnSoundName => "The Hand";

        private int updateTimer = 0;
        private bool scrapeFrames = false;
        private int chargeTimer = 0;
        private int specialTimer = 0;

        public override void AI()
        {
            if (scrapeFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
            }
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && projectile.owner == Main.myPlayer)
                {
                    secondaryAbilityFrames = true;
                    if (chargeTimer < 150f)
                    {
                        chargeTimer++;
                    }
                }
                if (!Main.mouseRight && chargeTimer != 0 && projectile.owner == Main.myPlayer)
                {
                    scrapeFrames = true;
                }
                if (!Main.mouseRight && chargeTimer != 0 && projectile.owner == Main.myPlayer && scrapeFrames && projectile.frame == 1)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BRRR"));
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    distanceToTeleport *= chargeTimer / 30f;
                    player.velocity += distanceToTeleport * 5f;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(chargeTimer / 30));       //5s max cooldown
                    chargeTimer = 0;
                }
                if (SpecialKeyCurrent() && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                {
                    specialTimer++;
                    secondaryAbilityFrames = true;

                }
                if (!SpecialKeyCurrent() && specialTimer != 0)
                {
                    scrapeFrames = true;
                    float mouseDistance = Vector2.Distance(player.Center, Main.MouseWorld);
                    if (specialTimer <= 60)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BRRR"));
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), projectile.position, Main.MouseWorld) && !npc.immortal && !npc.hide && !npc.townNPC)        //checking if the NPC collides with a line from The Hand to the mouse
                            {
                                Vector2 difference = player.position - npc.position;
                                npc.position = player.Center + (-difference / 2f);
                            }
                        }
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(15));
                    }
                    if (specialTimer > 60 && mouseDistance <= newMaxDistance * 2f)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BRRR"));
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), projectile.position, Main.MouseWorld) && !npc.immortal && !npc.hide && !npc.townNPC)
                            {
                                npc.StrikeNPC(60 * (specialTimer / 30), 0f, player.direction);      //damage goes up at a rate of 120dmg/s
                                npc.AddBuff(mod.BuffType("MissingOrgans"), 900);
                            }
                        }
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player otherPlayer = Main.player[p];
                            if (otherPlayer.active)
                            {
                                if (otherPlayer.team != player.team && otherPlayer.whoAmI != player.whoAmI && Collision.CheckAABBvLineCollision(otherPlayer.position, new Vector2(otherPlayer.width, otherPlayer.height), projectile.position, Main.MouseWorld))
                                {
                                    otherPlayer.Hurt(PlayerDeathReason.ByCustomReason(otherPlayer.name + " was scraped out of existence by " + player.name + "."), 60 * (specialTimer / 30), 1);
                                    otherPlayer.AddBuff(mod.BuffType("MissingOrgans"), 600);
                                }
                            }
                        }
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(25));
                    }
                    specialTimer = 0;
                }
                if (!attackFrames)
                {
                    if (!scrapeFrames && !secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Player player = Main.player[projectile.owner];

            if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && MyPlayer.RangeIndicators && chargeTimer != 0)
            {
                Texture2D positionIndicator = mod.GetTexture("Extras/PositionIndicator");
                Vector2 distanceToTeleport = Vector2.Zero;
                if (projectile.owner == Main.myPlayer)
                    distanceToTeleport = Main.MouseWorld - player.position; 
                distanceToTeleport.Normalize();
                distanceToTeleport *= 98f * (chargeTimer / 30f);
                spriteBatch.Draw(positionIndicator, (player.Center + distanceToTeleport) - Main.screenPosition, Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f));
            }
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Red * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), newMaxDistance * 2 / 150F, SpriteEffects.None, 0);
            }
            if (scrapeFrames)
            {
                Texture2D scrapeTrail = mod.GetTexture("Extras/ScrapeTrail");
                //spriteBatch.Draw(scrapeTrail, projectile.Center - Main.screenPosition, new Rectangle(0, 2 - projectile.frame, scrapeTrail.Width, scrapeTrail.Height / (projectile.frame + 1)), Color.White);
                int frameHeight = standTexture.Height / 2;
                spriteBatch.Draw(scrapeTrail, projectile.Center - Main.screenPosition + new Vector2(drawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * projectile.frame, standTexture.Width, frameHeight), Color.White, 0f, new Vector2(scrapeTrail.Width / 2f, frameHeight / 2f), 1f, effects, 0);
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
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
            if (secondaryAbilityFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Charge");
            }
            if (scrapeFrames)
            {
                if (!resetFrame)
                {
                    projectile.frame = 0;
                    projectile.frameCounter = 0;
                    resetFrame = true;
                }
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Scrape");
                if (resetFrame)
                {
                    if (currentAnimationDone)
                    {
                        normalFrames = true;
                        scrapeFrames = false;
                        resetFrame = false;
                    }
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                scrapeFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/TheHand/TheHand_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Charge")
            {
                AnimationStates(animationName, 4, 15, true);
            }
            if (animationName == "Scrape")
            {
                AnimationStates(animationName, 2, 10, false);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 12, true);
            }
        }
    }
}