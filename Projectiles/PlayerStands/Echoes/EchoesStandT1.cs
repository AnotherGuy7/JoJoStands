using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using JoJoStands.NPCs;
using JoJoStands.Items;
using JoJoStands.Gores.Echoes;
using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.PlayerBuffs;
using Microsoft.Xna.Framework;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT1 : StandClass
    {
        public override int PunchDamage => 108;
        public override int PunchTime => 2;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 1;
        public override StandAttackType StandType => StandAttackType.Melee;

        public override string Texture
        {
            get { return Mod.Name + "/Items/EchoesACT0"; }
        }

        private bool lowPriceThreeFreeze = false;
        private bool evolve = false;

        private float ability = 0f;
        private float add = 0f;


        public override void AI()
        {
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!lowPriceThreeFreeze)
            {
                if (!player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    if (Projectile.alpha > 0)
                        Projectile.alpha--;
                    if (Main.mouseLeft && Projectile.owner == Main.myPlayer && !player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                    {
                        player.AddBuff(ModContent.BuffType<AbilityCooldown>(), mPlayer.AbilityCooldownTime(30));
                        if (Main.MouseWorld.X > Projectile.Center.X)
                            ability = 0f;
                        if (Main.MouseWorld.X < Projectile.Center.X)
                            ability = 3f;
                        Projectile.velocity = Main.MouseWorld - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 12f;
                        Projectile.netUpdate = true;
                        lowPriceThreeFreeze = true;
                    }
                    Lighting.AddLight(Projectile.position, 21);
                }
                if (player.HasBuff(ModContent.BuffType<AbilityCooldown>()))
                {
                    if (Projectile.alpha < 256)
                        Projectile.alpha++;
                }

                float offset = -4;
                if (player.direction == -1)
                    offset = -5;
                Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center - new Vector2(offset, 60f), 0.2f);
                Projectile.velocity *= 0.8f;
                Projectile.direction = Projectile.spriteDirection = player.direction;
                Projectile.rotation = 0;
                Projectile.netUpdate = true;

            }
            if (lowPriceThreeFreeze)
            {
                add++;
                Projectile.alpha = 0;
                Projectile.penetrate = 2;
                Projectile.damage = newPunchDamage;
                Projectile.rotation = Projectile.velocity.ToRotation() + ability + (add*5/100*Projectile.direction);
                Projectile.velocity.Y += 0.33f;
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (lowPriceThreeFreeze)
            {
                Projectile.alpha = 255;
                lowPriceThreeFreeze = false;
                add = 0f;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
            return false;
        }

        public override void StandKillEffects()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (evolve)
            {
                player.maxMinions += 1;
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesACT1>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesACT1>());
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_1>(), 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_2>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT2>(), 0, 0f, Main.myPlayer, 2f);
                Main.NewText("Oh? Echoes is evolving!");
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (lowPriceThreeFreeze)
                return null;
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            if (lowPriceThreeFreeze)
                return true;
            return false;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
            {
                evolve = true;
                Projectile.Kill();
            }
            if (!player.HasBuff(ModContent.BuffType<StrongWill>()))
            {
                Projectile.alpha = 255;
                add = 0f;
                lowPriceThreeFreeze = false;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
        }

        public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            damage /= 2;
            if (Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                crit = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
            {
                if (!target.boss)
                {
                    if (Main.rand.NextFloat(1, 100 + 1) <= 25)
                    {
                        evolve = true;
                        Projectile.Kill();
                    }
                    else
                    {
                        Projectile.alpha = 255;
                        add = 0f;
                        lowPriceThreeFreeze = false;
                        Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                        SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
                    }
                }
                if (target.boss)
                {
                    evolve = true;
                    Projectile.Kill();
                }
            }
            if (!player.HasBuff(ModContent.BuffType<StrongWill>()))
            {
                Projectile.alpha = 255;
                add = 0f;
                lowPriceThreeFreeze = false;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(lowPriceThreeFreeze);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            lowPriceThreeFreeze = reader.ReadBoolean();
        }
    }
}