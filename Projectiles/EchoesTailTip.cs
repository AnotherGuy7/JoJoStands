using JoJoStands.NPCs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.Buffs.Debuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using System;
using Terraria.Audio;
using ReLogic.Utilities;

namespace JoJoStands.Projectiles
{
    public class EchoesTailTip : ModProjectile
    {
        private Vector2 targetPosition = Vector2.Zero;

        private int off = 1800;

        private int damageTimer = 0;
        private int boingFrame = 0;
        private int boingFrameCounter = 0;
        private int kaboomFrame = 0;
        private int kaboomFrameCounter = 0;
        private int visualSoundEffectSpawnTimer = 0;
        private int visualEffectTimer = 0;          //For some reason updating timers in loops is extremely slow.
        private SlotId soundSlotID;
        private ActiveSound activeSound;

        public const int Effect_Boing = 1;
        public const int Effect_Kabooom = 2;
        public const int Effect_Wooosh = 3;
        public const int Effect_Sizzle = 4;

        public const float BoingEffectDistance = 9f * 16f;
        public const float KabooomEffectDistance = 8f * 16f;
        public const float WoooshEffectDistance = 15f * 16f;
        public const float SizzleEffectDistance = 10f * 16f;
        private readonly Vector2 BoingOrigin = new Vector2(31, 22);
        private readonly Vector2 KaboomOrigin = new Vector2(25, 13);
        private readonly SoundStyle boingSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Echoes_Boing") { Volume = 0.1f, PitchVariance = 1f };
        private readonly SoundStyle boing2Sound = new SoundStyle("JoJoStands/Sounds/GameSounds/Echoes_Boing2") { Volume = 0.7f };
        private readonly SoundStyle woooshSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Echoes_Wooosh") { Volume = 0.7f };
        private readonly SoundStyle sizzleSound = new SoundStyle("JoJoStands/Sounds/GameSounds/Echoes_Sizzle") { Volume = 0.9f };

        private List<VisualSoundEffect> visualEffects = new List<VisualSoundEffect>();

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 6;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 2100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public struct VisualSoundEffect
        {
            public int duration;
            public int lifeStart;
            public Vector2 position;
            public int extraInfo1;
            public int extraInfo2;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.owner == Main.myPlayer)
                targetPosition = Main.MouseWorld;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (damageTimer > 0)
                damageTimer--;

            if (off > 0)
                off--;
            if (off == 0)
                Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage = 2;

            if (off == 0 && Projectile.alpha < 255)
                Projectile.alpha++;

            mPlayer.echoesTailTip = Projectile.whoAmI;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frame = 1;

            boingFrameCounter++;
            if (boingFrameCounter >= 6)
            {
                boingFrame += 1;
                boingFrameCounter = 0;
                if (boingFrame >= 4)
                    boingFrame = 0;
            }

            kaboomFrameCounter++;
            if (kaboomFrameCounter >= 4)
            {
                kaboomFrame += 1;
                kaboomFrameCounter = 0;
                if (kaboomFrame >= 5)
                    kaboomFrame = 0;
            }

            visualEffectTimer++;
            int echoesTailTipStage = Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage;
            if (echoesTailTipStage == 0)
            {
                if (Vector2.Distance(Projectile.Center, targetPosition) <= 10f)
                {
                    Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage = 1;
                    Projectile.velocity *= 0f;
                    Projectile.netUpdate = true;
                }

                if (Main.rand.Next(0, 1 + 1) == 0 && Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 0)
                {
                    int echoesTailTipType = Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType;
                    if (echoesTailTipType == Effect_Boing)
                        DustTrail(DustID.PinkTorch);
                    else if (echoesTailTipType == Effect_Kabooom)
                        DustTrail(DustID.PurpleTorch);
                    else if (echoesTailTipType == Effect_Wooosh)
                        DustTrail(DustID.IceTorch);
                    else if (echoesTailTipType == Effect_Sizzle)
                        DustTrail(DustID.RedTorch);
                }
            }
            else if (echoesTailTipStage == 1)
            {
                Projectile.frame = 0;
                int echoesTailTipType = Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType;
                if (echoesTailTipType == Effect_Boing)
                {
                    DustSpawn(DustID.PinkTorch, BoingEffectDistance);

                    visualSoundEffectSpawnTimer++;
                    if (visualSoundEffectSpawnTimer >= 120)
                    {
                        visualSoundEffectSpawnTimer = 0;
                        Vector2 spawnPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-BoingEffectDistance, BoingEffectDistance + 1), Main.rand.NextFloat(-BoingEffectDistance, BoingEffectDistance + 1));
                        if (Vector2.Distance(Projectile.Center, spawnPosition) <= BoingEffectDistance)
                        {
                            VisualSoundEffect boingEffect = new VisualSoundEffect
                            {
                                lifeStart = visualEffectTimer,
                                duration = Main.rand.Next(80, 120 + 1),
                                position = spawnPosition,
                                extraInfo1 = Main.rand.Next(0, 3 + 1),
                                extraInfo2 = Main.rand.Next(0, 360)
                            };
                            visualEffects.Add(boingEffect);
                            SoundEngine.PlaySound(boingSound);
                        }
                    }

                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= BoingEffectDistance && !npc.noTileCollide)
                        {
                            if ((npc.velocity.Y > 1f || npc.velocity.Y < -1f) && npc.collideY)
                            {
                                npc.velocity = npc.position - new Vector2(npc.position.X, npc.position.Y - 100f);
                                npc.velocity.Normalize();
                                npc.velocity.Y *= -16f;
                                npc.netUpdate = true;
                                SoundEngine.PlaySound(boing2Sound);
                            }
                        }
                    }
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= BoingEffectDistance)
                    {
                        if (player.velocity.Y > 1f || player.velocity.Y < -1f)
                            mPlayer.echoesBoing = true;
                        if (mPlayer.echoesBoing && mPlayer.collideY)
                        {
                            mPlayer.echoesBoing2 = true;
                            player.velocity = player.position - new Vector2(player.position.X, player.position.Y - 100f);
                            player.velocity.Normalize();
                            player.velocity.Y *= -24f;
                            SoundEngine.PlaySound(boing2Sound);
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player players = Main.player[p];
                            MyPlayer mPlayers = players.GetModPlayer<MyPlayer>();
                            players.noFallDmg = true;
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= BoingEffectDistance)
                            {
                                if (players.velocity.Y > 1f || players.velocity.Y < -1f)
                                    mPlayers.echoesBoing = true;
                                if (mPlayers.echoesBoing && mPlayers.collideY)
                                {
                                    mPlayers.echoesBoing2 = true;
                                    players.velocity = players.position - new Vector2(players.position.X, players.position.Y - 100f);
                                    players.velocity.Normalize();
                                    players.velocity.Y *= -24f;
                                    SoundEngine.PlaySound(boing2Sound);
                                }
                            }
                        }
                    }
                }
                else if (echoesTailTipType == Effect_Kabooom)
                {
                    DustSpawn(DustID.PurpleTorch, KabooomEffectDistance);

                    visualSoundEffectSpawnTimer++;
                    if (visualSoundEffectSpawnTimer >= 45)
                    {
                        visualSoundEffectSpawnTimer = 0;
                        Vector2 spawnPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-KabooomEffectDistance, KabooomEffectDistance + 1), Main.rand.NextFloat(-KabooomEffectDistance, KabooomEffectDistance + 1));
                        if (Vector2.Distance(Projectile.Center, spawnPosition) <= KabooomEffectDistance)
                        {
                            VisualSoundEffect kaboomEffect = new VisualSoundEffect
                            {
                                lifeStart = visualEffectTimer,
                                duration = Main.rand.Next(40, 80 + 1),
                                position = spawnPosition
                            };
                            visualEffects.Add(kaboomEffect);
                            SoundEngine.PlaySound(SoundID.Item62, spawnPosition);

                            for (int i = 0; i < 35; i++)
                            {
                                float angle = (360f / 35f) * i;
                                Vector2 dustPosition = spawnPosition + (angle.ToRotationVector2() * 8f);
                                Vector2 dustVelocity = dustPosition - spawnPosition;
                                dustVelocity.Normalize();
                                dustVelocity *= 9f;
                                int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID.PurpleTorch, dustVelocity.X, dustVelocity.Y, Scale: 2.5f);
                                Main.dust[dustIndex].noGravity = true;
                            }
                        }
                    }

                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= KabooomEffectDistance && !npc.noTileCollide)
                        {
                            if (npc.collideY && !npc.GetGlobalNPC<JoJoGlobalNPC>().echoesKaboom)
                            {
                                npc.GetGlobalNPC<JoJoGlobalNPC>().echoesKaboom = true;
                                npc.velocity = npc.position - new Vector2(npc.position.X, npc.position.Y - 100f);
                                npc.velocity.Normalize();
                                npc.velocity.Y *= -16f;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= KabooomEffectDistance)
                    {
                        if (mPlayer.collideY)
                        {
                            if (mPlayer.echoesKaboom3 > 0)
                                mPlayer.echoesKaboom3--;
                            if (mPlayer.echoesKaboom3 == 0)
                            {
                                mPlayer.echoesKaboom = true;
                                player.velocity = player.position - new Vector2(player.position.X, player.position.Y - 100f);
                                player.velocity.Normalize();
                                player.velocity.Y *= -24f;
                            }
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player players = Main.player[p];
                            MyPlayer mPlayers = players.GetModPlayer<MyPlayer>();
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= KabooomEffectDistance)
                            {
                                if (mPlayers.collideY)
                                {
                                    if (mPlayers.echoesKaboom3 > 0)
                                        mPlayers.echoesKaboom3--;
                                    if (mPlayers.echoesKaboom3 == 0)
                                    {
                                        mPlayers.echoesKaboom = true;
                                        players.velocity = players.position - new Vector2(players.position.X, players.position.Y - 100f);
                                        players.velocity.Normalize();
                                        players.velocity.Y *= -24f;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (echoesTailTipType == Effect_Wooosh)
                {
                    DustSpawn(DustID.IceTorch, WoooshEffectDistance);
                    if (!SoundEngine.TryGetActiveSound(soundSlotID, out activeSound))
                    {
                        soundSlotID = SoundEngine.PlaySound(woooshSound);
                    }
                    else
                    {
                        if (!activeSound.IsPlaying)
                            soundSlotID = SoundEngine.PlaySound(woooshSound);
                    }

                    visualSoundEffectSpawnTimer++;
                    if (visualSoundEffectSpawnTimer >= 45)
                    {
                        visualSoundEffectSpawnTimer = 0;
                        Vector2 spawnPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-WoooshEffectDistance, WoooshEffectDistance + 1), Main.rand.NextFloat(-WoooshEffectDistance, WoooshEffectDistance + 1));
                        if (Vector2.Distance(Projectile.Center, spawnPosition) <= WoooshEffectDistance)
                        {
                            VisualSoundEffect woooshEffect = new VisualSoundEffect
                            {
                                lifeStart = visualEffectTimer,
                                duration = Main.rand.Next(120, 180 + 1),
                                position = spawnPosition,
                                extraInfo1 = Main.rand.Next(0, 1 + 1) == 1 ? -1 : 1,
                                extraInfo2 = Main.rand.Next(-16, 16 + 1)
                            };
                            visualEffects.Add(woooshEffect);
                        }
                    }

                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && !npc.townNPC && Vector2.Distance(Projectile.Center, npc.Center) <= WoooshEffectDistance)
                            npc.AddBuff(ModContent.BuffType<WhooshDebuff>(), 600);
                    }
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= WoooshEffectDistance)
                        player.AddBuff(ModContent.BuffType<Whoosh>(), 600);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player players = Main.player[p];
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= WoooshEffectDistance)
                            {
                                if (players.hostile && player.hostile && player.InOpposingTeam(players))
                                    players.AddBuff(ModContent.BuffType<WhooshDebuff>(), 300);
                                if (!players.hostile || !player.hostile || players.hostile && player.hostile && !player.InOpposingTeam(players))
                                    players.AddBuff(ModContent.BuffType<Whoosh>(), 600);
                            }
                        }
                    }

                    for (int i = 0; i < visualEffects.Count; i++)
                    {
                        VisualSoundEffect woooshEffect = visualEffects[i];
                        woooshEffect.position.X += woooshEffect.extraInfo1 * (woooshEffect.extraInfo2 / 10f);

                        if (visualEffectTimer - woooshEffect.lifeStart >= woooshEffect.duration)
                            visualEffects.Remove(woooshEffect);
                        else
                            visualEffects[i] = woooshEffect;
                    }
                }
                else if (echoesTailTipType == Effect_Sizzle)
                {
                    DustSpawn(DustID.RedTorch, SizzleEffectDistance);
                    if (!SoundEngine.TryGetActiveSound(soundSlotID, out activeSound))
                    {
                        soundSlotID = SoundEngine.PlaySound(sizzleSound);
                    }
                    else
                    {
                        if (!activeSound.IsPlaying)
                            soundSlotID = SoundEngine.PlaySound(sizzleSound);
                    }

                    visualSoundEffectSpawnTimer++;
                    if (visualSoundEffectSpawnTimer >= 40)
                    {
                        visualSoundEffectSpawnTimer = 0;
                        Vector2 spawnPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-SizzleEffectDistance, SizzleEffectDistance + 1), Main.rand.NextFloat(-SizzleEffectDistance, SizzleEffectDistance + 1));
                        if (Vector2.Distance(Projectile.Center, spawnPosition) <= SizzleEffectDistance)
                        {
                            VisualSoundEffect sizzleEffect = new VisualSoundEffect
                            {
                                lifeStart = visualEffectTimer,
                                duration = Main.rand.Next(60, 120 + 1),
                                position = spawnPosition
                            };
                            visualEffects.Add(sizzleEffect);
                        }
                    }

                    if (damageTimer == 0)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal && !npc.townNPC && Vector2.Distance(Projectile.Center, npc.Center) <= SizzleEffectDistance)
                            {
                                bool crit = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
                                int defence = crit ? 4 : 2;
                                int damage1 = 0;
                                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 3)
                                    damage1 = 44;
                                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 4)
                                    damage1 = 68;
                                int damage2 = (int)Main.rand.NextFloat((int)(damage1 * mPlayer.standDamageBoosts * 0.85f), (int)(damage1 * mPlayer.standDamageBoosts * 1.15f)) + npc.defense / defence;
                                npc.StrikeNPC(damage2, 0f, 0, crit);
                                npc.AddBuff(BuffID.OnFire, 600);
                                if (mPlayer.crackedPearlEquipped)
                                {
                                    if (Main.rand.Next(1, 100 + 1) >= 60)
                                        npc.AddBuff(ModContent.BuffType<Infected>(), 10 * 60);
                                }
                                if (mPlayer.echoesTier == 3)
                                {
                                    if (npc.type == NPCID.Golem || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead)
                                    {
                                        if (crit)
                                            mPlayer.echoesACT3EvolutionProgress += damage2 * 2;
                                        else
                                            mPlayer.echoesACT3EvolutionProgress += damage2;
                                    }
                                }
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                Player players = Main.player[p];
                                if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= SizzleEffectDistance)
                                {
                                    if (players.hostile && player.hostile && player.InOpposingTeam(players))
                                    {
                                        if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 3)
                                            players.Hurt(PlayerDeathReason.ByCustomReason(players.name + " could no longer live."), (int)Main.rand.NextFloat((int)(22 * mPlayer.standDamageBoosts * 0.85f), (int)(22 * mPlayer.standDamageBoosts * 1.15f)) + players.statDefense, 0, true, false, false);
                                        if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 4)
                                            players.Hurt(PlayerDeathReason.ByCustomReason(players.name + " could no longer live."), (int)Main.rand.NextFloat((int)(34 * mPlayer.standDamageBoosts * 0.85f), (int)(34 * mPlayer.standDamageBoosts * 1.15f)) + players.statDefense, 0, true, false, false);
                                        players.AddBuff(BuffID.OnFire, 300);
                                        if (mPlayer.crackedPearlEquipped)
                                        {
                                            if (Main.rand.Next(1, 100 + 1) >= 60)
                                                players.AddBuff(ModContent.BuffType<Infected>(), 5 * 60);
                                        }
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < visualEffects.Count; i++)
                        {
                            VisualSoundEffect sizzleEffect = visualEffects[i];
                            sizzleEffect.position.Y -= 0.46f;

                            if (visualEffectTimer - sizzleEffect.lifeStart >= sizzleEffect.duration)
                                visualEffects.Remove(sizzleEffect);
                            else
                                visualEffects[i] = sizzleEffect;
                        }
                        damageTimer = 10;
                    }
                }
            }
            else if (echoesTailTipStage == 2)
            {
                Projectile.tileCollide = true;
                Projectile.frame = 1;
                Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.position.Y - 100f);
                Projectile.velocity.Normalize();
                Projectile.velocity.Y *= 9f;
                Projectile.netUpdate = true;

                if (SoundEngine.TryGetActiveSound(soundSlotID, out activeSound))
                    activeSound.Stop();
            }
            if (player.dead || player.ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] >= 2)
                Projectile.Kill();
        }

        private void DustSpawn(int dustID, float effectDistance)
        {
            int amountOfDust = Main.rand.Next(1, 5);
            for (int d = 0; d < amountOfDust; d++)
            {
                Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-effectDistance, effectDistance + 1), Main.rand.NextFloat(-effectDistance, effectDistance + 1));
                if (Vector2.Distance(Projectile.Center, dustPosition) > effectDistance)
                    continue;

                int dustIndex = Dust.NewDust(dustPosition, 1, 1, dustID);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].fadeIn = 2f;
            }
        }

        private void DustTrail(int DustID)
        {
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].fadeIn = 2f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage);
            writer.Write(Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType);
            writer.Write(Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage = reader.ReadInt32();
            Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType = reader.ReadInt32();
            Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier = reader.ReadInt32();
        }

        public override void PostDraw(Color lightColor)
        {
            if (Projectile.frame == 0)
            {
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == Effect_Boing)
                {
                    for (int i = 0; i < visualEffects.Count; i++)
                    {
                        VisualSoundEffect boingEffect = visualEffects[i];
                        if (visualEffectTimer - boingEffect.lifeStart >= boingEffect.duration)
                        {
                            visualEffects.Remove(boingEffect);
                            i--;
                            continue;
                        }

                        float alpha = (float)Math.Sin(((visualEffectTimer - boingEffect.lifeStart) / (float)boingEffect.duration) * Math.PI);
                        float scale = boingEffect.duration / 100f;

                        Texture2D boingTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_BOING").Value;
                        int frame = (boingFrame + boingEffect.extraInfo1) % 4;
                        Rectangle sourceRect = new Rectangle(0, frame * 44, 62, 44);

                        Main.spriteBatch.Draw(boingTexture, boingEffect.position - Main.screenPosition + new Vector2(0f, (float)Math.Sin(MathHelper.ToRadians(visualEffectTimer * 8) + boingEffect.extraInfo2)), sourceRect, Color.White * alpha, 0f, BoingOrigin, scale, SpriteEffects.None, 0);
                    }
                }
                else if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == Effect_Kabooom)
                {
                    for (int i = 0; i < visualEffects.Count; i++)
                    {
                        VisualSoundEffect kaboomEffect = visualEffects[i];
                        if (visualEffectTimer - kaboomEffect.lifeStart >= kaboomEffect.duration)
                        {
                            visualEffects.Remove(kaboomEffect);
                            i--;
                            continue;
                        }

                        float alpha = 1f - ((visualEffectTimer - kaboomEffect.lifeStart) / (float)kaboomEffect.duration);
                        float scale = 1f - ((visualEffectTimer - kaboomEffect.lifeStart) / (float)kaboomEffect.duration);
                        scale *= 1.4f;

                        int range = (int)(5f * alpha);
                        Vector2 positionOffset = new Vector2(Main.rand.Next(-range, range + 1), Main.rand.Next(-range, range + 1));
                        Texture2D kaboomTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_Kaboom").Value;
                        int frame = (visualEffectTimer / 5) % 5;
                        Rectangle sourceRect = new Rectangle(0, frame * 26, 50, 26);

                        Main.spriteBatch.Draw(kaboomTexture, kaboomEffect.position + positionOffset - Main.screenPosition, sourceRect, Color.White * alpha, 0f, KaboomOrigin, scale, SpriteEffects.None, 0);
                    }
                }
                else if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == Effect_Wooosh)
                {
                    for (int i = 0; i < visualEffects.Count; i++)
                    {
                        VisualSoundEffect woooshEffect = visualEffects[i];
                        if (visualEffectTimer - woooshEffect.lifeStart >= woooshEffect.duration)
                        {
                            visualEffects.Remove(woooshEffect);
                            i--;
                            continue;
                        }

                        float alpha = (float)Math.Sin(((visualEffectTimer - woooshEffect.lifeStart) / (float)woooshEffect.duration) * Math.PI);
                        float scale = woooshEffect.duration / 140f;

                        Texture2D woooshTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_Wooosh").Value;
                        Main.spriteBatch.Draw(woooshTexture, woooshEffect.position - Main.screenPosition, null, Color.White * alpha, 0f, woooshTexture.Size() / 2f, scale, SpriteEffects.None, 0);
                    }
                }
                else if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == Effect_Sizzle)
                {
                    for (int i = 0; i < visualEffects.Count; i++)
                    {
                        VisualSoundEffect sizzleEffect = visualEffects[i];
                        float alpha = (float)Math.Sin(((visualEffectTimer - sizzleEffect.lifeStart) / (float)sizzleEffect.duration) * Math.PI);
                        float scale = sizzleEffect.duration / 90f;

                        Texture2D sizzleTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Echoes_Sizzle").Value;
                        Main.spriteBatch.Draw(sizzleTexture, sizzleEffect.position - Main.screenPosition, null, Color.White * alpha, 0f, sizzleTexture.Size() / 2f, scale, SpriteEffects.None, 0);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override bool CanHitPvp(Player target) => false;
        public override bool? CanHitNPC(NPC target) => false;

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.echoesTailTip = -1;
        }
    }
}