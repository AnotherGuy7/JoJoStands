using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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
            drawOffsetX = -20;
            drawOriginOffsetY = -20;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            //player.position.X = projectile.position.X + 5;
            //player.position.Y = projectile.position.Y - 15;
            projectile.frame = 1;
            player.position = projectile.position + new Vector2(-10f, -15f);
            player.AddBuff(mod.BuffType("Exposing"), 2);
            bool specialPressed = false;
            specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            if (player.dead || !modPlayer.StandOut || modPlayer.creamVoidMode || player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] >= 2 || modPlayer.creamAnimationReverse && modPlayer.creamFrame == 0 && modPlayer.creamNormalToExposed)
            {
                modPlayer.creamFrame = 0;
                modPlayer.creamNormalToExposed = false;
                modPlayer.creamAnimationReverse = false;
                modPlayer.creamExposedMode = false;
                projectile.Kill();
                return;
            }
            if (Main.mouseRight && !modPlayer.creamNormalToExposed && !modPlayer.creamExposedToVoid)
            {
                modPlayer.creamFrame = 5;
                modPlayer.creamNormalToExposed = true;
                modPlayer.creamAnimationReverse = true;
            }
            if (projectile.owner == Main.myPlayer)
            {
                if (!Main.dedServ)
                modPlayer.creamExposedMode = true;
                float ScreenX = (float)Main.screenWidth / 2f;
                float ScreenY = (float)Main.screenHeight / 2f;
                modPlayer.VoidCamPosition = projectile.position + new Vector2(ScreenX, ScreenY);
                modPlayer.VoidCamPosition = new Vector2(projectile.position.X - ScreenX, projectile.position.Y - ScreenY);
                if (Main.mouseLeft)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= (1f + modPlayer.creamTier);

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
            widht = projectile.width + 15;
            height = projectile.height + 15;
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
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.creamExposedMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}