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
        private int tiledestroycooldown = 0;
        private bool voidwithoutplayer = false;
        private bool returnback = true;
        private Vector2 targetposition = Vector2.Zero;
        private Vector2 savedDashVelocity = Vector2.Zero;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            float distance = Vector2.Distance(player.Center, Projectile.Center);
            float distance2 = Vector2.Distance(targetposition, Projectile.Center);
            if (mPlayer.creamDash && !voidwithoutplayer)
            {
                voidwithoutplayer = true;
                targetposition = Main.MouseWorld;

            }
            if (voidwithoutplayer)
            {
                tiledestroycooldown++;
                if (Projectile.velocity.X < 0)
                    Projectile.spriteDirection = -1;
                else
                    Projectile.spriteDirection = 1;
                if (distance >= 800f)
                    returnback = false;
                if (returnback)
                {
                    Projectile.velocity = targetposition - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 6f;
                    if (distance2 <= 10f)
                        returnback = false;
                }
                if (!returnback)
                {
                    if (!Projectile.hide)
                    {
                        SoundEngine.PlaySound(SoundID.Item78);
                        Projectile.hide = true;
                    }
                    Projectile.velocity = player.Center - Projectile.Center;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 5f + mPlayer.creamTier;
                }
            }
            if (!voidwithoutplayer)
            {
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
                    bool secondspecialPressed = false;
                    if (!Main.dedServ)
                        specialPressed = JoJoStands.SpecialHotKey.JustPressed;
                    secondspecialPressed = JoJoStands.SecondSpecialHotKey.JustPressed;

                    float halfScreenWidth = (float)Main.screenWidth / 2f;
                    float halfScreenHeight = (float)Main.screenHeight / 2f;
                    mPlayer.VoidCamPosition = Projectile.position - new Vector2(halfScreenWidth, halfScreenHeight);

                    if (mPlayer.voidCounter <= 0 || secondspecialPressed && mPlayer.creamTier > 2 || specialPressed && !Main.mouseLeft)
                    {
                        if (specialPressed && !Main.mouseLeft || mPlayer.creamTier <= 2 && mPlayer.voidCounter <= 0)
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
                    else if (Main.mouseRight && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0 && mPlayer.creamTier >= 2)       //Dash option
                    {
                        mPlayer.voidCounter -= 1;
                        savedDashVelocity = Main.MouseWorld - Projectile.position;
                        savedDashVelocity.Normalize();
                        savedDashVelocity *= (8f + mPlayer.creamTier) * 2;

                        if (Main.MouseWorld.X > Projectile.position.X)
                            player.ChangeDir(1);
                        else
                            player.ChangeDir(-1);
                    }
                    else if (Main.mouseLeft && savedDashVelocity == Vector2.Zero && voidDashTimer <= 0)
                    {
                        Projectile.velocity = Main.MouseWorld - Projectile.position;
                        Projectile.velocity.Normalize();
                        if (mPlayer.creamTier >= 2)
                            Projectile.velocity *= 8f + mPlayer.creamTier;      // 10f, 11f, 12f
                        else
                            Projectile.velocity *= 0f;

                        if (mPlayer.creamTier >= 2 && Main.MouseWorld.X > Projectile.position.X)
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
                    Projectile.spriteDirection = player.direction;
                }
            }


            if (player.dead || !mPlayer.standOut || player.ownedProjectileCounts[ModContent.ProjectileType<Void>()] >= 2 || distance >= 1200f || !returnback && distance <= 40f)
            {
                if (mPlayer.creamDash && distance >= 1200f)
                {
                    mPlayer.standOut = false;
                }
                if (!returnback && distance <= 40f)
                {
                    SoundEngine.PlaySound(SoundID.Item78);
                    mPlayer.voidCounter += 2;
                }
                mPlayer.creamDash = false;
                Projectile.Kill();
            }

            if (returnback)
            {
                int creamLeftX = (int)(Projectile.position.X / 16f) - 1;
                int creamRightX = (int)((Projectile.position.X + (float)Projectile.width) / 16f) + 2;
                int creamUpY = (int)(Projectile.position.Y / 16f) - 1;
                int creamDownY = (int)((Projectile.position.Y + (float)Projectile.height) / 16f) + 2;

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
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }
            if (returnback)
            {
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
        }

        public override bool? CanDamage()
        {
            return returnback;
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
            if (returnback)
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 120 * mPlayer.creamTier);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (returnback)
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 60 * mPlayer.creamTier);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (returnback && voidwithoutplayer && tiledestroycooldown >= 20)
                returnback = false;
            return false;
        }

        public override bool? CanCutTiles()
        {
            return returnback;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width/4;
            height = Projectile.height/4;
            fallThrough = true;
            return returnback;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.creamDash = false;
            if (!voidwithoutplayer)
            {
                mPlayer.creamVoidMode = false;
                player.fallStart = (int)player.position.Y;
            }
        }
    }
}