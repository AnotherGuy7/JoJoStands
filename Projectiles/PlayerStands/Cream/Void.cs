using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class Void : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            drawOffsetX = -32;
            drawOriginOffsetY = -32;
        }

        private int voidDashTimer = 0;
        private int voidDashCooldownTimer = 0;
        private Vector2 savedDashVelocity = Vector2.Zero;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            //player.position.X = projectile.position.X + 5;
            //player.position.Y = projectile.position.Y - 15;
            modPlayer.creamVoidMode = true;
            player.position = projectile.position + new Vector2(5f, -15f);
            player.AddBuff(mod.BuffType("SphericalVoid"), 2);
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }

            if (projectile.ai[0] <= 60f)        //A timer for disabling void mode
            {
                projectile.ai[0]++;
            }
            if (voidDashCooldownTimer > 0)
            {
                voidDashCooldownTimer--;
            }

            if (projectile.owner == Main.myPlayer)
            {
                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                modPlayer.VoidCamPosition = projectile.position - new Vector2(halfScreenWidth, halfScreenHeight);

                if (player.dead || !modPlayer.StandOut || (projectile.ai[0] >= 60f && specialPressed) || modPlayer.voidCounter <= 0 || player.ownedProjectileCounts[mod.ProjectileType("Void")] >= 2)
                {
                    if (modPlayer.voidCounter <= 0)
                    {
                        Main.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 5f;
                        Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ExposingCream"), 0, 6f, player.whoAmI);
                    }
                    projectile.Kill();
                }

                if (savedDashVelocity != Vector2.Zero)
                {
                    voidDashTimer++;
                    projectile.velocity = savedDashVelocity;
                    if (voidDashTimer >= 90)
                    {
                        voidDashTimer = 0;
                        voidDashCooldownTimer = 180;
                        savedDashVelocity = Vector2.Zero;
                    }
                }
                else if (Main.mouseRight && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)       //Dash option
                {
                    modPlayer.voidCounter -= 1;
                    savedDashVelocity = Main.MouseWorld - projectile.position;
                    savedDashVelocity.Normalize();
                    savedDashVelocity *= 12f;

                    if (Main.MouseWorld.X > projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else if (Main.mouseLeft && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 5f + modPlayer.creamTier;      //8f and 9f

                    if (Main.MouseWorld.X > projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
            }

            int creamLeftX = (int)(projectile.position.X / 16f) - 1;
            int creamRightX = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
            int creamUpY = (int)(projectile.position.Y / 16f) - 1;
            int creamDownY = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;

            if (creamLeftX < 0)
            {
                creamLeftX = 0;
            }
            if (creamRightX > Main.maxTilesX)
            {
                creamRightX = Main.maxTilesX;
            }

            if (creamUpY < 0)
            {
                creamUpY = 0;
            }
            if (creamDownY > Main.maxTilesY)
            {
                creamDownY = Main.maxTilesY;
            }

            for (int detectedTileX = creamLeftX; detectedTileX < creamRightX; detectedTileX++)
            {
                for (int detectedTileY = creamUpY; detectedTileY < creamDownY; detectedTileY++)
                {
                    Tile tileToDestroy = Main.tile[detectedTileX, detectedTileY];
                    if (tileToDestroy.active() && tileToDestroy.type != TileID.LihzahrdBrick && tileToDestroy.type != TileID.LihzahrdAltar)
                    {
                        WorldGen.KillTile(detectedTileX, detectedTileY, false, false, true);
                    }
                    if (Main.tile[detectedTileX, detectedTileY].liquid > 0)
                    {
                        Main.tile[detectedTileX, detectedTileY].liquid = 0;
                        WorldGen.SquareTileFrame(detectedTileX, detectedTileY, false);
                    }
                }
            }

            projectile.frameCounter++;
            if (projectile.frameCounter >= 12)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("MissingOrgans"), 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("MissingOrgans"), 300);
        }

        public override bool? CanHitNPC(NPC target)
        {
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

            modPlayer.creamVoidMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}