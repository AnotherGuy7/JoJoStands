using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandT2 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }
        public override float maxDistance => 98f;
        public override int punchDamage => 34;
        public override int punchTime => 7;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 10f;
        public override int standType => 1;

        private int updateTimer = 0;
        private bool parryFrames = false;

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

            if (!mPlayer.standAutoMode)
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
                if (Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !attackFrames && !parryFrames && projectile.owner == Main.myPlayer)
                {
                    normalFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    projectile.netUpdate = true;
                    Rectangle parryRectangle = new Rectangle((int)projectile.Center.X + (4 * projectile.direction), (int)projectile.Center.Y - 29, 16, 54);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != projectile.type && !otherProj.friendly)
                            {
                                parryFrames = true;
                                otherProj.owner = projectile.owner;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(10));
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (!npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && parryRectangle.Intersects(npc.Hitbox))
                            {
                                npc.StrikeNPC(npc.damage * 2, 6f, player.direction);
                                secondaryAbilityFrames = false;
                                parryFrames = true;
                                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(10));
                            }
                        }
                    }
                }
                if (!Main.mouseRight)
                {
                    secondaryAbilityFrames = false;
                }
                if (!attackFrames && !parryFrames)
                {
                    if (!Main.mouseRight && !player.HasBuff(mod.BuffType("AbilityCooldown")))
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
            if (parryFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Parry");
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Parry")
            {
                normalFrames = true;
                parryFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/SilverChariot/SilverChariot_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 5, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 1, 1, true);
            }
            if (animationName == "Parry")
            {
                AnimateStand(animationName, 6, 8, false);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}