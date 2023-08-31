using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.PearlJam
{ 
	public class PearlJamStandT2 : StandClass
	{
        public override float ProjectileSpeed => 5f;
        public override int HalfStandHeight => 34;
        public override StandAttackType StandType => StandAttackType.Ranged;
        public override int ProjectileDamage => 20;
        public override int ShootTime => 35;
        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            UpdateStandSync();
            StayBehind();
            currentAnimationState = AnimationState.Idle;
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (mPlayer.standOut)
                Projectile.timeLeft = 2;
            if (shootCount > 0)
                shootCount--;
            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual)
            {
                if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
                {
                    if (shootCount <= 0)
                    {
                        shootCount += newShootTime;
                        Vector2 shootVel = Main.MouseWorld - Projectile.Center;
                        if (shootVel == Vector2.Zero)
                            shootVel = new Vector2(0f, 1f);

                        shootVel.Normalize();
                        shootVel *= ProjectileSpeed;
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootVel, ModContent.ProjectileType<HealingTomatoMozzarella>(), newProjectileDamage*-1, 3f, Projectile.owner);
                        Main.projectile[proj].netUpdate = true;
                        Projectile.netUpdate = true;
                    }
                }
                else if (Main.mouseRight && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    player.AddBuff(ModContent.BuffType<AbilityCooldown>(), 410);
                    player.AddBuff(BuffID.Regeneration, 350);
                    player.AddBuff(BuffID.WellFed2, 350);
                    player.AddBuff(BuffID.Ironskin, 350);
                }
            }

            if (mPlayer.standControlStyle == MyPlayer.StandControlStyle.Auto)
            {
                player.AddBuff(BuffID.Regeneration, 240);
                player.AddBuff(BuffID.WellFed2, 240);
                player.AddBuff(BuffID.Ironskin, 240);              
            }
        }

        public override void SelectAnimation()
        {
            if (currentAnimationState == AnimationState.Idle)
            {
                PlayAnimation("Idle");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = (Texture2D)ModContent.Request<Texture2D>("JoJoStands/Projectiles/PlayerStands/PearlJam/PearlJam_" + animationName);

            if (animationName == "Idle")
            {
                AnimateStand(animationName, 3, 15, true);
            }
        }
    }
}
