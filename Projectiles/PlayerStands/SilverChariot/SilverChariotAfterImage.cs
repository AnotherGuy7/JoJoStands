using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotAfterImage : StandClass
    {
        public override void SetStaticDefaults()
        { }

        public override void SetDefaults()
        {
            projectile.alpha = 186;
            projectile.width = 38;
            projectile.height = 1;
            projectile.netImportant = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 32;
        public override int punchTime => 7;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 10f;
        public override int standType => 1;
        public override bool useProjectileAlpha => true;

        public int updateTimer = 0;
        private bool parryFrames = false;
        private int parryCooldownTime = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.silverChariotShirtless)
                projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (parryCooldownTime == 0)
            {
                if (projectile.ai[1] == 3f)
                {
                    parryCooldownTime = 6;
                }
                else
                {
                    parryCooldownTime = 3;
                }
            }

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer && !secondaryAbilityFrames)
                {
                    Punch(7.5f);
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
                                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(parryCooldownTime));
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
                                player.AddBuff(mod.BuffType("AbilityCooldown"), mPlayer.AbilityCooldownTime(parryCooldownTime));
                            }
                        }
                    }

                    projectile.direction = 1;
                    if (projectile.position.X <= player.position.X)
                    {
                        projectile.direction = -1;
                    }
                    projectile.spriteDirection = projectile.direction;
                }
                if (!Main.mouseRight)
                {
                    secondaryAbilityFrames = false;
                }

                if (!attackFrames)
                {
                    float angle = 360f / projectile.ai[1];
                    angle *= projectile.ai[0];

                    projectile.position = player.Center + (MathHelper.ToRadians(angle).ToRotationVector2() * (4f * 16f));
                    if (!secondaryAbilityFrames && !parryFrames)
                    {
                        normalFrames = true;
                        projectile.spriteDirection = projectile.direction = player.direction;
                    }
                    HandleDrawOffsets();
                }
            }
            else
            {
                BasicPunchAI();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work

            SyncAndApplyDyeSlot();
            DrawStand(spriteBatch, drawColor);

            return true;
        }

        private void DrawStand(SpriteBatch spriteBatch, Color drawColor)
        {
            effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }

            drawColor *= projectile.alpha / 255f;
            if (standTexture != null && Main.netMode != NetmodeID.Server)
            {
                int frameHeight = standTexture.Height / Main.projFrames[projectile.whoAmI];
                spriteBatch.Draw(standTexture, projectile.Center - Main.screenPosition + new Vector2(drawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * projectile.frame, standTexture.Width, frameHeight), drawColor, 0f, new Vector2(standTexture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
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
                if (projectile.frame >= 5)
                {
                    parryFrames = false;
                    normalFrames = true;
                }
                PlayAnimation("Parry");
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
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/SilverChariot/SilverChariot_Shirtless_" + animationName);
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
                    AnimateStand(animationName, 6, 8, false);
                }
                if (animationName == "Pose")
                {
                    AnimateStand(animationName, 1, 10, true);
                }
            }
        }
    }
}
