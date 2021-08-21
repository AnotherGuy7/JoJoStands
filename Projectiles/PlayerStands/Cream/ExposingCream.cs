using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class ExposingCream : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/Cream/Void"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.hide = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -10;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            projectile.frame = 1;
            player.position = projectile.position + new Vector2(0f, 0f);
            player.AddBuff(mod.BuffType("Exposing"), 2);

            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            if (player.dead || !mPlayer.standOut || mPlayer.creamVoidMode || player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] >= 2 || mPlayer.creamAnimationReverse && mPlayer.creamFrame == 0 && mPlayer.creamNormalToExposed)
            {
                mPlayer.creamFrame = 0;
                mPlayer.creamNormalToExposed = false;
                mPlayer.creamAnimationReverse = false;
                mPlayer.creamExposedMode = false;
                projectile.Kill();
                return;
            }
            if (Main.mouseRight && !mPlayer.creamNormalToExposed && !mPlayer.creamExposedToVoid)
            {
                mPlayer.creamFrame = 5;
                mPlayer.creamNormalToExposed = true;
                mPlayer.creamAnimationReverse = true;
            }
            if (projectile.owner == Main.myPlayer)
            {
                if (!Main.dedServ)
                    mPlayer.creamExposedMode = true;
                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.VoidCamPosition = projectile.Center - (halfScreenWidth, halfScreenHeight);
                if (Main.mouseLeft)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= (1f + mPlayer.creamTier);

                    if (Main.MouseWorld.X > projectile.position.X)
                    {
                        player.ChangeDir(1);
                    }
                    else
                    {
                        player.ChangeDir(-1);
                    }
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
                projectile.netUpdate = true;
            }
        }

        public override bool TileCollideStyle(ref int widht, ref int height, ref bool fallThrough)
        {
            widht = projectile.width + 14;
            height = projectile.height + 14;
            fallThrough = true;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.creamExposedMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}