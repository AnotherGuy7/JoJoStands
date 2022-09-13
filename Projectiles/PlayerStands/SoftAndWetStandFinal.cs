using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.SoftAndWet
{
    public class SoftAndWetStandFinal : StandClass
    {

        public override float maxDistance => 98f;
        public override int punchDamage => 96;
        public override int punchTime => 9;
        public override int halfStandHeight => 48;
        public override int altDamage => ((int)(tierNumber * 15));
        public override int standOffset => 0;
        public override float fistWhoAmI => 0f;
        public override float tierNumber => 4f;

        public bool trapOn = false;
        public bool touchedTile = false;
        private float maxSpecDistance = 165f;
        private Vector2 savedPosition = Vector2.Zero;


        public bool bubbleBarrier = false;
        public override StandType standType => StandType.Melee;

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

            if (!mPlayer.standAutoMode)
            {
                secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<PlunderBubble>()] != 0;
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
                if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                {
                    shootCount += 28;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    SoundEngine.PlaySound(SoundID.Item85);
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<PlunderBubble>(), (int)(altDamage * mPlayer.standDamageBoosts), 2f, Projectile.owner, Projectile.whoAmI); ;
                    shootCount += 1;
                    Main.projectile[proj].netUpdate = true;
                    Projectile.netUpdate = true;
                }

                if (SpecialKeyPressed() && Projectile.owner == Main.myPlayer)
                {
                    attackFrames = false;
                    idleFrames = false;
                    float mouseToPlayerDistance = Vector2.Distance(Main.MouseWorld, player.Center);
                    bool mouseOnPlatform = TileID.Sets.Platforms[Main.tile[(int)(Main.MouseWorld.X / 16f), (int)(Main.MouseWorld.Y / 16f)].TileType];
                    if (!touchedTile)
                    {
                        if (mouseToPlayerDistance < maxSpecDistance)
                        {
                            if ((Collision.SolidCollision(Main.MouseWorld, 1, 1) || mouseOnPlatform) && !touchedTile)
                            {
                                touchedTile = true;
                                savedPosition = Main.MouseWorld;
                                SoundEngine.PlaySound(SoundID.SplashWeak);
                            }
                        }
                    }
                    else
                        if (touchedTile) { 
                        trapOn = true;
                        touchedTile = false;
                        float numberProjectiles = 4;
                        float rotation = MathHelper.ToRadians(55);      
                        float randomSpeedOffset = Main.rand.NextFloat(-3f, 3f);
                        Vector2 shootVel = new(0f, 1f);
                        
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(16));
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int trap = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, perturbedSpeed, ModContent.ProjectileType<BombBubble>(), 350, 9f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[trap].timeLeft = 100;
                            Main.projectile[trap].velocity.Y = -3f;
                            Main.projectile[trap].netUpdate = true;
                        }
                        
                        savedPosition = Vector2.Zero;
                    }
                  }
                if (SecondSpecialKeyPressed() && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                    {
                    player.AddBuff(ModContent.BuffType<BarrierBuff>(), 600);
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(27));
                    Vector2 playerFollow = Vector2.Zero;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, playerFollow,ModContent.ProjectileType<BubbleBarrier>(), 0, 0f, Projectile.owner, Projectile.whoAmI);
                }
                if (attackFrames == true && Main.rand.NextBool(37))
                {
                    SoundEngine.PlaySound(SoundID.Drip);
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<TinyBubble>(), 35, 2f, Projectile.owner, Projectile.whoAmI); 
                }

                if (attackFrames == true && Main.rand.NextBool(38))
                {
                    Vector2 shootVel = Main.MouseWorld - Projectile.position;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);
                    shootVel.Normalize();
                    shootVel *= 3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, shootVel, ModContent.ProjectileType<TinyBubble>(), 35, 2f, Projectile.owner, Projectile.whoAmI);
                }

            }
                       if (touchedTile && MyPlayer.AutomaticActivations)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    float npcDistance = Vector2.Distance(npc.Center, savedPosition);
                    if (npc.active && !npc.friendly && npcDistance < 30f && touchedTile)     
                    {
                        trapOn = true;
                        float numberProjectiles = 4;
                        float rotation = MathHelper.ToRadians(55);
                        float randomSpeedOffset = Main.rand.NextFloat(-3f, 3f);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(16));
                        Vector2 shootVel = new(0f, 1f);
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            touchedTile = false;
                            Vector2 perturbedSpeed = new Vector2(shootVel.X + randomSpeedOffset, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f;
                            int trap = Projectile.NewProjectile(Projectile.GetSource_FromThis(), savedPosition, perturbedSpeed, ModContent.ProjectileType<BombBubble>(), 410, 9f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[trap].timeLeft = 100;
                            Main.projectile[trap].velocity.Y = -3f;
                            Main.projectile[trap].netUpdate = true;
                        }
                        savedPosition = Vector2.Zero;
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                BasicPunchAI();
            }
        }

        public override bool PreDrawExtras()
        {
            if (touchedTile)
            {
                Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Extras/BubbleTrap");
                Main.EntitySpriteDraw(texture, savedPosition - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0f, new Vector2(16f / 2f), 1f, SpriteEffects.None, 0);
            }
            return true;
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/SoftAndWet/SoftAndWet_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 2, true);
            }
        }
    }
}