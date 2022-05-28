using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.StickyFingers
{
    public class StickyFingersStandT2 : StandClass
    {
        public override int punchDamage => 37;
        public override int altDamage => 31;
        public override int punchTime => 11;
        public override int halfStandHeight => 39;
        public override float fistWhoAmI => 4f;
        public override float tierNumber => 2f;
        public override int standType => 1;
        public override string punchSoundName => "Ari";
        public override string poseSoundName => "Arrivederci";
        public override string spawnSoundName => "Sticky Fingers";
        public override bool useProjectileAlpha => true;

        private int updateTimer = 0;
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
            updateTimer++;
            if (shootCount > 0)
                shootCount--;

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                Projectile.netUpdate = true;
            }

            if (!mPlayer.standAutoMode)
            {
                secondaryAbility = secondaryAbilityFrames = player.ownedProjectileCounts[ModContent.ProjectileType<StickyFingersFistExtended>()] != 0;
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer && player.ownedProjectileCounts[ModContent.ProjectileType<StickyFingersFistExtended>()] == 0)
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
                    if (!secondaryAbility)
                        StayBehind();
                    else
                        GoInFront();
                }
                if (Main.mouseRight && shootCount <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<StickyFingersFistExtended>()] == 0 && !zipperAmbush && !playerHasAbilityCooldown && Projectile.owner == Main.myPlayer)
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
                if ((mouseRightJustReleased || mouseRightForceRelease) && Projectile.owner == Main.myPlayer)
                {
                    if (mouseRightHoldTimer < 60 || Vector2.Distance(Main.MouseWorld, player.Center) >= 4 * 16f)
                    {
                        shootCount += 120;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= shootSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<StickyFingersFistExtended>(), (int)(altDamage * mPlayer.standDamageBoosts), 6f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                    else
                    {
                        shootCount += 60;
                        zipperAmbush = true;
                        savedAmbushPosition = player.position;
                        player.position = Main.MouseWorld;
                    }
                    mouseRightJustReleased = false;
                    mouseRightForceRelease = false;
                    mouseRightHoldTimer = 0;
                }
                if (zipperAmbush)
                {
                    player.immune = true;
                    player.immuneTime = 2;
                    for (int i = 0; i < Main.maxNPCTypes; i++)
                        player.npcTypeNoAggro[i] = true;

                    mPlayer.hideAllPlayerLayers = true;
                    mPlayer.stickyFingersAmbushMode = true;
                    if (Main.mouseRight && shootCount <= 0)
                    {
                        zipperAmbush = false;
                        player.immune = true;
                        player.immuneTime = 15;
                        for (int i = 0; i < Main.maxNPCTypes; i++)
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
                        player.AddBuff(ModContent.BuffType<SurpriseAttack>(), 3 * 60);
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(25));
                        if (JoJoStands.SoundsLoaded)
                            SoundEngine.PlaySound(new SoundStyle("JoJoStandsSounds/Sounds/SoundEffects/Zip"));
                    }
                }
            }
            if (mPlayer.standAutoMode)
            {
                PunchAndShootAI(ModContent.ProjectileType<StickyFingersFistExtended>(), shootMax: 1);
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/StickyFingers/StickyFingers_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimateStand(animationName, 1, 10, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 10, true);
            }
        }
    }
}