using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Tusk
{
    public class TuskAct4Stand : StandClass
    {
        public override string poseSoundName => "ItsBeenARoundaboutPath";
        public override string punchSoundName => "Tusk_Ora";

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 105;
        public override int punchTime => 12;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 0f;
        public override int standType => 2;
        public override int standOffset => 20;

        private int goldenRectangleEffectTimer = 256;
        private bool playedSpawnCry = false;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (shootCount > 0)
                shootCount--;
            if (player.whoAmI == Main.myPlayer && mPlayer.tuskActNumber == 4)         //Making an owner check cause tuskActNumber isn't in sync with other players, causing TA4 to die for everyone else
                projectile.timeLeft = 2;

            if (goldenRectangleEffectTimer >= 215)
            {
                if (JoJoStands.SoundsLoaded && !playedSpawnCry)
                {
                    Terraria.Audio.LegacySoundStyle chumimiiin = JoJoStands.JoJoStandsSounds.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/Chumimiiin");
                    chumimiiin.WithVolume(MyPlayer.ModSoundsVolume);
                    Main.PlaySound(chumimiiin, projectile.position);
                    playedSpawnCry = true;
                }
                for (int i = 0; i < Main.rand.Next(4, 6 + 1); i++)
                {
                    Vector2 dustSpeed = projectile.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f + 1f), Main.rand.NextFloat(-5f, 5f + 1f));
                    Dust.NewDust(projectile.position - new Vector2(0f, halfStandHeight), projectile.width, halfStandHeight * 2, 169, dustSpeed.X, dustSpeed.Y);
                }
            }
            if (goldenRectangleEffectTimer > 0)
                goldenRectangleEffectTimer -= 2;

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
                if (!attackFrames)
                {
                    StayBehind();
                }
            }

            if (mPlayer.standAutoMode)
            {
                NPC target = FindNearestTarget(9f * 16f);
                if (target != null)
                {
                    attackFrames = true;
                    normalFrames = false;
                    PlayPunchSound();

                    Vector2 velocity = (target.position + new Vector2(0f, -4f)) - projectile.position;
                    velocity.Normalize();
                    projectile.velocity = velocity * 4f;

                    projectile.direction = 1;
                    if ((target.position - projectile.Center).X < 0f)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;

                    if (Main.myPlayer == projectile.owner)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += newPunchTime;
                            Vector2 shootVel = target.position - projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(projectile.Center, shootVel, mod.ProjectileType("Fists"), newPunchDamage, punchKnockback, projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                            Main.projectile[proj].timeLeft = 3;
                            projectile.netUpdate = true;
                        }
                    }
                }
                else
                {
                    attackFrames = false;
                    normalFrames = true;
                }
                if (target == null || (!attackFrames && normalFrames))
                {
                    StayBehind();
                }
            }
        }

        private readonly Vector2 rectangleCenterOffset = new Vector2(57f, 36f);


        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            if (goldenRectangleEffectTimer > 0)
            {
                Vector2 rectangleOffset = Vector2.Zero;
                /*if (projectile.spriteDirection == 1)
                {
                    rectangleOffset = new Vector2(-30f, 0f);
                }*/
                spriteBatch.Draw(mod.GetTexture("Extras/GoldenSpinComplete"), ((projectile.Center + new Vector2(-10 * projectile.spriteDirection, 0f)) + rectangleOffset) - Main.screenPosition - rectangleCenterOffset, Color.White * (((float)goldenRectangleEffectTimer * 3.9215f) / 1000f));
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
                PlayAnimation("Idle");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/Tusk/TuskAct4_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
        }
    }
}