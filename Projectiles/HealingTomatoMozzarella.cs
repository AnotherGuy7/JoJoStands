using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Buffs.Debuffs;
namespace JoJoStands.Projectiles
{
    public class HealingTomatoMozzarella : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 14;
            Projectile.aiStyle = 14;
            Projectile.timeLeft = 400;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Main.projFrames[Projectile.type] = 1;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.statLife += 30;
            target.AddBuff(BuffID.Regeneration, 1120);
            target.AddBuff(BuffID.WellFed2, 1120);
            target.AddBuff(BuffID.Honey, 480);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.Regeneration, 1120);
            target.AddBuff(BuffID.WellFed2, 1120);
            target.AddBuff(BuffID.Honey, 480);
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, 266, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
            for (int i = 0; i < 100; i++)
            {
                Player target = Main.player[i];
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y - Projectile.Center.Y - 2;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                if (distance < 120f && target.active)
                {
                    target.statLife += 1;
                    target.AddBuff(BuffID.Regeneration, 2);
                    target.AddBuff(BuffID.WellFed, 2);
                    target.AddBuff(BuffID.Honey, 2);
                }
            }
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y - Projectile.Center.Y - 2;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
                if (distance < 120f && !target.friendly && target.active)
                {
                    target.AddBuff(BuffID.Regeneration, 2);
                    target.AddBuff(BuffID.WellFed, 2);
                    target.AddBuff(BuffID.Honey, 2);
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
