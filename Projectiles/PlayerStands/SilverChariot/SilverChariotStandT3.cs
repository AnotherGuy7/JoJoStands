using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotStandT3 : StandClass
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 10;
        }
        public override float maxDistance => 98f;
        public override int punchDamage => 51;
        public override int punchTime => 6;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 10f;
        public override string spawnSoundName => "Silver Chariot";
        public override StandType standType => StandType.Melee;
        private const int AfterImagesLimit = 3;

        private bool parryFrames = false;
        private bool Shirtless = false;
        private float punchMovementSpeed = 5f;

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

            mPlayer.silverChariotShirtless = Shirtless;
            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && !secondaryAbilityFrames && Projectile.owner == Main.myPlayer)
                {
                    Punch(punchMovementSpeed);
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        idleFrames = true;
                        attackFrames = false;
                    }
                }
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !attackFrames && Projectile.owner == Main.myPlayer)
                {
                    HandleDrawOffsets();
                    idleFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = true;
                    Projectile.netUpdate = true;
                    Rectangle parryRectangle = new Rectangle((int)Projectile.Center.X + (4 * Projectile.direction), (int)Projectile.Center.Y - 29, 16, 54);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly)
                            {
                                parryFrames = true;
                                secondaryAbilityFrames = false;
                                otherProj.owner = Projectile.owner;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
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
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                            }
                        }
                    }
                }
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                {
                    secondaryAbilityFrames = false;
                }
                if (!attackFrames && !parryFrames)
                {
                    if (!secondaryAbilityFrames)
                        StayBehind();
                    else
                        GoInFront();
                }

                if (SpecialKeyPressed())
                {
                    Shirtless = !Shirtless;

                    if (Shirtless)
                    {
                        punchMovementSpeed = 7.5f;
                        if (!player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && player.ownedProjectileCounts[ModContent.ProjectileType<SilverChariotAfterImage>()] == 0)
                        {
                            for (int i = 0; i < AfterImagesLimit; i++)
                            {
                                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<SilverChariotAfterImage>(), 0, 0f, Projectile.owner, i, AfterImagesLimit);
                                Main.projectile[proj].netUpdate = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
                            }
                        }
                    }
                    else
                    {
                        punchMovementSpeed = 5f;
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
                PlayAnimation("Secondary");
            }
            if (parryFrames)
            {
                idleFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                PlayAnimation("Parry");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void AnimationCompleted(string animationName)
        {
            if (animationName == "Parry")
            {
                idleFrames = true;
                parryFrames = false;
            }
        }

        public override void PlayAnimation(string animationName)
        {
            string pathAddition = "";
            if (Shirtless)
                pathAddition = "Shirtless_";

            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_" + pathAddition + animationName);

            if (!Shirtless)
            {
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
                    AnimateStand(animationName, 6, 3, false);
                }
                if (animationName == "Pose")
                {
                    AnimateStand(animationName, 1, 10, true);
                }
            }
            else
            {
                if (animationName == "Idle")
                {
                    AnimateStand("Shirtless" + animationName, 4, 30, true);
                }
                if (animationName == "Attack")
                {
                    AnimateStand("Shirtless" + animationName, 5, newPunchTime, true);
                }
                if (animationName == "Secondary")
                {
                    AnimateStand("Shirtless" + animationName, 1, 1, true);
                }
                if (animationName == "Parry")
                {
                    AnimateStand("Shirtless" + animationName, 6, 3, false);
                }
                if (animationName == "Pose")
                {
                    AnimateStand(animationName, 1, 10, true);
                }
            }

        }
    }
}