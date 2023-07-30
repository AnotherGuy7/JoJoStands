using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StickyFingers
{
    public class StickyFingersStandFinal : StandClass
    {
        public override int PunchDamage => 76;
        public override int AltDamage => 67;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 39;
        public override int FistWhoAmI => 4;
        public override int TierNumber => 4;
        public override StandAttackType StandType => StandAttackType.Melee;
        public override string PunchSoundName => "Ari";
        public override string PoseSoundName => "Arrivederci";
        public override string SpawnSoundName => "Sticky Fingers";
        public override bool UseProjectileAlpha => true;
        public static readonly SoundStyle ZipperSound = new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip");

        private int mouseRightHoldTimer = 0;
        private bool mouseRightForceRelease = false;
        private bool zipperAmbush = false;
        private bool mouseRightPressed = false;
        private bool mouseRightJustReleased = false;
        private Vector2 savedAmbushPosition;

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

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                secondaryAbility = player.ownedProjectileCounts[ModContent.ProjectileType<StickyFingersFistExtended>()] != 0;
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft && !secondaryAbility && !zipperAmbush)
                        Punch();
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (Main.mouseRight && shootCount <= 0 && !secondaryAbility && !zipperAmbush && !playerHasAbilityCooldown)
                    {
                        mouseRightHoldTimer++;
                        mouseRightPressed = true;
                        mouseRightJustReleased = false;
                        if (mouseRightHoldTimer >= 60)
                            mouseRightForceRelease = true;
                    }
                    if (Main.mouseRightRelease && !mouseRightJustReleased && mouseRightPressed && mouseRightHoldTimer >= 5)
                    {
                        mouseRightPressed = false;
                        mouseRightJustReleased = true;
                    }
                    if (mouseRightJustReleased || mouseRightForceRelease)
                    {
                        if (mouseRightHoldTimer < 60 || Vector2.Distance(Main.MouseWorld, player.Center) >= 4 * 16f)
                        {
                            shootCount += 20;
                            Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                            if (shootVel == Vector2.Zero)
                                shootVel = new Vector2(0f, 1f);

                            shootVel.Normalize();
                            shootVel *= ProjectileSpeed;
                            int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StickyFingersFistExtended>(), (int)(AltDamage * mPlayer.standDamageBoosts), 6f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[projIndex].netUpdate = true;
                            Projectile.netUpdate = true;
                        }
                        else
                        {
                            if (!playerHasAbilityCooldown)
                            {
                                shootCount += 60;
                                zipperAmbush = true;
                                savedAmbushPosition = player.position;
                                player.position = Main.MouseWorld;
                                Projectile.netUpdate = true;
                            }
                        }
                        mouseRightJustReleased = false;
                        mouseRightForceRelease = false;
                        mouseRightHoldTimer = 0;
                    }
                }
                if (!attacking)
                {
                    if (!secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (zipperAmbush)
                {
                    player.immune = true;
                    player.immuneTime = 2;
                    player.shadowDodge = true;
                    player.shadowDodgeCount = -100f;
                    for (int i = 0; i < NPCID.Count; i++)
                        player.npcTypeNoAggro[i] = true;

                    mPlayer.hideAllPlayerLayers = true;
                    mPlayer.stickyFingersAmbushMode = true;
                    if (Main.mouseRight && shootCount <= 0 && Projectile.owner == Main.myPlayer)
                    {
                        zipperAmbush = false;
                        player.immune = true;
                        player.immuneTime = 15;
                        for (int i = 0; i < NPCID.Count; i++)
                            player.npcTypeNoAggro[i] = false;

                        Vector2 ambushVelocity = savedAmbushPosition - player.position;
                        ambushVelocity.Normalize();
                        ambushVelocity *= 6f;
                        player.position = savedAmbushPosition;
                        player.velocity = ambushVelocity;
                        if (ambushVelocity.X > 0)
                            player.direction = 1;
                        else
                            player.direction = -1;
                        savedAmbushPosition = Vector2.Zero;

                        shootCount += 30;
                        mPlayer.stickyFingersAmbushMode = false;
                        player.AddBuff(ModContent.BuffType<SurpriseAttack>(), 7 * 60);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(15));
                        Projectile.netUpdate = true;
                        if (JoJoStands.SoundsLoaded)
                            SoundEngine.PlaySound(ZipperSound);
                    }
                }
                if (SpecialKeyPressed() && shootCount <= 0 && !secondaryAbility && player.ownedProjectileCounts[ModContent.ProjectileType<StickyFingersTraversalZipper>()] == 0 && !zipperAmbush)
                {
                    shootCount += 20;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                        shootVel = new Vector2(0f, 1f);

                    shootVel.Normalize();
                    shootVel *= ProjectileSpeed * 3;
                    int projIndex = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StickyFingersTraversalZipper>(), 0, 0f, Projectile.owner, 1f);
                    Main.projectile[projIndex].netUpdate = true;
                    Projectile.netUpdate = true;
                    if (player.HasBuff<ZipperDodge>())
                        player.ClearBuff(ModContent.BuffType<ZipperDodge>());
                }
                if (SecondSpecialKeyPressed() && !player.HasBuff<ZipperDodge>())
                {
                    player.AddBuff(ModContent.BuffType<ZipperDodge>(), 2);
                }
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                PunchAndShootAI(ModContent.ProjectileType<StickyFingersFistExtended>(), shootMax: 1);
            }
            if (secondaryAbility)
                currentAnimationState = AnimationState.SecondaryAbility;
            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(zipperAmbush);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            zipperAmbush = reader.ReadBoolean();
        }

        public override bool PreDrawExtras()
        {
            Projectile.alpha = zipperAmbush ? 0 : 255;
            return true;
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
            else if (currentAnimationState == AnimationState.SecondaryAbility)
                PlayAnimation("Secondary");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StickyFingers/StickyFingers_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 30, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "Secondary")
                AnimateStand(animationName, 1, 10, true);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 10, true);
        }
    }
}