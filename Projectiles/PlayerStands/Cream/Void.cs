using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.ItemBuff;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class Void : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            DrawOffsetX = -10;
            DrawOriginOffsetY = -10;
            Projectile.netImportant = true;
        }

        private int voidDashTimer = 0;
        private int voidDashCooldownTimer = 0;
        private Vector2 savedDashVelocity = Vector2.Zero;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.creamVoidMode = true;
            mPlayer.hideAllPlayerLayers = true;
            player.position = Projectile.position + new Vector2(0f, 0f);
            player.AddBuff(ModContent.BuffType<SphericalVoid>(), 2);
            if (player.mount.Type != 0)
                player.mount.Dismount(player);
            if (voidDashCooldownTimer > 0)
                voidDashCooldownTimer--;

            if (Projectile.owner == Main.myPlayer)
            {
                bool specialPressed = false;
                if (!Main.dedServ)
                    specialPressed = JoJoStands.SpecialHotKey.JustPressed;

                float halfScreenWidth = (float)Main.screenWidth / 2f;
                float halfScreenHeight = (float)Main.screenHeight / 2f;
                mPlayer.VoidCamPosition = Projectile.position - new Vector2(halfScreenWidth, halfScreenHeight);

                if (player.dead || !mPlayer.standOut || player.ownedProjectileCounts[ModContent.ProjectileType<Void>()] >= 2)
                    Projectile.Kill();

                if (mPlayer.voidCounter <= 0 || Main.mouseRight && mPlayer.creamTier > 2 || specialPressed && !Main.mouseLeft)
                {
                    if (specialPressed && !Main.mouseLeft || mPlayer.creamTier == 2 && mPlayer.voidCounter <= 0)
                        mPlayer.creamNormalToVoid = true;

                    Projectile.Kill();
                    mPlayer.creamFrame = 7;
                    mPlayer.creamExposedToVoid = true;
                    mPlayer.creamAnimationReverse = true;
                    SoundEngine.PlaySound(SoundID.Item78);
                    Vector2 shootVelocity = Main.MouseWorld - player.position;
                    shootVelocity.Normalize();
                    shootVelocity *= 5f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Top, shootVelocity, ModContent.ProjectileType<ExposingCream>(), 0, 6f, player.whoAmI);
                    Projectile.netUpdate = true;
                }
                if (savedDashVelocity != Vector2.Zero)
                {
                    voidDashTimer++;
                    Projectile.velocity = savedDashVelocity;
                    if (voidDashTimer >= 90)
                    {
                        voidDashTimer = 0;
                        voidDashCooldownTimer = 180;
                        savedDashVelocity = Vector2.Zero;
                    }
                    Projectile.netUpdate = true;
                }
                else if (specialPressed && Main.mouseLeft && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)       //Dash option
                {
                    mPlayer.voidCounter -= 1;
                    savedDashVelocity = Main.MouseWorld - Projectile.position;
                    savedDashVelocity.Normalize();
                    savedDashVelocity *= 12f;

                    if (Main.MouseWorld.X > Projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                }
                else if (Main.mouseLeft && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)
                {
                    Projectile.velocity = Main.MouseWorld - Projectile.position;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f + mPlayer.creamTier;      // 7f, 8f, 9f

                    if (Main.MouseWorld.X > Projectile.position.X)
                        player.ChangeDir(1);
                    else
                        player.ChangeDir(-1);
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.netUpdate = true;
                }
            }
            Projectile.spriteDirection = player.direction;

            int creamLeftX = (int)(Projectile.position.X / 16f);
            int creamRightX = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 1;
            int creamUpY = (int)(Projectile.position.Y / 16f) - 1;
            int creamDownY = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 1;

            if (creamLeftX < 0)
                creamLeftX = 0;
            if (creamRightX > Main.maxTilesX)
                creamRightX = Main.maxTilesX;

            if (creamUpY < 0)
                creamUpY = 0;
            if (creamDownY > Main.maxTilesY)
                creamDownY = Main.maxTilesY;

            for (int detectedTileX = creamLeftX; detectedTileX < creamRightX; detectedTileX++)
            {
                for (int detectedTileY = creamUpY; detectedTileY < creamDownY; detectedTileY++)
                {
                    Tile tileToDestroy = Main.tile[detectedTileX, detectedTileY];
                    if (mPlayer.creamTier <= 2)
                    {
                        if (tileToDestroy.HasTile && tileToDestroy.TileType != TileID.LihzahrdBrick && tileToDestroy.TileType != TileID.BlueDungeonBrick && tileToDestroy.TileType != TileID.GreenDungeonBrick && tileToDestroy.TileType != TileID.PinkDungeonBrick && tileToDestroy.TileType != TileID.LihzahrdAltar && tileToDestroy.TileType != TileID.DemonAltar)
                        {
                            WorldGen.KillTile(detectedTileX, detectedTileY, false, false, true);
                        }
                    }
                    if (mPlayer.creamTier >= 3)
                    {
                        if (tileToDestroy.HasTile && tileToDestroy.TileType != TileID.LihzahrdBrick && tileToDestroy.TileType != TileID.LihzahrdAltar)
                        {
                            WorldGen.KillTile(detectedTileX, detectedTileY, false, false, true);
                        }
                    }
                    if (Main.tile[detectedTileX, detectedTileY].LiquidAmount > 0)
                    {
                        Main.tile[detectedTileX, detectedTileY].LiquidAmount = 0;
                        WorldGen.SquareTileFrame(detectedTileX, detectedTileY, false);
                    }
                }
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }

            for (int i = 0; i < Main.rand.Next(2, 4 + 1); i++)
            {
                int dustIndex;
                if (Main.rand.Next(0, 1 + 1) == 0)
                    dustIndex = Dust.NewDust(Projectile.position, Projectile.width + 6, Projectile.height + 6, DustID.PurpleCrystalShard);
                else
                    dustIndex = Dust.NewDust(Projectile.position, Projectile.width + 6, Projectile.height + 6, DustID.PurpleTorch);

                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
                crit = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.AddBuff(ModContent.BuffType<MissingOrgans>(), 120 * mPlayer.creamTier);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.AddBuff(ModContent.BuffType<MissingOrgans>(), 60 * mPlayer.creamTier);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width - 4;
            height = Projectile.height - 4;
            fallThrough = true;
            return true;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            mPlayer.creamVoidMode = false;
            player.fallStart = (int)player.position.Y;
        }
    }
}