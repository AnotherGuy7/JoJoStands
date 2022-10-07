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

namespace JoJoStands.Projectiles
{
    public class EchoesTailTip : ModProjectile
    {
        private Vector2 targetPosition = Vector2.Zero;

        private int off = 1800;

        private int offsetDraw = 0;
        private int timerDraw = 0;

        private int damageTimer = 0;

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

            if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 0)
            {
                if (Vector2.Distance(Projectile.Center, targetPosition) <= 10f)
                {
                    Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage = 1;
                    Projectile.velocity *= 0f;
                    Projectile.netUpdate = true;
                }

                if (Main.rand.Next(0, 1 + 1) == 0 && Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 0)
                {
                    if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 4)
                        DustTrail(DustID.RedTorch, false);
                    if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 3)
                        DustTrail(DustID.IceTorch, false);
                    if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 2)
                        DustTrail(DustID.BlueTorch, false);
                    if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 1)
                        DustTrail(DustID.PinkTorch, false);
                }
            }
            if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 1)
            {
                Projectile.frame = 0;
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 4)
                {
                    DustSpawn(DustID.RedTorch);
                    if (damageTimer == 0)
                    {
                        for (int n = 0; n < Main.maxNPCs; n++)
                        {
                            NPC npc = Main.npc[n];
                            if (npc.active && !npc.hide && !npc.immortal && !npc.townNPC && Vector2.Distance(Projectile.Center, npc.Center) <= 120f)
                            {
                                bool crit = Main.rand.NextFloat(1, 100 + 1) <= mPlayer.standCritChangeBoosts;
                                int defence = 2;
                                if (crit)
                                    defence = 4;
                                else
                                    defence = 2;
                                int damage1 = 0;
                                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 3)
                                    damage1 = 44;
                                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 4)
                                    damage1 = 68;
                                int damage2 = (int)Main.rand.NextFloat((int)(damage1 * mPlayer.standDamageBoosts * 0.85f), (int)(damage1 * mPlayer.standDamageBoosts * 1.15f)) + npc.defense / defence;
                                npc.StrikeNPC(damage2, 0f, 0, crit);
                                npc.AddBuff(BuffID.OnFire, 600);

                                if (mPlayer.echoesTier == 3)
                                {
                                    if (npc.type == NPCID.Golem || npc.type == NPCID.GolemFistLeft || npc.type == NPCID.GolemFistRight || npc.type == NPCID.GolemHead)
                                    {
                                        if (crit)
                                            mPlayer.echoesACT3Evolve += damage2 * 2;
                                        if (!crit)
                                            mPlayer.echoesACT3Evolve += damage2;
                                    }
                                }
                            }
                        }
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            for (int p = 0; p < Main.maxPlayers; p++)
                            {
                                Player players = Main.player[p];
                                if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= 120f)
                                {
                                    if (players.hostile && player.hostile && player.InOpposingTeam(players))
                                    {
                                        if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 3)
                                            players.Hurt(PlayerDeathReason.ByCustomReason(players.name + " could no longer live."), (int)Main.rand.NextFloat((int)(22 * mPlayer.standDamageBoosts * 0.85f), (int)(22 * mPlayer.standDamageBoosts * 1.15f)) + players.statDefense, 0, true, false, false);
                                        if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipTier == 4)
                                            players.Hurt(PlayerDeathReason.ByCustomReason(players.name + " could no longer live."), (int)Main.rand.NextFloat((int)(34 * mPlayer.standDamageBoosts * 0.85f), (int)(34 * mPlayer.standDamageBoosts * 1.15f)) + players.statDefense, 0, true, false, false);
                                        players.AddBuff(BuffID.OnFire, 300);
                                    }
                                }
                            }
                        }
                        damageTimer = 10;
                    }
                }
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 3)
                {
                    DustSpawn(DustID.IceTorch);
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && !npc.townNPC && Vector2.Distance(Projectile.Center, npc.Center) <= 120f)
                            npc.AddBuff(ModContent.BuffType<WhooshDebuff>(), 600);
                    }
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= 120f)
                        player.AddBuff(ModContent.BuffType<Whoosh>(), 600);
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player players = Main.player[p];
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= 120f)
                            {
                                if (players.hostile && player.hostile && player.InOpposingTeam(players))
                                    players.AddBuff(ModContent.BuffType<WhooshDebuff>(), 300);
                                if (!players.hostile || !player.hostile || players.hostile && player.hostile && !player.InOpposingTeam(players))
                                    players.AddBuff(ModContent.BuffType<Whoosh>(), 600);
                            }
                        }
                    }
                }
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 2)
                {
                    DustSpawn(DustID.BlueTorch);
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= 120f && !npc.noTileCollide)
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
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= 120f)
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
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= 120f)
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
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 1)
                {
                    DustSpawn(DustID.PinkTorch);
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && !npc.hide && !npc.immortal && Vector2.Distance(Projectile.Center, npc.Center) <= 120f && !npc.noTileCollide)
                        {
                            if (npc.velocity.Y > 1f || npc.velocity.Y < -1f)
                                npc.GetGlobalNPC<JoJoGlobalNPC>().echoesBoing = true;
                            if (npc.GetGlobalNPC<JoJoGlobalNPC>().echoesBoing && npc.collideY)
                            {
                                npc.velocity = npc.position - new Vector2(npc.position.X, npc.position.Y - 100f);
                                npc.velocity.Normalize();
                                npc.velocity.Y *= -16f;
                                npc.netUpdate = true;
                            }
                        }
                    }
                    if (player.active && Vector2.Distance(Projectile.Center, player.Center) <= 120f)
                    {
                        if (player.velocity.Y > 1f || player.velocity.Y < -1f)
                            mPlayer.echoesBoing = true;
                        if (mPlayer.echoesBoing && mPlayer.collideY)
                        {
                            mPlayer.echoesBoing2 = true;
                            player.velocity = player.position - new Vector2(player.position.X, player.position.Y - 100f);
                            player.velocity.Normalize();
                            player.velocity.Y *= -24f;
                        }
                    }
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        for (int p = 0; p < Main.maxPlayers; p++)
                        {
                            Player players = Main.player[p];
                            MyPlayer mPlayers = players.GetModPlayer<MyPlayer>();
                            players.noFallDmg = true;
                            if (players.active && players.whoAmI != Projectile.owner && Vector2.Distance(Projectile.Center, players.Center) <= 120f)
                            {
                                if (players.velocity.Y > 1f || players.velocity.Y < -1f)
                                    mPlayers.echoesBoing = true;
                                if (mPlayers.echoesBoing && mPlayers.collideY)
                                {
                                    mPlayers.echoesBoing2 = true;
                                    players.velocity = players.position - new Vector2(players.position.X, players.position.Y - 100f);
                                    players.velocity.Normalize();
                                    players.velocity.Y *= -24f;
                                }
                            }
                        }
                    }
                }
            }
            if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipStage == 2)
            {
                Projectile.tileCollide = true;
                Projectile.frame = 1;
                Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.position.Y - 100f);
                Projectile.velocity.Normalize();
                Projectile.velocity.Y *= 9f;
                Projectile.netUpdate = true;
            }
            if (player.dead || player.ownedProjectileCounts[ModContent.ProjectileType<EchoesTailTip>()] >= 2)
                Projectile.Kill();
        }

        private void DustTrail(int DustID, bool light)
        {
            int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID);
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].noLight = light;
            Main.dust[dustIndex].fadeIn = 2f;
        }
        private void DustSpawn(int DustID)
        {
            for (int i = 0; i < Main.rand.Next(1, 5); i++)
            {
                Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.NextFloat(-120f, 120f), Main.rand.NextFloat(-120f, 120f));
                if (Vector2.Distance(Projectile.Center, dustPosition) > 120f)
                    continue;

                int dustIndex = Dust.NewDust(dustPosition, 1, 1, DustID);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].noLight = true;
                Main.dust[dustIndex].fadeIn = 2f;
            }
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
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 4)
                {
                    if (timerDraw == 0)
                        timerDraw = 25;
                    if (timerDraw == 25)
                        offsetDraw = 46;
                    if (timerDraw == 20)
                        offsetDraw = 92;
                    if (timerDraw == 15)
                        offsetDraw = 138;
                    if (timerDraw == 10)
                        offsetDraw = 184;
                    if (timerDraw == 5)
                        offsetDraw = 0;
                    if (timerDraw > 0)
                        timerDraw--;
                    Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Sizzle").Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 90) - Main.screenPosition, new Rectangle(0, offsetDraw, 68, 46), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                }
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 3)
                {
                    if (timerDraw == 0)
                        timerDraw = 15;
                    if (timerDraw == 15)
                        offsetDraw = 46;
                    if (timerDraw == 10)
                        offsetDraw = 92;
                    if (timerDraw == 5)
                        offsetDraw = 0;
                    if (timerDraw > 0)
                        timerDraw--;
                    Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/WOOOSH").Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center + new Vector2(0, 50) - Main.screenPosition, new Rectangle(0, offsetDraw, 60, 46), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                }
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 2)
                {
                    if (timerDraw == 0)
                        timerDraw = 40;
                    if (timerDraw == 40)
                        offsetDraw = 46;
                    if (timerDraw == 35)
                        offsetDraw = 92;
                    if (timerDraw == 30)
                        offsetDraw = 138;
                    if (timerDraw == 25)
                        offsetDraw = 184;
                    if (timerDraw == 20)
                        offsetDraw = 230;
                    if (timerDraw == 15)
                        offsetDraw = 276;
                    if (timerDraw == 10)
                        offsetDraw = 322;
                    if (timerDraw == 5)
                        offsetDraw = 0;
                    if (timerDraw > 0)
                        timerDraw--;
                    Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/Kaboom").Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, 160), new Rectangle(0, offsetDraw, 54, 46), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                }
                if (Projectile.GetGlobalProjectile<JoJoGlobalProjectile>().echoesTailTipType == 1)
                {
                    if (timerDraw == 0)
                        timerDraw = 20;
                    if (timerDraw == 20)
                        offsetDraw = 60;
                    if (timerDraw == 15)
                        offsetDraw = 120;
                    if (timerDraw == 10)
                        offsetDraw = 180;
                    if (timerDraw == 5)
                        offsetDraw = 0;
                    if (timerDraw > 0)
                        timerDraw--;
                    Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/BOING").Value;
                    Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, 90), new Rectangle(0, offsetDraw, 86, 60), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
            return false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.echoesTailTip = -1;
        }
    }
}