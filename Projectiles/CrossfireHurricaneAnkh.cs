using JoJoStands.Buffs.Debuffs;
using JoJoStands.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class CrossfireHurricaneAnkh : ModProjectile
    {
        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/FireAnkh"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 12 * 60;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        private float radiusAdd = 1f;
        private float radiusAddIncrementTimer = 0f;
        private float currentRadius = 0f;
        private float rotationSpeed = 0.02f;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            radiusAddIncrementTimer++;
            if (radiusAddIncrementTimer >= 25)
            {
                radiusAdd += 0.5f;
                radiusAddIncrementTimer = 0;
            }
            if (rotationSpeed < 0.1f)
                rotationSpeed *= 1.03f;

            Projectile.ai[1] += 0.02f;
            if (currentRadius < Projectile.ai[0])
                currentRadius += radiusAdd;
            Vector2 offset = player.Center + (Projectile.ai[1].ToRotationVector2() * currentRadius) - (Projectile.Size / 2f);
            Projectile.position = offset;
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, Scale: 3.5f);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].velocity *= 1.4f;

            if (Projectile.Center.X > player.Center.X)
                Projectile.direction = 1;
            else
                Projectile.direction = -1;
            Projectile.spriteDirection = Projectile.direction;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;

            target.immune[Projectile.owner] = 0;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.GetGlobalNPC<JoJoGlobalNPC>().hitByCrossfireHurricane = true;
            target.GetGlobalNPC<JoJoGlobalNPC>().crossfireHurricaneEffectTimer = (5 + (3 * (mPlayer.standTier - 3))) * 60;
            if (Main.rand.Next(0, 101) < 50f)
                target.AddBuff(BuffID.OnFire, 300);
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(0, 101) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
    }
}