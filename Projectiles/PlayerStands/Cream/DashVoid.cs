using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Cream
{
    public class DashVoid : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override string Texture
        {
            get { return Mod.Name + "/Projectiles/PlayerStands/Cream/Void"; }
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

        private int tiledestroycooldown = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            float distance = Vector2.Distance(player.Center, Projectile.Center);

            tiledestroycooldown++;

            if (!mPlayer.heartHeadbandEquipped)
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

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            target.AddBuff(ModContent.BuffType<MissingOrgans>(), 120 * mPlayer.creamTier);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
                target.AddBuff(ModContent.BuffType<MissingOrgans>(), 60 * mPlayer.creamTier);
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (tiledestroycooldown >= 20)
                Projectile.Kill();

            if (Main.player[Projectile.owner].GetModPlayer<MyPlayer>().heartHeadbandEquipped)
                Projectile.velocity *= -0.6f;

            return false;
        }

        public override bool? CanCutTiles()
        {
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = Projectile.width/4;
            height = Projectile.height/4;
            fallThrough = true;
            return true;
        }
    }
}