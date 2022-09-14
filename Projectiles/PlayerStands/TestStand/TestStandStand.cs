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
        public override int PunchDamage => 70;
        public override int HalfStandHeight => 37;
        public override int PunchTime => 7;
        public override StandAttackType StandType => StandAttackType.Melee;

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
            UpdateStandSync();
            //rippleEffectTimer--;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && !player.HasBuff(ModContent.BuffType<TheWorldBuff>()))
            {
                Timestop(30);
                timestopPoseTimer += 60;
            }
            if (timestopPoseTimer > 0)
            {
                timestopPoseTimer--;
                idleFrames = false;
                attackFrames = false;
                Projectile.frame = 6;
                Main.mouseLeft = false;
                Main.mouseRight = false;
            }
            if (mPlayer.timestopActive && !mPlayer.timestopOwner)
                return;

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
                idleFrames = false;
                PlayAnimation("Attack");
            }
            if (idleFrames)
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