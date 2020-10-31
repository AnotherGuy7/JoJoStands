using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.KillerQueenBTD
{
    public class KillerQueenBTDStand : StandClass
    {
        public override float shootSpeed => 4f;
        public override int shootTime => 60;
        public override int halfStandHeight => 37;
        public override int standType => 2;
        public override int standOffset => -10;
        public override string poseSoundName => "IWouldntLose";

        private int bubbleDamage = 180;      //not using projectileDamage cause this one changes
        private int btdStartDelay = 0;


        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (mPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (Main.dayTime)
            {
                bubbleDamage = 180;
            }
            if (!Main.dayTime)
            {
                bubbleDamage = 158;
            }
            drawOriginOffsetY = -halfStandHeight;
            int newBubbleDamage = (int)(bubbleDamage * mPlayer.standDamageBoosts);

            if (!attackFrames)
                StayBehind();
            if (attackFrames)
                GoInFront();

            if (SpecialKeyPressed() && !player.HasBuff(mod.BuffType("BitesTheDust")))
            {
                if (JoJoStands.JoJoStandsSounds == null)
                {
                    btdStartDelay = 205;
                }
                else
                {
                    Terraria.Audio.LegacySoundStyle biteTheDust = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/BiteTheDust");
                    biteTheDust.WithVolume(MyPlayer.soundVolume);
                    Main.PlaySound(biteTheDust, projectile.position);
                    btdStartDelay = 1;
                }
            }
            if (btdStartDelay != 0)
            {
                btdStartDelay++;
                if (btdStartDelay >= 205)
                {
                    player.AddBuff(mod.BuffType("BitesTheDust"), 10);
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/BiteTheDustEffect"));
                    btdStartDelay = 0;
                }
            }


            if (!mPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    attackFrames = true;
                    Main.mouseRight = false;
                    projectile.netUpdate = true;
                    if (projectile.frame == 4 && !mPlayer.StandAutoMode)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = Main.MouseWorld - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Bubble"), newBubbleDamage, 6f, Main.myPlayer, 1f, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                    if (projectile.frame >= 5)
                    {
                        attackFrames = false;
                    }
                }
                else if (!secondaryAbilityFrames)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        normalFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && projectile.ai[0] == 0f)
                {
                    secondaryAbilityFrames = true;
                    projectile.ai[0] = 1f;      //to detonate all bombos
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/KQButtonClick"));
                }
                if (secondaryAbilityFrames && projectile.ai[0] == 1f)
                {
                    if (projectile.frame >= 2)
                    {
                        projectile.ai[0] = 0f;
                        normalFrames = true;
                        attackFrames = false;
                        secondaryAbilityFrames = false;
                    }
                }
            }
            if (mPlayer.StandAutoMode)
            {
                NPC target = null;
                Vector2 targetPos = projectile.position;
                float targetDist = 350f;
                if (target == null)
                {
                    for (int k = 0; k < 200; k++)       //the targeting system
                    {
                        NPC npc = Main.npc[k];
                        if (npc.CanBeChasedBy(this, false))
                        {
                            float distance = Vector2.Distance(npc.Center, player.Center);
                            if (distance < targetDist && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                if (npc.boss)       //is gonna try to detect bosses over anything
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                                else        //if it fails to detect a boss, it'll detect the next best thing
                                {
                                    targetDist = distance;
                                    targetPos = npc.Center;
                                    target = npc;
                                }
                            }
                        }
                    }
                }
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;
                    if ((targetPos - projectile.Center).X > 0f)
                    {
                        projectile.spriteDirection = projectile.direction = 1;
                    }
                    else if ((targetPos - projectile.Center).X < 0f)
                    {
                        projectile.spriteDirection = projectile.direction = -1;
                    }
                    if (attackFrames && projectile.frame == 5 && shootCount <= 0)
                    {
                        if (Main.myPlayer == projectile.owner)
                        {
                            shootCount += newShootTime;
                            Vector2 shootVel = targetPos - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, mod.ProjectileType("Bubble"), newBubbleDamage, 6f, Main.myPlayer, 0f, projectile.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    normalFrames = true;
                    attackFrames = false;
                }
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
                PlayAnimation("Secondary");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/KillerQueenBTD/KQBTD_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 6, newShootTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 4, 18 - Main.player[projectile.whoAmI].GetModPlayer<MyPlayer>().standSpeedBoosts, false);
            }
        }
    }
}