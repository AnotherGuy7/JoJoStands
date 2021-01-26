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
            projectile.ignoreWater = true;
            drawOffsetX = -10;
            drawOriginOffsetY = -10;
        }

        private int voidDashTimer = 0;
        private int voidDashCooldownTimer = 0;
        private Vector2 savedDashVelocity = Vector2.Zero;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

            modPlayer.creamVoidMode = true;
            player.position = projectile.position + new Vector2(0f, 0f);
            player.AddBuff(mod.BuffType("SphericalVoid"), 2);
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
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

                if (player.dead || !modPlayer.StandOut || player.ownedProjectileCounts[mod.ProjectileType("Void")] >= 2)
                {
                    projectile.Kill();
                }
                if (modPlayer.voidCounter <= 0 || Main.mouseRight && modPlayer.creamTier > 2 || specialPressed && !Main.mouseLeft)
                {
                    if (specialPressed && !Main.mouseLeft || modPlayer.creamTier == 2 && modPlayer.voidCounter <= 0)
                    {
                        modPlayer.creamNormalToVoid = true;
                    }
                    projectile.Kill();
                    modPlayer.creamFrame = 7;
                    modPlayer.creamExposedToVoid = true;
                    modPlayer.creamAnimationReverse = true;
                    Main.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ExposingCream"), 0, 6f, player.whoAmI);
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
                else if (specialPressed && Main.mouseLeft && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)       //Dash option
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
                    projectile.velocity *= 5f + modPlayer.creamTier;      // 7f, 8f, 9f

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
            projectile.spriteDirection = player.direction;

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
                    if (modPlayer.creamTier <= 2)
                    {
                        if (tileToDestroy.active() && tileToDestroy.type != TileID.LihzahrdBrick && tileToDestroy.type != TileID.BlueDungeonBrick && tileToDestroy.type != TileID.GreenDungeonBrick && tileToDestroy.type != TileID.PinkDungeonBrick && tileToDestroy.type != TileID.LihzahrdAltar && tileToDestroy.type != TileID.DemonAltar)
                        {
                            WorldGen.KillTile(detectedTileX, detectedTileY, false, false, true);
                        }
                    }
                    if (modPlayer.creamTier >= 3)
                    {
                        if (tileToDestroy.active() && tileToDestroy.type != TileID.LihzahrdBrick && tileToDestroy.type != TileID.LihzahrdAltar)
                        {
                            WorldGen.KillTile(detectedTileX, detectedTileY, false, false, true);
                        }
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
                if (projectile.frame >= 3)
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
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            target.AddBuff(mod.BuffType("MissingOrgans"), 120 * modPlayer.creamTier);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            target.AddBuff(mod.BuffType("MissingOrgans"), 60 * modPlayer.creamTier);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool TileCollideStyle(ref int widht, ref int height, ref bool fallThrough)
        {
            widht = projectile.width - 4;
            height = projectile.height - 4;
            fallThrough = true;
            return true;
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