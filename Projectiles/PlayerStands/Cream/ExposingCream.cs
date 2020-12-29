using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class ExposingCream : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.hide = true;
        }
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/Cream/Void"; }
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            player.position.X = projectile.position.X + 5;
            player.position.Y = projectile.position.Y - 15;
            player.AddBuff(mod.BuffType("Exposing"), 2);
            bool specialPressed = false;
            specialPressed = JoJoStands.SpecialHotKey.JustPressed;
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            if (projectile.owner == Main.myPlayer)
            {
                modPlayer.ExposingMode = true;
                float ScreenX = (float)Main.screenWidth / 2f;
                float ScreenY = (float)Main.screenHeight / 2f;
                modPlayer.VoidCamPosition = projectile.position + new Vector2(ScreenX, ScreenY);
                modPlayer.VoidCamPosition = new Vector2(projectile.position.X - ScreenX, projectile.position.Y - ScreenY);
                if (Main.mouseLeft)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= (1f + modPlayer.CreamPower);

                    if (Main.MouseWorld.X > projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
                if (player.dead)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                }
                if (projectile.timeLeft <= 3540 && specialPressed)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                }
                if (!modPlayer.StandOut)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("ExposingCream")] >= 2)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                }
                if (modPlayer.VoidCounter == modPlayer.VoidMax)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("Void"), (int)((64 * modPlayer.CreamPower) * modPlayer.standDamageBoosts), 6f, player.whoAmI);
                }
                if (Main.mouseRight)
                {
                    modPlayer.ExposingMode = false;
                    projectile.Kill();
                }
                projectile.netUpdate = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool TileCollideStyle(ref int widht, ref int height, ref bool fallThrough)
        {
            widht = projectile.width + 10;
            height = projectile.height + 10;
            fallThrough = true;
            return  true;
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.ExposingMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}