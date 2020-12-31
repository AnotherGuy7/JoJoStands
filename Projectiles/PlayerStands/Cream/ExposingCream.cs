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
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            drawOffsetX = -32;
            drawOriginOffsetY = -32;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            //player.position.X = projectile.position.X + 5;
            //player.position.Y = projectile.position.Y - 15;
            projectile.frame = 1;
            player.position = projectile.position + new Vector2(5f, -15f);
            player.AddBuff(mod.BuffType("Exposing"), 2);
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            if (player.dead || !modPlayer.StandOut || modPlayer.creamVoidMode || player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] >= 2)
            {
                modPlayer.creamExposedMode = false;
                projectile.Kill();
                return;
            }

            if (projectile.ai[0] <= 60f)        //A timer for disabling exposed mode
            {
                projectile.ai[0]++;
            }

            if (projectile.owner == Main.myPlayer)
            {
                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

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
                        drawOffsetX = -32;
                    }
                    else
                    {
                        player.ChangeDir(-1);
                        drawOffsetX = 0;
                    }
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }

                if (Main.mouseRight && projectile.ai[0] >= 60f)
                {
                    modPlayer.creamExposedMode = false;
                    projectile.Kill();
                }
                if (specialPressed)
                {
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("Void"), (int)((64 * modPlayer.creamTier) * modPlayer.standDamageBoosts), 6f, player.whoAmI);
                    projectile.Kill();
                }
                projectile.netUpdate = true;
            }
        }

        public override bool TileCollideStyle(ref int widht, ref int height, ref bool fallThrough)
        {
            widht = projectile.width + 10;
            height = projectile.height + 10;
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