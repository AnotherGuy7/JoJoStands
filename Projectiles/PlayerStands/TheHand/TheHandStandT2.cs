using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TheHand
{
    public class TheHandStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 58;
        public override int punchTime => 12;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;

        private int updateTimer = 0;
        private bool scrapeFrames = false;
        private int chargeTimer = 0;
        private int specialTimer = 0;

        public override void AI()
        {
            SelectAnimation();
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
                if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                {
                    secondaryAbilityFrames = true;
                    if (chargeTimer < 150f)
                    {
                        chargeTimer++;
                    }
                }
                if (!Main.mouseRight && chargeTimer != 0)
                {
                    scrapeFrames = true;
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BRRR"));
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    distanceToTeleport *= 98f * (chargeTimer / 30f);
                    player.position = player.Center + distanceToTeleport;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(chargeTimer / 10));       //15s max cooldown
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
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(20));
                    }
                    if (specialTimer > 60)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BRRR"));
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && Collision.CheckAABBvLineCollision(npc.position, new Vector2(npc.width, npc.height), projectile.position, Main.MouseWorld) && !npc.immortal && !npc.hide && !npc.townNPC)
                            {
                                npc.StrikeNPC(60 * (specialTimer / 240), 0f, player.direction);
                                //npc.AddBuff(mod.BuffType("MissingOrgans"), 900);
                            }
                        }
                        player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(30));
                    }
                    specialTimer = 0;
                }
                /*if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")))
                {
                    chargingScrape = true;
                    secondaryAbilityFrames = true;
                }
                if (!Main.mouseRight && chargingScrape)
                {
                    chargingScrape = false;
                    scrapeFrames = true;
                    Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, mod.ProjectileType("TheHandMouse"), 0, 0f, Main.myPlayer, projectile.whoAmI);
                }
                if (projectile.ai[0] == 1f)     //teleport in that direction
                {
                    Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                    distanceToTeleport.Normalize();
                    distanceToTeleport *= 98f;
                    player.position = player.Center + distanceToTeleport;
                    player.AddBuff(mod.BuffType("AbilityCooldown"), modPlayer.AbilityCooldownTime(5));
                    projectile.ai[0] = 0f;
                }
                if (projectile.ai[0] == 2f)     //pull the enemy
                {
                    projectile.ai[0] = 0f;
                }
                if (projectile.ai[0] == 3f)     //break tiles
                {
                    projectile.ai[0] = 0f;
                }*/
                if (!attackFrames)
                {
                    if (!scrapeFrames)
                        StayBehind();
                    else
                        GoInFront();
                }
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
            Main.NewText(currentAnimationDone);
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Player player = Main.player[projectile.owner];
            if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && MyPlayer.RangeIndicators && chargeTimer != 0)
            {
                Texture2D positionIndicator = mod.GetTexture("Extras/PositionIndicator");
                Vector2 distanceToTeleport = Main.MouseWorld - player.position;
                distanceToTeleport.Normalize();
                distanceToTeleport *= 98f * (chargeTimer / 30f);
                spriteBatch.Draw(positionIndicator, (player.Center + distanceToTeleport) - Main.screenPosition, Color.White * (((float)MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f));
            }
            return true;
        }

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
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Scrape");
                if (currentAnimationDone)
                {
                    normalFrames = true;
                    scrapeFrames = false;
                }
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
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
                AnimationStates(animationName, 2, 180, false);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 2, 12, true);
            }
        }
    }
}