using JoJoStands.Buffs.Debuffs;
using JoJoStands.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.GoldExperience
{
    public class GoldExperienceStandT1 : StandClass
    {
        public override int punchDamage => 16;
        public override int punchTime => 12;
        public override int halfStandHeight => 35;
        public override float fistWhoAmI => 2f;
        public override float tierNumber => 1f;
        public override int standOffset => 32;
        public override string punchSoundName => "GER_Muda";
        public override string poseSoundName => "TheresADreamInMyHeart";
        public override string spawnSoundName => "Gold Experience";
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
                if (Main.mouseRight && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()) && Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<GEFrog>(), 1, 0f, Projectile.owner, tierNumber, tierNumber - 1f);
                    Main.projectile[proj].netUpdate = true;
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
                }
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
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/GoldExperience/GoldExperience_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 4, 30, true);
            }
            if (animationName == "Attack")
            {
                AnimateStand(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Pose")
            {
                AnimateStand(animationName, 1, 12, true);
            }
        }
    }
}