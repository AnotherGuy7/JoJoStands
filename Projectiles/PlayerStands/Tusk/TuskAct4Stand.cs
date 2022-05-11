using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
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
            Main.projFrames[Projectile.type] = 8;
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
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (shootCount > 0)
                shootCount--;
            if (player.whoAmI == Main.myPlayer && mPlayer.tuskActNumber == 4)         //Making an owner check cause tuskActNumber isn't in sync with other players, causing TA4 to die for everyone else
                Projectile.timeLeft = 2;

            if (goldenRectangleEffectTimer >= 215)
            {
                if (JoJoStands.SoundsLoaded && !playedSpawnCry)
                {
                    LegacySoundStyle chumimiiin = SoundLoader.GetLegacySoundSlot(JoJoStands.JoJoStandsSounds, "Sounds/SoundEffects/Chumimiiin");
                    chumimiiin.WithVolume(MyPlayer.ModSoundsVolume);
                    SoundEngine.PlaySound(chumimiiin, Projectile.position);
                    playedSpawnCry = true;
                }
                for (int i = 0; i < Main.rand.Next(4, 6 + 1); i++)
                {
                    Vector2 dustSpeed = Projectile.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f + 1f), Main.rand.NextFloat(-5f, 5f + 1f));
                    Dust.NewDust(Projectile.position - new Vector2(0f, halfStandHeight), Projectile.width, halfStandHeight * 2, 169, dustSpeed.X, dustSpeed.Y);
                }
            }
            if (goldenRectangleEffectTimer > 0)
                goldenRectangleEffectTimer -= 2;

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
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

                    Vector2 velocity = (target.position + new Vector2(0f, -4f)) - Projectile.position;
                    velocity.Normalize();
                    Projectile.velocity = velocity * 4f;

                    Projectile.direction = 1;
                    if ((target.position - Projectile.Center).X < 0f)
                    {
                        Projectile.direction = -1;
                    }
                    Projectile.spriteDirection = Projectile.direction;

                    if (Main.myPlayer == Projectile.owner)
                    {
                        if (shootCount <= 0)
                        {
                            shootCount += newPunchTime;
                            Vector2 shootVel = target.position - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                            {
                                shootVel = new Vector2(0f, 1f);
                            }
                            shootVel.Normalize();
                            shootVel *= shootSpeed;
                            int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<Fists>(), newPunchDamage, punchKnockback, Projectile.owner);
                            Main.projectile[proj].netUpdate = true;
                            Main.projectile[proj].timeLeft = 3;
                            Projectile.netUpdate = true;
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


        public override bool PreDrawExtras()
        {
            if (goldenRectangleEffectTimer > 0)
            {
                Vector2 rectangleOffset = Vector2.Zero;
                /*if (Projectile.spriteDirection == 1)
                {
                    rectangleOffset = new Vector2(-30f, 0f);
                }*/
                Main.EntitySpriteDraw((Texture2D)ModContent.Request<Texture2D>("Extras/GoldenSpinComplete"), ((Projectile.Center + new Vector2(-10 * Projectile.spriteDirection, 0f)) + rectangleOffset) - Main.screenPosition - rectangleCenterOffset, null, Color.White * (((float)goldenRectangleEffectTimer * 3.9215f) / 1000f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("Projectiles/PlayerStands/Tusk/TuskAct4_" + animationName);

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