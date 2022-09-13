using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using JoJoStands.Items;
using JoJoStands.Items.CraftingMaterials;
using System.Security.Cryptography.X509Certificates;

namespace JoJoStands.Projectiles
{
    public class PlunderBubble : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 210;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.25f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.NextBool(2))
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                Main.dust[dustIndex].noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            bool hasPlunderItem = player.HeldItem.type == ItemID.Torch || player.HeldItem.type == ItemID.CursedTorch || player.HeldItem.type == ItemID.IchorTorch || player.HeldItem.type == ModContent.ItemType<ViralMeteoriteBar>() || player.HeldItem.type == ItemID.IceTorch;

 
            if (Main.rand.NextFloat(0, 101) <= mPlayer.standCritChangeBoosts)
            {
                crit = true;
            }
            if (Main.rand.NextBool(10) && hasPlunderItem == false && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))

            {
                target.AddBuff(ModContent.BuffType<ParchedDebuff>(), 200);
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
            }
            if (Main.rand.NextBool(15) && hasPlunderItem == false && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))

            {
                target.AddBuff(ModContent.BuffType<Asphyxiating>(), 350);
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(5));
            }


            if (Main.rand.NextBool(8) && player.HeldItem.type == ItemID.Torch)
            {
                target.AddBuff(BuffID.OnFire, 190);
            }

            if (Main.rand.NextBool(8) && player.HeldItem.type == ItemID.IchorTorch)
            {
                target.AddBuff(BuffID.Ichor, 280);
            }

            if (Main.rand.NextBool(8) && player.HeldItem.type == ItemID.CursedTorch)
            {
                target.AddBuff(BuffID.CursedInferno, 230);
            }
            if (Main.rand.NextBool(8) && player.HeldItem.type == ItemID.IceTorch)
            {
                target.AddBuff(BuffID.Frostburn, 230);
            }

            if (Main.rand.NextBool(8) && player.HeldItem.type == ModContent.ItemType<ViralMeteoriteBar>())
            {
                target.AddBuff(ModContent.BuffType<Infected>(), 280);
            }



            if (mPlayer.awakenedAmuletEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 80)
                    target.AddBuff(ModContent.BuffType<Infected>(), 60 * 9);
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.NextFloat(0, 101) >= 60)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextBool(10) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))

            {
                target.AddBuff(BuffID.Obstructed, 220);
                player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(6));
            }
        }

    

    }
}