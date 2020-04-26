using Microsoft.Xna.Framework;
using System.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class Fists : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/Fists"; }
        }

        public override void SetDefaults()      //0 = SP; 1 = TW; 2 = GE; 3 = GER; 4 = SF's; 5 = KQ (Stand); 6 = KC; 
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 4;
            projectile.alpha = 255;     //completely transparent
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.ai[0] == 3f && mPlayer.BackToZero)
            {
                target.GetGlobalNPC<NPCs.JoJoGlobalNPC>().affectedbyBtz = true;
                target.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
            }
            if (projectile.ai[0] == 4f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 180);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 240);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 360);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 480);
                }
            }
            if (projectile.ai[0] == 5f)
            {
                if (projectile.ai[1] == 1f)
                {
                    PlayerStands.KillerQueenStandT1.savedTarget = target;
                }
                if (projectile.ai[1] == 2f)
                {
                    PlayerStands.KillerQueenStandT2.savedTarget = target;
                }
                if (projectile.ai[1] == 3f)
                {
                    PlayerStands.KillerQueenStandT3.savedTarget = target;
                }
                if (projectile.ai[1] == 4f)
                {
                    PlayerStands.KillerQueenStandFinal.savedTarget = target;
                }
            }
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (projectile.ai[0] == 3f && mPlayer.BackToZero)
            {
                target.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
            }
            if (projectile.ai[0] == 4f)
            {
                if (projectile.ai[1] == 1f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 180);
                }
                if (projectile.ai[1] == 2f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 240);
                }
                if (projectile.ai[1] == 3f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 360);
                }
                if (projectile.ai[1] == 4f)
                {
                    target.AddBuff(mod.BuffType("Zipped"), 480);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                for (int p = 0; p < Main.maxProjectiles; p++)
                {
                    Projectile otherProj = Main.projectile[p];
                    if (otherProj.active && projectile.Hitbox.Intersects(otherProj.Hitbox))
                    {
                        if (otherProj.type == projectile.type && projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            Dust.NewDust(otherProj.position + otherProj.velocity, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                        }
                        if (otherProj.type == mod.ProjectileType("Knife") && projectile.owner != otherProj.owner && player.team != Main.player[otherProj.owner].team)
                        {
                            otherProj.owner = projectile.owner;
                            otherProj.velocity = -otherProj.velocity * 0.4f;
                            Dust.NewDust(otherProj.position + otherProj.velocity, projectile.width, projectile.height, DustID.FlameBurst, otherProj.velocity.X * -0.5f, otherProj.velocity.Y * -0.5f);
                            if (MyPlayer.Sounds)
                            {
                                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/Punch_land").WithVolume(.3f));
                            }
                        }
                    }
                }
            }
        }
    }
}