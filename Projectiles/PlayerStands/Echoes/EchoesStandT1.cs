using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.PlayerBuffs;
using JoJoStands.Gores.Echoes;
using JoJoStands.Items;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JoJoStands.Projectiles.PlayerStands.Echoes
{
    public class EchoesStandT1 : StandClass
    {
        public override int PunchDamage => 108;
        public override int PunchTime => 2;
        public override int FistWhoAmI => 15;
        public override int TierNumber => 1;
        public override StandAttackType StandType => StandAttackType.Melee;
        public override bool CanUseRangeIndicators => false;

        public override string Texture
        {
            get { return Mod.Name + "/Items/EchoesAct0"; }
        }

        private bool thrown = false;
        private bool evolve = false;

        private float ability = 0f;
        private float rotationAdd = 0f;

        public override void ExtraSetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 34;
        }


        public override void AI()
        {
            UpdateStandInfo();
            UpdateStandSync();

            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standOut)
                Projectile.timeLeft = 2;

            if (!thrown)
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
                        else
                            ability = 3f;

                        Projectile.velocity = Main.MouseWorld - Projectile.Center;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= 12f;
                        Projectile.netUpdate = true;
                        thrown = true;
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
            else
            {
                rotationAdd++;
                Projectile.alpha = 0;
                Projectile.penetrate = 2;
                Projectile.damage = newPunchDamage;
                Projectile.rotation = Projectile.velocity.ToRotation() + ability + (rotationAdd * (5 / 100) * Projectile.direction);
                Projectile.velocity.Y += 0.33f;
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (thrown)
            {
                Projectile.alpha = 255;
                thrown = false;
                rotationAdd = 0f;
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
                mPlayer.StandSlot.SlotItem.type = ModContent.ItemType<EchoesAct1>();
                mPlayer.StandSlot.SlotItem.SetDefaults(ModContent.ItemType<EchoesAct1>());
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_1>(), 1f);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_2>(), 1f);
                SoundEngine.PlaySound(SoundID.NPCDeath1, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EchoesStandT2>(), 0, 0f, Main.myPlayer, 2f);
                Main.NewText(Language.GetText("Mods.JoJoStands.MiscText.EchoesEvolve").Value);
            }
        }

        public override bool? CanHitNPC(NPC target) => thrown ? null : false;

        public override bool CanHitPvp(Player target) => thrown ? true : false;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.PvP)
            {
                Player player = Main.player[Projectile.owner];
                if (player.HasBuff(ModContent.BuffType<StrongWill>()))
                {
                    evolve = true;
                    Projectile.Kill();
                }
                else
                {
                    thrown = false;
                    Projectile.alpha = 255;
                    rotationAdd = 0f;
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                    SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (Main.rand.Next(1, 100 + 1) <= mPlayer.standCritChangeBoosts)
                modifiers.SetCrit();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (player.HasBuff(ModContent.BuffType<StrongWill>()))
            {
                if (!target.boss)
                {
                    if (Main.rand.Next(1, 100 + 1) <= 25)
                    {
                        evolve = true;
                        Projectile.Kill();
                    }
                    else
                    {
                        Projectile.alpha = 255;
                        rotationAdd = 0f;
                        thrown = false;
                        Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                        SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
                    }
                }
                else
                {
                    evolve = true;
                    Projectile.Kill();
                }
            }
            else
            {
                thrown = false;
                Projectile.alpha = 255;
                rotationAdd = 0f;
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.GoreType<ACT0_Gore_3>(), 1f);
                SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            }
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(thrown);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            thrown = reader.ReadBoolean();
        }
    }
}