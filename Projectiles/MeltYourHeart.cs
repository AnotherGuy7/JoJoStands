using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class MeltYourHeart : ModProjectile
    {
        private int dripTimer;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 1800;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.maxPenetrate = -1;
        }

        private const int AttachmentIndex_Top = 1;
        private const int AttachmentIndex_Bottom = 2;
        private const int AttachmentIndex_Left = 3;
        private const int AttachmentIndex_Right = 4;


        public override void AI()
        {
            if (Projectile.ai[0] != 0f)
            {
                dripTimer++;
                DrawOriginOffsetY = -4;
                if (Projectile.ai[0] == AttachmentIndex_Top)     //stuck to the top
                {
                    Projectile.rotation = 0f;
                    Projectile.frameCounter++;
                    if (Projectile.frameCounter >= 22.5f)
                    {
                        Projectile.frame++;
                        Projectile.frameCounter = 0;
                        if (Projectile.frame >= 5)
                        {
                            int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 3f, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                            Main.projectile[drip].netUpdate = true;
                            Projectile.netUpdate = true;
                            Projectile.frame = 1;
                        }
                    }
                }
                else if (Projectile.ai[0] == AttachmentIndex_Bottom)     //stuck to the bottom
                {
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(180f);
                }
                else if (Projectile.ai[0] == AttachmentIndex_Right)     //stuck to the right
                {
                    DrawOffsetX = -8;
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(270f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        Projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                else if (Projectile.ai[0] == AttachmentIndex_Left)     //stuck to the left
                {
                    Projectile.frame = 1;
                    Projectile.rotation = MathHelper.ToRadians(90f);
                    if (dripTimer >= 90)
                    {
                        int drip = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 6f, ModContent.ProjectileType<MeltYourHeartDrip>(), Projectile.damage, 2f, Projectile.owner, Projectile.whoAmI);
                        Main.projectile[drip].netUpdate = true;
                        Projectile.netUpdate = true;
                        dripTimer = 0;
                    }
                }
                Projectile.velocity = Vector2.Zero;
            }
            else
            {
                Projectile.frame = 0;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(0, 101) < 20)
                target.AddBuff(BuffID.Confused, 120);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.position = (Projectile.position / 16f) * 16;
            for (int i = 0; i < 4; i++)
            {
                int checkNumber = i + 1;
                int Xadd = 0;
                int Yadd = 0;
                if (checkNumber == AttachmentIndex_Top)
                    Yadd = -1;
                else if (checkNumber == AttachmentIndex_Bottom)
                    Yadd = 1;
                else if (checkNumber == AttachmentIndex_Left)
                    Xadd = 1;
                else if (checkNumber == AttachmentIndex_Right)
                    Xadd = -1;

                Tile tileTarget = Main.tile[(int)(Projectile.Center.X / 16f) + Xadd, (int)(Projectile.Center.Y / 16f) + Yadd];
                if (tileTarget.HasTile)
                {
                    Projectile.ai[0] = checkNumber;
                    Projectile.frame = 0;
                }
            }
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}