using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KingCrimson
{
    public class KingCrimsonStandFinal : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 11;
        }

        public override int punchDamage => 186;
        public override float punchKnockback => 5f;
        public override int punchTime => 20;      //KC's punch timings are based on it's frame, so punchTime has to be 3 frames longer than the duration of the frame KC punches in
        public override int halfStandHeight => 32;
        public override float fistWhoAmI => 6f;
        public override int standOffset => 0;
        public override string poseSoundName => "AllThatRemainsAreTheResults";
        public override string spawnSoundName => "King Crimson";
        public override int standType => 1;

        private int updateTimer = 0;
        private int timeskipStartDelay = 0;
        private int blockSearchTimer = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
                shootCount--;
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("SkippingTime")) && !player.HasBuff(mod.BuffType("ForesightBuff")) && timeskipStartDelay <= 0)
            {
                if (JoJoStands.JoJoStandsSounds == null)
                    timeskipStartDelay = 80;
                else
                {
                    Terraria.Audio.LegacySoundStyle kingCrimson = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/KingCrimson");
                    kingCrimson.WithVolume(MyPlayer.ModSoundsVolume);
                    Main.PlaySound(kingCrimson, projectile.position);
                    timeskipStartDelay = 1;
                }
            }
            if (timeskipStartDelay != 0)
            {
                timeskipStartDelay++;
                if (timeskipStartDelay >= 80)
                {
                    player.AddBuff(mod.BuffType("PreTimeSkip"), 10);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));
                    timeskipStartDelay = 0;
                }
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !secondaryAbilityFrames && !player.HasBuff(mod.BuffType("SkippingTime")))
                {
                    HandleDrawOffsets();
                    attackFrames = true;
                    normalFrames = false;
                    projectile.netUpdate = true;

                    float rotaY = Main.MouseWorld.Y - projectile.Center.Y;
                    projectile.rotation = MathHelper.ToRadians((rotaY * projectile.spriteDirection) / 6f);

                    projectile.direction = 1;
                    if (Main.MouseWorld.X < projectile.position.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    Vector2 velocityAddition = Main.MouseWorld - projectile.position;
                    velocityAddition.Normalize();
                    velocityAddition *= 5f;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + velocityAddition;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }

                    if (shootCount <= 0 && (projectile.frame == 0 || projectile.frame == 4))
                    {
                        shootCount += newPunchTime / 2;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner, fistWhoAmI);
                        Main.projectile[proj].netUpdate = true;
                        projectile.netUpdate = true;
                    }
                    LimitDistance();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && !playerHasAbilityCooldown && !player.HasBuff(mod.BuffType("SkippingTime")))
                {
                    GoInFront();
                    normalFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;

                    if (blockSearchTimer > 0)
                    {
                        blockSearchTimer--;
                        return;
                    }

                    int rectWidth = 56;
                    int rectHeight = 64;
                    Rectangle blockRect = new Rectangle((int)projectile.Center.X - (rectWidth / 2), (int)projectile.Center.Y - (rectHeight / 2), rectWidth, rectHeight);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (blockRect.Intersects(otherProj.Hitbox) && otherProj.type != projectile.type && !otherProj.friendly)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, 114);
                                }

                                otherProj.penetrate -= 1;
                                if (otherProj.penetrate <= 0)
                                {
                                    projectile.Kill();
                                }
                                secondaryAbilityFrames = false;

                                Vector2 repositionOffset = new Vector2(5f * 16f * -player.direction, 0f);
                                while (WorldGen.SolidTile((int)(player.position.X + repositionOffset.X) / 16, (int)(player.position.Y + repositionOffset.Y) / 16))
                                {
                                    repositionOffset.Y -= 16f;
                                }
                                player.position += repositionOffset;
                                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(4));
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));
                                for (int i = 0; i < 20; i++)
                                {
                                    Dust.NewDust(player.position, player.width, player.height, 114);
                                }
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.lifeMax > 5 && !npc.immortal && !npc.townNPC && !npc.friendly && !npc.hide && npc.Hitbox.Intersects(blockRect))
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, 114);
                            }

                            Vector2 repositionOffset = new Vector2(5f * 16f * -player.direction, 0f);
                            while (WorldGen.SolidTile((int)(player.position.X + repositionOffset.X) / 16, (int)(player.position.Y + repositionOffset.Y) / 16))
                            {
                                repositionOffset.Y -= 16f;
                            }
                            player.position += repositionOffset;
                            player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(4));
                            npc.StrikeNPC(newPunchDamage * 2, punchKnockback * 1.5f, projectile.direction);
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/TimeSkip"));

                            for (int i = 0; i < 20; i++)
                            {
                                Dust.NewDust(player.position, player.width, player.height, 114);
                            }
                        }
                    }
                    blockSearchTimer += 5;
                }
                else
                {
                    secondaryAbilityFrames = false;
                }
                if (!attackFrames && !secondaryAbilityFrames)
                {
                    StayBehind();
                }
                if (SecondSpecialKeyPressed() && projectile.owner == Main.myPlayer && !player.HasBuff(mod.BuffType("ForesightBuff")) && !player.HasBuff(mod.BuffType("SkippingTime")))
                {
                    player.AddBuff(mod.BuffType("ForesightBuff"), 540);
                    mPlayer.epitaphForesightActive = true;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModNetHandler.effectSync.SendForesight(256, player.whoAmI, true, player.whoAmI);
                    }
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
                PlayAnimation("Block");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/KingCrimson/KingCrimson_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 6, newPunchTime / 2, true);
            }
            if (animationName == "Block")
            {
                AnimateStand(animationName, 4, 15, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}