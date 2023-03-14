using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles
{
    public class ControllableBubble : ModProjectile
    {
        public override string Texture => Mod.Name + "/Projectiles/PlunderBubble";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 30 * 60;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.alpha = 254;
        }

        private int explosionTimer = 0;
        private const float ExplosionRadius = 6f * 16f;
        private readonly SoundStyle PopSound = new SoundStyle(SoundID.SplashWeak.SoundPath)
        {
            Volume = 0.4f,
            Pitch = 0.6f,
            PitchVariance = 0.3f
        };

        private readonly Dictionary<short, byte> PlunderInteractionTypes = new Dictionary<short, byte>()
        {
            { DustID.Torch, PlunderBubble.Plunder_Fire },
            { DustID.Ichor, PlunderBubble.Plunder_Ichor },
            { DustID.IchorTorch, PlunderBubble.Plunder_Ichor },
            { DustID.CursedTorch, PlunderBubble.Plunder_Cursed },
            { DustID.IceTorch, PlunderBubble.Plunder_Ice },
        };

        private readonly float[] BubbleAcceleration = new float[2] { 0.09f, 0.12f };
        private readonly float[] MaxBubbleSpeed = new float[2] { 2.1f, 3.4f };


        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int tierIndex = (mPlayer.standTier - 4) + 1;
            if (Projectile.alpha > 0)
                Projectile.alpha -= 2;
            if (explosionTimer > 0)
            {
                explosionTimer--;
                if (explosionTimer <= 0)
                    Projectile.Kill();
            }
            if (!mPlayer.standOut)
            {
                Projectile.Kill();
                return;
            }

            if (Main.mouseLeft && Projectile.owner == Main.myPlayer)
            {
                Vector2 velocity = Main.MouseWorld - Projectile.Center;
                velocity.Normalize();
                velocity *= BubbleAcceleration[tierIndex];
                if (Math.Abs(Projectile.velocity.X - velocity.X) > MaxBubbleSpeed[tierIndex])
                    velocity.X = (Math.Abs(velocity.X) / velocity.X) * 0.02f;
                if (Math.Abs(Projectile.velocity.Y - velocity.Y) > MaxBubbleSpeed[tierIndex])
                    velocity.Y = (Math.Abs(velocity.Y) / velocity.Y) * 0.02f;

                Projectile.velocity += velocity;
                if (Projectile.velocity.Length() >= MaxBubbleSpeed[tierIndex])
                    Projectile.velocity *= 0.98f;

                Projectile.netUpdate = true;
            }
            else
            {
                Projectile.velocity *= 0.98f;
            }

            for (int d = 0; d < Main.maxDust; d++)
            {
                Dust dust = Main.dust[d];
                if (dust.active && PlunderInteractionTypes.ContainsKey((short)dust.type) && Projectile.Hitbox.Contains(dust.position.ToPoint()))
                {
                    Projectile.ai[0] = PlunderInteractionTypes[(short)dust.type];
                    break;
                }
            }

            if (Main.mouseRight && mPlayer.standControlStyle == MyPlayer.StandControlStyle.Manual && !player.HasBuff<AbilityCooldown>() && Projectile.owner == Main.myPlayer)
            {
                Projectile.ai[1] = 1f;
                explosionTimer = Main.rand.Next(1, 25 + 1);
                Projectile.netUpdate = true;
            }

            if (Main.rand.NextBool(2))
            {
                int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f, Projectile.alpha);
                Main.dust[dustIndex].noGravity = true;
            }

            if (Projectile.ai[0] != PlunderBubble.Plunder_None && Main.rand.NextBool(3))
            {
                for (int i = 0; i < Main.rand.Next(1, 3 + 1); i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, PlunderBubble.PlunderDusts[(int)Projectile.ai[0] - 1], Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((byte)explosionTimer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            explosionTimer = reader.ReadByte();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int plunderType = (int)Projectile.ai[0];
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;

            if (plunderType == PlunderBubble.Plunder_None && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
            {
                if (Main.rand.NextBool(5))
                    target.AddBuff(ModContent.BuffType<ParchedDebuff>(), 200);
                else if (Main.rand.NextBool(7))
                    target.AddBuff(ModContent.BuffType<Asphyxiating>(), 350);
            }
            else
            {
                if (Main.rand.NextBool(3))
                {
                    if (plunderType == PlunderBubble.Plunder_Fire)
                        target.AddBuff(BuffID.OnFire, 190);
                    else if (plunderType == PlunderBubble.Plunder_Ichor)
                        target.AddBuff(BuffID.Ichor, 280);
                    else if (plunderType == PlunderBubble.Plunder_Cursed)
                        target.AddBuff(BuffID.CursedInferno, 230);
                    else if (plunderType == PlunderBubble.Plunder_Ice)
                        target.AddBuff(BuffID.Frostburn, 230);
                    else if (plunderType == PlunderBubble.Plunder_Viral)
                        target.AddBuff(ModContent.BuffType<Infected>(), 280);
                }
            }
            if (mPlayer.crackedPearlEquipped)
            {
                if (Main.rand.Next(1, 100 + 1) <= 40)
                    target.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(10) && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                target.AddBuff(BuffID.Obstructed, 4 * 60);
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.ai[1] == 0f)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X * -0.5f, Projectile.velocity.Y * -0.5f);
                for (int i = 0; i < 15; i++)
                {
                    float circlePos = i;
                    Vector2 spawnPos = Projectile.Center + (circlePos.ToRotationVector2() * 8f);
                    Vector2 velocity = spawnPos - Projectile.Center;
                    velocity.Normalize();
                    int dustIndex = Dust.NewDust(spawnPos, Projectile.width, Projectile.height, DustID.Cloud, velocity.X * 0.8f, velocity.Y * 0.8f, Scale: Main.rand.NextFloat(0.8f, 2.2f));
                    Main.dust[dustIndex].noGravity = true;
                }
                SoundEngine.PlaySound(PopSound);
            }
            else
            {
                for (int i = 0; i < 30; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Alpha: 100, Scale: 1.2f);
                    Main.dust[dustIndex].velocity *= 1.4f;
                }
                for (int i = 0; i < 20; i++)
                {
                    int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 3.5f);
                    Main.dust[dustIndex].noGravity = true;
                    Main.dust[dustIndex].velocity *= 7f;
                    dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Alpha: 100, Scale: 1.5f);
                    Main.dust[dustIndex].velocity *= 3f;
                }

                bool crit = false;
                MyPlayer mPlayer = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();
                if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                    crit = true;

                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.lifeMax > 5 && !npc.friendly && !npc.hide && !npc.immortal && npc.Distance(Projectile.Center) <= ExplosionRadius)
                        {
                            int hitDirection = -1;
                            if (npc.position.X - Projectile.position.X > 0)
                                hitDirection = 1;

                            npc.StrikeNPC(Projectile.damage * 2, 9f, hitDirection, crit);
                        }
                    }
                }
                SoundEngine.PlaySound(SoundID.Item14);
            }
        }
    }
}