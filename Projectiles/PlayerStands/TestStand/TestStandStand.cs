using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.TestStand
{
    public class TestStandStand : StandClass
    {
        public override int punchDamage => 70;
        public override int halfStandHeight => 37;
        public override int punchTime => 7;
        public override int standType => 1;

        private int timestopPoseTimer = 0;

        /*ripple effect info
        private int rippleCount = 3;
        private int rippleSize = 5;
        private int rippleSpeed = 15;
        private float distortStrength = 100f;*/

        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            //rippleEffectTimer--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;

            if (!mPlayer.standAutoMode)
            {
                if (Main.mouseLeft && player.whoAmI == Main.myPlayer)
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
                if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
                {
                    Timestop(30);
                    timestopPoseTimer += 60;
                }
                if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && player.HasBuff(ModContent.BuffType<TheWorldBuff>()) && timestopPoseTimer <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<RoadRoller>()] == 0)
                {
                    shootCount += 12;
                    Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                    if (shootVel == Vector2.Zero)
                    {
                        shootVel = new Vector2(0f, 1f);
                    }
                    shootVel.Normalize();
                    shootVel *= shootSpeed + 4f;
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<RoadRoller>(), 120, 5f, Projectile.owner);
                    Main.projectile[proj].netUpdate = true;
                    Projectile.netUpdate = true;
                }
                if (timestopPoseTimer > 0)
                {
                    timestopPoseTimer--;
                    normalFrames = false;
                    attackFrames = false;
                    Projectile.frame = 6;
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                }
                /*if (rippleEffectTimer <= 0)
                {
                    Filters.Scene["Shockwave"].Deactivate();
                    rippleEffectTimer = 0;
                }*/
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
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                attackFrames = false;
                PlayAnimation("Idle");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/TestStand/TestStand_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 2, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
        }
    }
}