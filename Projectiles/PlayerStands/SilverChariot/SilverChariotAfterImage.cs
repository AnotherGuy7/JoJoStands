using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SilverChariot
{
    public class SilverChariotAfterImage : StandClass
    {
        public override void SetStaticDefaults()
        { }

        public override void SetDefaults()
        {
            Projectile.alpha = 186;
            Projectile.width = 38;
            Projectile.height = 1;
            Projectile.netImportant = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.shouldFallThrough = true;
        }

        public override float maxDistance => 98f;
        public override int punchDamage => 26;
        public override int punchTime => 7;
        public override int halfStandHeight => 37;
        public override float fistWhoAmI => 10f;
        public override StandType standType => StandType.Melee;
        public override bool useProjectileAlpha => true;

        private bool parryFrames = false;
        private int parryCooldownTime = 0;

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.silverChariotShirtless)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;

            if (parryCooldownTime == 0)
            {
                if (Projectile.ai[1] == 3f)
                    parryCooldownTime = 6;
                else
                    parryCooldownTime = 3;
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
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !attackFrames && !parryFrames && Projectile.owner == Main.myPlayer)
                {
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
                                otherProj.owner = Projectile.owner;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(parryCooldownTime));
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
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(parryCooldownTime));
                            }
                        }
                    }

                    Projectile.direction = 1;
                    if (Projectile.position.X <= player.position.X)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;
                }
                if (!Main.mouseRight)
                {
                    secondaryAbilityFrames = false;
                }

                if (!attackFrames)
                {
                    float angle = 360f / Projectile.ai[1];
                    angle *= Projectile.ai[0];

                    Projectile.position = player.Center + (MathHelper.ToRadians(angle).ToRotationVector2() * (4f * 16f));
                    if (!secondaryAbilityFrames && !parryFrames)
                    {
                        idleFrames = true;
                        Projectile.spriteDirection = Projectile.direction = player.direction;
                    }
                    HandleDrawOffsets();
                }
            }
            else
            {
                BasicPunchAI();
            }
        }

        public override bool PreDraw(ref Color drawColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);        //starting a draw with dyes that work

            SyncAndApplyDyeSlot();
            DrawStand(drawColor);

            return true;
        }

        private void DrawStand(Color drawColor)
        {
            effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                effects = SpriteEffects.FlipHorizontally;

            drawColor *= Projectile.alpha / 255f;
            if (standTexture != null && Main.netMode != NetmodeID.Server)
            {
                int frameHeight = standTexture.Height / Main.projFrames[Projectile.whoAmI];
                Main.EntitySpriteDraw(standTexture, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), drawColor, 0f, new Vector2(standTexture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
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
                if (Projectile.frame >= 5)
                {
                    parryFrames = false;
                    idleFrames = true;
                }
                PlayAnimation("Parry");
            }
            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                idleFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_Shirtless_" + animationName);
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
