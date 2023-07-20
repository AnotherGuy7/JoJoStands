using JoJoStands.Buffs.Debuffs;
using JoJoStands.Networking;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
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
            Projectile.friendly = true;
            Projectile.shouldFallThrough = true;
        }

        public override float MaxDistance => 98f;
        public override int PunchDamage => 26;
        public override int PunchTime => 9;
        public override int HalfStandHeight => 37;
        public override int FistWhoAmI => 10;
        public override bool UseProjectileAlpha => true;
        public override StandAttackType StandType => StandAttackType.Melee;
        public new AnimationState currentAnimationState;
        public new AnimationState oldAnimationState;

        private bool parryFrames = false;
        private int parryCooldownTime = 0;

        public new enum AnimationState
        {
            Idle,
            Attack,
            Secondary,
            Parry,
            Pose
        }

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
            if (secondaryAbility || parryFrames)
            {
                if (mouseX > player.position.X)
                    player.direction = 1;
                if (mouseX < player.position.X)
                    player.direction = -1;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer && !secondaryAbility)
                {
                    Punch(7.5f);
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        currentAnimationState = AnimationState.Idle;
                }
                if (Main.mouseRight && !attacking && !parryFrames && Projectile.owner == Main.myPlayer)
                {
                    secondaryAbility = true;
                    currentAnimationState = AnimationState.Secondary;
                    Projectile.netUpdate = true;
                    Rectangle parryRectangle = new Rectangle((int)Projectile.Center.X + (4 * Projectile.direction), (int)Projectile.Center.Y - 29, 16, 54);
                    for (int p = 0; p < Main.maxProjectiles; p++)
                    {
                        Projectile otherProj = Main.projectile[p];
                        if (otherProj.active)
                        {
                            if (parryRectangle.Intersects(otherProj.Hitbox) && otherProj.type != Projectile.type && !otherProj.friendly && !otherProj.GetGlobalProjectile<JoJoGlobalProjectile>().exceptionForSCParry)
                            {
                                parryFrames = true;
                                otherProj.owner = Projectile.owner;
                                otherProj.damage += (int)(otherProj.damage * mPlayer.standDamageBoosts) - otherProj.damage;
                                otherProj.damage *= 2;
                                otherProj.velocity *= -1;
                                otherProj.hostile = false;
                                otherProj.friendly = true;
                                SyncCall.SyncStandEffectInfo(player.whoAmI, otherProj.whoAmI, 10, 1);
                            }
                        }
                    }
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                        {
                            if (!npc.townNPC && !npc.friendly && !npc.immortal && !npc.hide && parryRectangle.Intersects(npc.Hitbox))
                            {
                                int damage = (int)(npc.damage * 2 * mPlayer.standDamageBoosts);
                                NPC.HitInfo hitInfo = new NPC.HitInfo()
                                {
                                    Damage = damage,
                                    Knockback = 6f,
                                    HitDirection = player.direction
                                };
                                npc.StrikeNPC(hitInfo);
                                SyncCall.SyncStandEffectInfo(player.whoAmI, npc.whoAmI, 10, 2, damage, player.direction);
                                secondaryAbility = false;
                                parryFrames = true;
                                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(3));
                            }
                        }
                    }

                    Projectile.direction = 1;
                    if (Projectile.position.X <= player.position.X)
                        Projectile.direction = -1;
                    Projectile.spriteDirection = Projectile.direction;
                }
                if (!Main.mouseRight && Projectile.owner == Main.myPlayer)
                    secondaryAbility = false;

                if (!attacking)
                {
                    float angle = 360f / Projectile.ai[1];
                    angle *= Projectile.ai[0];

                    Projectile.position = player.Center + (MathHelper.ToRadians(angle).ToRotationVector2() * (4f * 16f));
                    if (!secondaryAbility && !parryFrames)
                    {
                        currentAnimationState = AnimationState.Idle;
                        Projectile.spriteDirection = Projectile.direction = player.direction;
                    }
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
                int frameHeight = standTexture.Height / Main.projFrames[Projectile.type];
                Main.EntitySpriteDraw(standTexture, Projectile.Center - Main.screenPosition + new Vector2(DrawOffsetX / 2f, 0f), new Rectangle(0, frameHeight * Projectile.frame, standTexture.Width, frameHeight), drawColor, 0f, new Vector2(standTexture.Width / 2f, frameHeight / 2f), 1f, effects, 0);
            }
        }

        public override void SelectAnimation()
        {
            if (oldAnimationState != currentAnimationState)
            {
                Projectile.frame = 0;
                Projectile.frameCounter = 0;
                oldAnimationState = currentAnimationState;
                Projectile.netUpdate = true;
            }

            if (currentAnimationState == AnimationState.Idle)
                PlayAnimation("Idle");
            else if (currentAnimationState == AnimationState.Attack)
                PlayAnimation("Attack");
            else if (currentAnimationState == AnimationState.Secondary)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Parry)
                PlayAnimation("Parry");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SilverChariot/SilverChariot_Shirtless_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 5, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 1, 1, true);
            else if (animationName == "Parry")
                AnimateStand(animationName, 6, 8, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }
        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(parryFrames);
        }
        public override void ReceiveExtraStates(BinaryReader reader)
        {
            parryFrames = reader.ReadBoolean();
        }
    }
}
