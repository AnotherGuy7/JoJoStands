using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace JoJoStands.Projectiles
{
    public class EchoesTailTip : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 10;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 2100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        private Vector2 targetPosition = Vector2.Zero;
        private bool off = false;
        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Projectile.owner == Main.myPlayer)
                targetPosition = Main.MouseWorld;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (Vector2.Distance(Projectile.Center, targetPosition) <= 10f)
            {
                Projectile.hide = true;
                Projectile.velocity *= 0f;
                Projectile.netUpdate = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Main.rand.Next(0, 1 + 1) == 0 && !Projectile.hide)
                DustTrail(DustID.PinkTorch);
            if (Projectile.hide)
                DustSpawn(DustID.PinkTorch);
        }
        private void DustTrail(int DustID)
        {
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].noLight = true;
            Main.dust[dustIndex].fadeIn = 2f;
        }
        private void DustSpawn(int DustID)
        {
            for (int i = 0; i < Main.rand.Next(1, 5); i++)
            {
                Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-120f, 120f), Main.rand.NextFloat(-120f, 120f));
                if (Vector2.Distance(Projectile.Center, dustPosition) > 120f)
                    continue;

                int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].noLight = true;
                Main.dust[dustIndex].fadeIn = 2f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return true;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
    }
}