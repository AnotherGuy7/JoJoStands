using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using JoJoStands.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.PurpleHaze
{
    public abstract class PurpleHazeStand : StandClass
    {
        protected virtual bool CanReleaseVirus => false;
        protected virtual bool CanInfectOnHit => false;
        protected virtual bool CanAOEBurst => false;

        private bool specialActive = false;
        private NPC specialTarget = null;
        private bool specialAnimationStarted = false;
        private bool burstFired = false;

        public override float MaxDistance => 98f;
        public override int PunchDamage => 23;
        public override int PunchTime => 11;
        public override int HalfStandHeight => 37;
        public override int FistID => 0;
        public override int TierNumber => 1;
        public override string PoseSoundName => "PurpleHazePose";
        public override string SpawnSoundName => "PurpleHaze";
        public override int AmountOfPunchVariants => 3;
        public override string PunchTexturePath => "JoJoStands/Projectiles/PlayerStands/PurpleHaze/PurpleHaze_Punch_";
        public override Vector2 PunchSize => new Vector2(44, 12);
        public override bool CanUsePart4Dye => false;
        public override StandAttackType StandType => StandAttackType.Melee;

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

            if (specialActive)
            {
                HandleSpecialActive();
                return;
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.mouseLeft)
                        Punch();
                    else if (Main.mouseRight)
                        SecondaryAttack();
                    else
                    {
                        attacking = false;
                        currentAnimationState = AnimationState.Idle;
                    }

                    if (SpecialKeyPressed())
                        TryStartSpecial();
                }
                if (!attacking)
                    StayBehind();
            }
            else if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
                BasicPunchAI();

            if (mPlayer.posing)
                currentAnimationState = AnimationState.Pose;
        }

        private void SecondaryAttack()
        {
            // TODO
        }

        private void TryStartSpecial()
        {
            if (!CanAOEBurst || specialActive)
                return;

            NPC target = FindNearestTarget(
                MyPlayer.StandSearchTypeEnum.MostHealth,
                newMaxDistance * 4f,
                Vector2.Zero
            );

            if (target == null)
                return;

            specialActive = true;
            specialTarget = target;
            specialAnimationStarted = false;
            burstFired = false;

            Main.player[Projectile.owner].AddBuff(
                ModContent.BuffType<AbilityCooldown>(),
                300
            );

            Projectile.netUpdate = true;
        }

        private void HandleSpecialActive()
        {
            if (specialTarget == null || !specialTarget.active)
            {
                EndSpecial();
                return;
            }

            Player player = Main.player[Projectile.owner];

            if (!specialAnimationStarted)
            {
                currentAnimationState = AnimationState.Special;
                Projectile.spriteDirection = Projectile.direction =
                    specialTarget.Center.X > Projectile.Center.X ? 1 : -1;

                Vector2 toTarget = specialTarget.Center - Projectile.Center;
                toTarget.Normalize();
                Projectile.velocity = toTarget * 18f;

                if (Main.netMode != NetmodeID.Server)
                {
                    float shakeAmount = 2.5f;
                    Projectile.position += new Vector2(
                        Main.rand.NextFloat(-shakeAmount, shakeAmount),
                        Main.rand.NextFloat(-shakeAmount, shakeAmount)
                    );
                }

                float distToTarget = Vector2.Distance(Projectile.Center, specialTarget.Center);
                if (distToTarget < 24f)
                {
                    specialAnimationStarted = true;
                    Projectile.velocity = Vector2.Zero;
                    Projectile.frame = 0;
                    Projectile.frameCounter = 0;
                    currentAnimationState = AnimationState.Special;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                currentAnimationState = AnimationState.Special;

                Vector2 desiredPos = specialTarget.Center - new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                Projectile.position = Vector2.Lerp(Projectile.position, desiredPos, 0.25f);
                Projectile.velocity = Vector2.Zero;

                Projectile.spriteDirection = Projectile.direction =
                    specialTarget.Center.X > Projectile.Center.X ? 1 : -1;

                if (Main.netMode != NetmodeID.Server)
                {
                    float shakeAmount = 1.5f;
                    Projectile.position += new Vector2(
                        Main.rand.NextFloat(-shakeAmount, shakeAmount),
                        Main.rand.NextFloat(-shakeAmount, shakeAmount)
                    );
                }
                if (Projectile.frame >= 10 && !burstFired)
                {
                    burstFired = true;
                    if (Projectile.owner == Main.myPlayer)
                        FireMushroomCloud();
                }
                if (Projectile.frame >= 12 && burstFired)
                    EndSpecial();
            }
        }

        private void FireMushroomCloud()
        {
            Vector2 blastCenter = specialTarget.Center;
            float groundY = blastCenter.Y + specialTarget.height * 0.5f;

            int shockwaveCount = 80;
            for (int i = 0; i < shockwaveCount; i++)
            {
                float angle = MathHelper.TwoPi / shockwaveCount * i;
                float hSpeed = System.MathF.Cos(angle) * Main.rand.NextFloat(28f, 48f);
                float vSpeed = System.MathF.Sin(angle) * Main.rand.NextFloat(1f, 3f);
                Vector2 vel = new Vector2(hSpeed, vSpeed);
                int d = Dust.NewDust(new Vector2(blastCenter.X, groundY), 0, 0,
                    DustID.PurpleTorch, vel.X, vel.Y,
                    0, new Color(200, 100, 255), Main.rand.NextFloat(6f, 10f));
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.08f;
            }

            int shockDebrisCount = 60;
            for (int i = 0; i < shockDebrisCount; i++)
            {
                float angle = MathHelper.TwoPi / shockDebrisCount * i;
                float hSpeed = System.MathF.Cos(angle) * Main.rand.NextFloat(20f, 40f);
                float vSpeed = System.MathF.Abs(System.MathF.Sin(angle)) * Main.rand.NextFloat(-2f, 0f);
                int d = Dust.NewDust(new Vector2(blastCenter.X, groundY + Main.rand.NextFloat(-8f, 8f)),
                    0, 0, DustID.Smoke, hSpeed, vSpeed,
                    240, new Color(90, 10, 150), Main.rand.NextFloat(3f, 6f));
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.12f;
            }

            int concussionCount = 72;
            for (int i = 0; i < concussionCount; i++)
            {
                float angle = MathHelper.TwoPi / concussionCount * i;
                float speed = Main.rand.NextFloat(22f, 38f);
                Vector2 vel = new Vector2(speed, 0f).RotatedBy(angle);
                int d = Dust.NewDust(new Vector2(blastCenter.X, groundY), 4, 4,
                    DustID.Smoke, vel.X, vel.Y,
                    100, new Color(180, 50, 255), Main.rand.NextFloat(8f, 14f));
                Main.dust[d].noGravity = true;
            }

            int baseCount = 90;
            for (int i = 0; i < baseCount; i++)
            {
                float xOffset = Main.rand.NextFloat(-90f, 90f);
                float riseSpeed = Main.rand.NextFloat(26f, 48f);
                float wobble = xOffset * 0.06f + Main.rand.NextFloat(-2f, 2f);
                int d = Dust.NewDust(
                    new Vector2(blastCenter.X + xOffset, groundY + Main.rand.NextFloat(-10f, 20f)),
                    6, 6, DustID.Smoke, wobble, -riseSpeed,
                    130, new Color(160, 30, 230), Main.rand.NextFloat(9f, 16f));
                Main.dust[d].noGravity = true;
            }

            int midCount = 80;
            for (int i = 0; i < midCount; i++)
            {
                float xOffset = Main.rand.NextFloat(-55f, 55f);
                float riseSpeed = Main.rand.NextFloat(22f, 40f);
                float wobble = xOffset * 0.04f + Main.rand.NextFloat(-1.5f, 1.5f);
                Vector2 spawnPos = new Vector2(blastCenter.X + xOffset,
                    groundY - Main.rand.NextFloat(80f, 220f));
                int d = Dust.NewDust(spawnPos, 6, 6, DustID.Smoke, wobble, -riseSpeed,
                    140, new Color(140, 20, 210), Main.rand.NextFloat(8f, 14f));
                Main.dust[d].noGravity = true;
            }

            int neckCount = 60;
            for (int i = 0; i < neckCount; i++)
            {
                float xOffset = Main.rand.NextFloat(-30f, 30f);
                float riseSpeed = Main.rand.NextFloat(18f, 32f);
                Vector2 spawnPos = new Vector2(blastCenter.X + xOffset,
                    groundY - Main.rand.NextFloat(200f, 380f));
                int d = Dust.NewDust(spawnPos, 5, 5, DustID.Smoke, xOffset * 0.03f, -riseSpeed,
                    150, new Color(120, 15, 190), Main.rand.NextFloat(7f, 12f));
                Main.dust[d].noGravity = true;
            }

            int coreGlowCount = 48;
            for (int i = 0; i < coreGlowCount; i++)
            {
                float xOffset = Main.rand.NextFloat(-12f, 12f);
                float riseSpeed = Main.rand.NextFloat(28f, 50f);
                float yAlong = Main.rand.NextFloat(0f, 400f);
                int d = Dust.NewDust(
                    new Vector2(blastCenter.X + xOffset, groundY - yAlong),
                    0, 0, DustID.PurpleTorch, xOffset * 0.02f, -riseSpeed,
                    0, new Color(220, 120, 255), Main.rand.NextFloat(8f, 16f));
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.02f;
            }

            float capOriginY = blastCenter.Y - 480f;

            int capBellyCount = 70;
            for (int i = 0; i < capBellyCount; i++)
            {
                float xOffset = Main.rand.NextFloat(-140f, 140f);
                float curve = -(xOffset * xOffset) * 0.0012f;
                Vector2 spawnPos = new Vector2(blastCenter.X + xOffset,
                    capOriginY + 60f + curve + Main.rand.NextFloat(0f, 30f));
                Vector2 vel = new Vector2(xOffset * 0.055f, Main.rand.NextFloat(-4f, -1f));
                int d = Dust.NewDust(spawnPos, 10, 10, DustID.Smoke, vel.X, vel.Y,
                    180, new Color(80, 10, 130), Main.rand.NextFloat(10f, 16f));
                Main.dust[d].noGravity = true;
            }

            int capCount = 110;
            for (int i = 0; i < capCount; i++)
            {
                float t = (float)i / (capCount - 1);
                float angle = MathHelper.ToRadians(-180f + 180f * t);
                float speed = Main.rand.NextFloat(14f, 30f);
                Vector2 vel = new Vector2(System.MathF.Cos(angle), System.MathF.Sin(angle)) * speed;
                Vector2 spawnPos = new Vector2(
                    blastCenter.X + Main.rand.NextFloat(-40f, 40f),
                    capOriginY + Main.rand.NextFloat(-30f, 30f)
                );
                int d = Dust.NewDust(spawnPos, 12, 12, DustID.Smoke, vel.X, vel.Y,
                    120, new Color(170, 40, 240), Main.rand.NextFloat(12f, 20f));
                Main.dust[d].noGravity = true;
            }

            int capRimCount = 60;
            for (int i = 0; i < capRimCount; i++)
            {
                float t = (float)i / (capRimCount - 1);
                float angle = MathHelper.ToRadians(-160f + 160f * t);
                float speed = Main.rand.NextFloat(22f, 38f);
                Vector2 vel = new Vector2(System.MathF.Cos(angle), System.MathF.Sin(angle)) * speed;
                Vector2 spawnPos = new Vector2(
                    blastCenter.X + Main.rand.NextFloat(-50f, 50f),
                    capOriginY + Main.rand.NextFloat(-40f, 10f)
                );
                int d = Dust.NewDust(spawnPos, 14, 14, DustID.Smoke, vel.X, vel.Y,
                    100, new Color(200, 60, 255), Main.rand.NextFloat(14f, 22f));
                Main.dust[d].noGravity = true;
            }

            int glowCount = 56;
            for (int i = 0; i < glowCount; i++)
            {
                float angle = MathHelper.TwoPi / glowCount * i;
                float speed = Main.rand.NextFloat(12f, 26f);
                Vector2 vel = new Vector2(System.MathF.Cos(angle), System.MathF.Sin(angle)) * speed;
                int d = Dust.NewDust(
                    new Vector2(blastCenter.X, capOriginY), 0, 0,
                    DustID.PurpleTorch, vel.X, vel.Y,
                    0, default, Main.rand.NextFloat(14f, 24f));
                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 0.03f;
            }

            int lingerCount = 80;
            for (int i = 0; i < lingerCount; i++)
            {
                Vector2 spawnOffset = new Vector2(
                    Main.rand.NextFloat(-160f, 160f),
                    Main.rand.NextFloat(-500f, 60f)
                );
                Vector2 vel = new Vector2(
                    Main.rand.NextFloat(-1.5f, 1.5f),
                    Main.rand.NextFloat(-2f, -0.3f)
                );
                int d = Dust.NewDust(blastCenter + spawnOffset, 0, 0, DustID.Smoke, vel.X, vel.Y,
                    230, new Color(60, 5, 100), Main.rand.NextFloat(5f, 9f));
                Main.dust[d].noGravity = true;
            }

            if (CanReleaseVirus)
            {
                int virusCount = 8;
                for (int i = 0; i < virusCount; i++)
                {
                    float angle = MathHelper.TwoPi / virusCount * i;
                    Vector2 virusVel = new Vector2(ProjectileSpeed * 0.6f, 0f).RotatedBy(angle);
                    // TODO: spawn PurpleHazeVirus projectile
                }
            }
        }

        private void EndSpecial()
        {
            specialActive = false;
            specialTarget = null;
            specialAnimationStarted = false;
            burstFired = false;
            attacking = false;
            currentAnimationState = AnimationState.Idle;
            Projectile.velocity = Vector2.Zero;
            Projectile.netUpdate = true;
        }

        public override void OnDyeChanged()
        {
            punchTextures = new Texture2D[AmountOfPunchVariants];
            for (int v = 0; v < AmountOfPunchVariants; v++)
                punchTextures[v] = ModContent.Request<Texture2D>(PunchTexturePath + (v + 1), AssetRequestMode.ImmediateLoad).Value;
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
                PlayAnimation("CapsuleShot");
            else if (currentAnimationState == AnimationState.Special)
                PlayAnimation("Special");
            else if (currentAnimationState == AnimationState.Pose)
                PlayAnimation("Pose");
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = GetStandTexture("JoJoStands/Projectiles/PlayerStands/PurpleHaze", "PurpleHaze_" + animationName);

            if (animationName == "Idle")
                AnimateStand(animationName, 4, 12, true);
            else if (animationName == "Attack")
                AnimateStand(animationName, 4, newPunchTime, true);
            else if (animationName == "CapsuleShot")
                AnimateStand(animationName, 11, 15, true);
            else if (animationName == "Special")
                AnimateStand(animationName, 13, 15, false);
            else if (animationName == "Pose")
                AnimateStand(animationName, 1, 600, true);
        }
    }
}