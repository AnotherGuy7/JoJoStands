using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class Void : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.aiStyle = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.hide = true;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.rotation += MathHelper.ToRadians(13f * projectile.direction);
            player.position.X = projectile.position.X + 5;
            player.position.Y = projectile.position.Y - 15;
            player.AddBuff(mod.BuffType("SphericalVoid"), 2);
            if (player.mount.Type != 0)
            {
                player.mount.Dismount(player);
            }
            if (projectile.owner == Main.myPlayer)
            {
                modPlayer.VoidMode = true;
                float ScreenX = (float)Main.screenWidth / 2f;
                float ScreenY = (float)Main.screenHeight / 2f;
                modPlayer.VoidCamPosition = projectile.position + new Vector2(ScreenX, ScreenY);
                modPlayer.VoidCamPosition = new Vector2(projectile.position.X - ScreenX, projectile.position.Y - ScreenY);
                if (Main.mouseLeft) 
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= (5f + modPlayer.CreamPower);

                    if (Main.MouseWorld.X > projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else
                {
                    projectile.velocity = Vector2.Zero;
                }
                bool specialPressed = false;
                specialPressed = JoJoStands.SpecialHotKey.JustPressed;
                if (player.dead)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                }
                if (projectile.timeLeft <= 3540 && Main.mouseRight)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                }
                if (!modPlayer.StandOut)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                }
                if (specialPressed)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("Void")] >= 2)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                }
                if (modPlayer.VoidCounter <= 0)
                {
                    modPlayer.VoidMode = false;
                    projectile.Kill();
                    if (modPlayer.CreamPower > 2)
                    {
                        Main.PlaySound(SoundID.Item78);
                        Vector2 shootVelocity = Main.MouseWorld - player.position;
                        shootVelocity.Normalize();
                        shootVelocity *= 5f;
                        Projectile.NewProjectile(player.Top, shootVelocity, mod.ProjectileType("ExposingCream"), 0, 6f, player.whoAmI);
                    }
                }
                projectile.netUpdate = true;
            }
            {
                int cream1 = (int)(projectile.position.X / 16f) - 1;
                int cream2 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
                int cream3 = (int)(projectile.position.Y / 16f) - 1;
                int cream4 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
                if (cream1 < 0)
                {
                    cream1 = 0;
                }
                if (cream2 > Main.maxTilesX)
                {
                    cream2 = Main.maxTilesX;
                }
                if (cream3 < 0)
                {
                    cream3 = 0;
                }
                if (cream4 > Main.maxTilesY)
                {
                    cream4 = Main.maxTilesY;
                }
                for (int cream5 = cream1; cream5 < cream2; cream5++)
                {
                    for (int cream6 = cream3; cream6 < cream4; cream6++)
                    {
                        Tile tileToDestroy1 = Main.tile[cream5, cream6];
                        if (modPlayer.CreamPower <= 2)
                        {
                            if (tileToDestroy1.active() && tileToDestroy1.type != TileID.LihzahrdBrick && tileToDestroy1.type != TileID.BlueDungeonBrick && tileToDestroy1.type != TileID.GreenDungeonBrick && tileToDestroy1.type != TileID.PinkDungeonBrick && tileToDestroy1.type != TileID.LihzahrdAltar && tileToDestroy1.type != TileID.DemonAltar)
                            {
                                WorldGen.KillTile(cream5, cream6, false, false, true);
                            }
                        }
                        if (modPlayer.CreamPower >= 3)
                        {
                            if (tileToDestroy1.active() && tileToDestroy1.type != TileID.LihzahrdBrick && tileToDestroy1.type != TileID.LihzahrdAltar)
                            {
                                WorldGen.KillTile(cream5, cream6, false, false, true);
                            }
                        }
                        if (Main.tile[cream5, cream6].liquid > 0)
                        {
                            int amount = Main.tile[cream5, cream6].liquid;
                            Main.tile[cream5, cream6].liquid = 0;
                            WorldGen.SquareTileFrame(cream5, cream6, false);
                        }
                    }
                }
                return;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
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
        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("MissingOrgans"), 300);
        }
        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("MissingOrgans"), 300);
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            modPlayer.VoidMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}