using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;


namespace JoJoStands.Projectiles
{
    public class HealingSpaghetti : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 8;
            Projectile.aiStyle = 14;
            Projectile.timeLeft = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.statLife += 10;
            target.AddBuff(BuffID.Regeneration, 480);
            target.AddBuff(BuffID.WellFed, 480);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.Regeneration, 480);
            target.AddBuff(BuffID.WellFed, 480);
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 266, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            for (int i = 0; i < 100; i++)
            {
                NPC target = Main.npc[i];
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y - Projectile.Center.Y - 2;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                if (distance < 80f && target.friendly && target.active)
                {
                    target.AddBuff(BuffID.Regeneration, 480);
                    target.AddBuff(BuffID.WellFed, 480);

                }
            }
            for (int i = 0; i < 100; i++)
            {
                Player target = Main.player[i];
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y - Projectile.Center.Y - 2;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                if (distance < 80f && target.active)
                {

                    target.statLife += 1;
                    target.AddBuff(BuffID.Regeneration, 480);
                    target.AddBuff(BuffID.WellFed, 480);
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); //makes dust based on tile
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position); //plays impact sound
        }
    }
}
