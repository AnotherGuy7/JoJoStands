using JoJoStands.Buffs.Debuffs;
using JoJoStands.Buffs.EffectBuff;
using JoJoStands.DropConditions;
using JoJoStands.Items;
using JoJoStands.Items.Accessories;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Tiles;
using JoJoStands.NPCs.TownNPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace JoJoStands.NPCs
{
    public class JoJoGlobalNPC : GlobalNPC
    {
        public bool frozenInTime = false;
        public bool affectedbyBtz = false;
        public bool taggedByButterfly = false;
        public bool applyingForesightPositions = false;
        public bool foresightResetIndex = false;
        public bool taggedWithPhantomMarker = false;
        public bool grabbedByHermitPurple = false;
        public bool taggedByKillerQueen = false;
        public bool stunnedByBindingEmerald = false;
        public bool highlightedByTheHandMarker = false;
        public bool boundByStrings = false;
        public int foresightSaveTimer = 0;
        public int foresightPositionIndex = 0;
        public int foresightPositionIndexMax = 0;
        public int btZSaveTimer = 0;
        public int btzTotalRewindTime = 0;
        public int btzTotalRewindTimer = 0;
        public int aiStyleSave = 0;
        public int lifeRegenIncrement = 0;
        public int lockRegenCounter = 0;
        public bool forceDeath = false;
        public int btzPositionIndex = 0;
        public bool taggedForDeathLoop = false;
        public bool spawnedByDeathLoop = false;
        public int deathTimer = 0;
        public int bindingEmeraldDurationTimer = 0;
        public float kingCrimsonDonutMultiplier = 1f;
        public Vector2 playerPositionOnSkip = Vector2.Zero;
        public Vector2 preTimestopVelocity = Vector2.Zero;
        public Vector2[] BtZPositions = new Vector2[400];
        public ForesightSaveData[] foresightData = new ForesightSaveData[120];

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public struct ForesightSaveData
        {
            public Vector2 position;
            public Rectangle frame;
            public int direction;
            public float rotation;
        }

        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            globalLoot.Add(ItemDropRule.ByCondition(new SunDropletCondition(), ModContent.ItemType<SunDroplet>(), 10, 1, 3));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToFightCondition(), ModContent.ItemType<WillToFight>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToProtectCondition(), ModContent.ItemType<WillToProtect>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToChangeCondition(), ModContent.ItemType<WillToChange>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToControlCondition(), ModContent.ItemType<WillToControl>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToDestroyCondition(), ModContent.ItemType<WillToDestroy>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new WillToEscapeCondition(), ModContent.ItemType<WillToEscape>(), 14));

            globalLoot.Add(ItemDropRule.ByCondition(new SoulOfTimeCondition(), ModContent.ItemType<SoulofTime>(), 14));


            /*IItemDropRule sunDropletRule = new LeadingConditionRule(new SunDropletCondition());
            sunDropletRule.OnSuccess(new CommonDrop(ModContent.ItemType<SunDroplet>(), 10, 1, 3));
            globalLoot.Add(sunDropletRule);

            IItemDropRule willToFightRule = new LeadingConditionRule(new WillToFightCondition());
            willToFightRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToFight>(), 14));
            globalLoot.Add(willToFightRule);

            IItemDropRule willToProtectRule = new LeadingConditionRule(new WillToProtectCondition());
            willToProtectRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToProtect>(), 14));
            globalLoot.Add(willToProtectRule);

            IItemDropRule willToDestroyRule = new LeadingConditionRule(new WillToDestroyCondition());
            willToDestroyRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToDestroy>(), 14));
            globalLoot.Add(willToDestroyRule);

            IItemDropRule willToControlRule = new LeadingConditionRule(new WillToControlCondition());
            willToControlRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToControl>(), 14));
            globalLoot.Add(willToControlRule);

            IItemDropRule willToEscapeRule = new LeadingConditionRule(new WillToEscapeCondition());
            willToEscapeRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToEscape>(), 14));
            globalLoot.Add(willToEscapeRule);

            IItemDropRule willToChangeRule = new LeadingConditionRule(new WillToChangeCondition());
            willToChangeRule.OnSuccess(new CommonDrop(ModContent.ItemType<WillToChange>(), 14));
            globalLoot.Add(willToChangeRule);

            IItemDropRule soulOfTimeRule = new LeadingConditionRule(new SoulOfTimeCondition());
            soulOfTimeRule.OnSuccess(new CommonDrop(ModContent.ItemType<SoulofTime>(), 14));
            globalLoot.Add(soulOfTimeRule);*/
        }

        /*public override bool SpecialNPCLoot(NPC npc)
        {
            if (taggedByButterfly)      //increases the drop chances of loot by calling it again when called, cause it's gonna normally call NPCLoot and call it again here
            {
                npc.NPCLoot();
                npc.value = 0;
            }
            return base.SpecialNPCLoot(npc);
        }*/

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.WallofFlesh)
            {
                npcLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<StandEmblem>(), ModContent.ItemType<HamonEmblem>()));
            }
            if (npc.type == NPCID.Zombie || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinScout || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinTinkerer || npc.type == NPCID.GoblinWarrior || npc.townNPC)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Hand>(), 25));
            }
        }


        public override void GetChat(NPC npc, ref string chat)
        {
            if (MyPlayer.SecretReferences)
            {
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 3)      //Placement contributor reference
                {
                    chat = "I knew a guy who loved to name things. He’s still around, and he’s probably still naming everything he can find. I wonder what kind of Placement he went through in the dimension shift...";
                }
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 3)      //Nekro contributor reference             
                {
                    //Removed at Nekro's behest for reasons described as "cringe" and "bad"
                    //chat = "There was a man with splt personalities named Nekro and Sektor, they named about 10 of the stands you have access to, kinda reminds me of a friend from Egypt...";
                    chat = "Did you know animals can develop stands too? I even once saw an eagle who got struck with the arrow become humanoid once it developed a stand capable of turning people to glass. I even managed to teach it my old cigarette trick.";
                }
                if (npc.type == ModContent.NPCType<MarineBiologist>() && Main.rand.Next(0, 101) <= 5)      //Techno contributor reference
                {
                    chat = "Some weirdo with an afro once zoomed past me at the speed of a train, with his Stand carrying him in a box. Gramps seemed to approve. Who knows where that lunatic is now.";
                }
                if (npc.type == NPCID.Nurse && Main.rand.Next(0, 100) <= 5)     //ciocollata reference
                {
                    chat = "I heard there was a surgeon fired for killing patients and recording it, there are some sick people in this world.";
                }
                if (npc.type == NPCID.Demolitionist && Main.rand.Next(0, 100) <= 10)        //obviously, a Killer Queen reference
                {
                    chat = Main.LocalPlayer.name + " do you know what a 'Killer Queen' is? I heard it can make anything explode...";
                }
                if (npc.type == NPCID.Guide && Main.rand.Next(0, 100) <= 4)     //Betty contributor reference
                {
                    chat = "Hey " + Main.LocalPlayer.name + ", the other day one small girl calling herself 'The Dead Princess' came to me asking for a new name to her list... I'm not sure what was she talking about...";
                }
                if (npc.type == NPCID.Mechanic && Main.rand.Next(0, 100) <= 5)      //Phil contributer reference
                {
                    chat = "I've heard of someone named Phil selling something called 'Flex Tape' that can fix everything, mind if you get some for me?";
                }
                if (npc.type == NPCID.Mechanic && Main.rand.Next(0, 100) <= 5)      //Archerous contributer reference
                {
                    chat = "Have the lights been acting weird lately? If I didn't know better, I'd say Archerous has something to do with this. That 'Lucy in the Sky' of his has been causing me some real trouble!";
                }
                if (npc.type == NPCID.Cyborg && Main.rand.Next(0, 100) <= 5)      //AG contributer reference
                {
                    chat = "Hey, if you ever see me acting weird, just tie me up and lock me inside of my home. I hear there's someone called 'A. Guy' or something of the sort looking to mess with me.";
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.TravellingMerchant && ((Main.hardMode && Main.rand.Next(0, 101) >= 90) || NPC.downedPlantBoss))
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ViralPearlRing>());
                nextSlot++;
            }
            if (type == NPCID.Painter)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<IWouldntLose>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<OfficersRegret>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<QuietLife>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ShotintheDark>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<BloodForTheKing>());
                nextSlot++;
            }
        }

        public override bool PreAI(NPC npc)
        {
            MyPlayer player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            if (player.timestopActive || frozenInTime)
            {
                if (npc.velocity != Vector2.Zero)
                {
                    preTimestopVelocity = npc.velocity;
                }
                npc.velocity = Vector2.Zero;
                npc.frameCounter = 1;
                if (!npc.noGravity)
                {
                    npc.velocity.Y -= 0.3f;     //the default gravity value, so that if enemies have gravity enabled, this velocity counters that gravity
                }
                npc.netUpdate = true;
                return false;
            }
            if (player.backToZeroActive)
            {
                if (!affectedbyBtz)
                {
                    btZSaveTimer--;
                    if (btZSaveTimer <= 0)
                    {
                        if (btzPositionIndex < BtZPositions.Length)
                        {
                            btzPositionIndex += 1;
                            BtZPositions[btzPositionIndex] = npc.position;
                            btZSaveTimer = 30;
                        }
                    }
                }
                if (affectedbyBtz)
                {
                    if (btzTotalRewindTimer > 0)
                    {
                        btzTotalRewindTimer--;
                    }

                    btZSaveTimer--;
                    if (btZSaveTimer <= 0)
                    {
                        if (btzPositionIndex > 1)
                        {
                            btzPositionIndex -= 1;
                            npc.position = BtZPositions[btzPositionIndex];
                            if (npc.position == BtZPositions[btzPositionIndex])
                            {
                                Array.Clear(BtZPositions, btzPositionIndex, 1);
                            }
                            btZSaveTimer = 5;
                        }
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(BuffID.Confused, 600);
                    return false;
                }
            }
            else
            {
                if (npc.HasBuff(ModContent.BuffType<AffectedByBtZ>()))
                {
                    int buffIndex = npc.FindBuffIndex(ModContent.BuffType<AffectedByBtZ>());
                    npc.DelBuff(buffIndex);
                }
                btZSaveTimer = 0;
                btzPositionIndex = 0;
                btzTotalRewindTime = 0;
                btzTotalRewindTimer = 0;
                affectedbyBtz = false;
            }
            if (player.timeskipPreEffect)
            {
                aiStyleSave = 0;
                playerPositionOnSkip = Vector2.Zero;
            }
            if (player.timeskipActive && !npc.townNPC && !npc.friendly && !npc.boss && !npc.immortal)
            {
                if (playerPositionOnSkip == Vector2.Zero)
                {
                    playerPositionOnSkip = Main.player[PreTimeSkip.userIndex].position;
                }
                if (aiStyleSave == 0 && npc.aiStyle != 0)
                {
                    aiStyleSave = npc.aiStyle;
                    npc.aiStyle = 0;
                }
                if (npc.aiStyle == 0)
                {
                    npc.velocity /= 2;
                    if (npc.direction == -1)
                    {
                        npc.spriteDirection = 1;
                    }
                    if (npc.direction == 1)
                    {
                        npc.spriteDirection = -1;
                    }
                    if (playerPositionOnSkip.X > npc.position.X)
                    {
                        npc.velocity.X = 1f;
                        npc.direction = 1;
                    }
                    if (playerPositionOnSkip.X < npc.position.X)
                    {
                        npc.velocity.X = -1f;
                        npc.direction = -1;
                    }
                    if (npc.noGravity)
                    {
                        if (playerPositionOnSkip.Y > npc.position.Y)
                        {
                            npc.velocity.X = -1f;
                        }
                        if (playerPositionOnSkip.Y < npc.position.Y)
                        {
                            npc.velocity.X = 1f;
                        }
                    }
                    if (!npc.noGravity)
                    {
                        int tilesInFront = (int)((npc.Center.X + (float)(15 * npc.direction)) / 16f);
                        int tilesUnder = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)) && !Collision.SolidTilesVersatile(tilesInFront - npc.direction * 2, tilesInFront - npc.direction, tilesUnder - 5, tilesUnder - 1) && !Collision.SolidTiles(tilesInFront, tilesInFront, tilesUnder - 5, tilesUnder - 3) && npc.ai[1] == 0f)
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                    }
                }
                return false;
            }
            if (player.timeskipActive && npc.boss)
            {
                npc.defense /= 2;
            }
            if (!player.timeskipActive && npc.aiStyle == 0)
            {
                playerPositionOnSkip = Vector2.Zero;
                npc.aiStyle = aiStyleSave;
                aiStyleSave = 0;
            }
            if (player.epitaphForesightActive && !npc.immortal)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    foresightData[foresightPositionIndex] = new ForesightSaveData();
                    foresightData[foresightPositionIndex].position = npc.position;
                    foresightData[foresightPositionIndex].frame = npc.frame;
                    foresightData[foresightPositionIndex].rotation = npc.rotation;
                    foresightData[foresightPositionIndex].direction = npc.direction;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 5;
                    if (foresightPositionIndex >= foresightData.Length)
                    {
                        foresightPositionIndex--;
                        foresightPositionIndexMax--;
                    }
                }
            }
            if (!player.epitaphForesightActive && applyingForesightPositions)
            {
                if (!foresightResetIndex)
                {
                    foresightPositionIndex = 0;
                    foresightResetIndex = true;
                }
                npc.velocity = Vector2.Zero;
                npc.position = foresightData[foresightPositionIndex].position;
                npc.frame = foresightData[foresightPositionIndex].frame;
                npc.rotation = foresightData[foresightPositionIndex].rotation;
                npc.direction = (int)foresightData[foresightPositionIndex].direction;
                if (foresightSaveTimer > 0)
                    foresightSaveTimer--;

                if (foresightSaveTimer <= 0)
                {
                    if (foresightData[foresightPositionIndex].position != Vector2.Zero)
                        foresightData[foresightPositionIndex].position = Vector2.Zero;
                    if (foresightData[foresightPositionIndex].rotation != 0f)
                        foresightData[foresightPositionIndex].rotation = 0f;
                    if (foresightData[foresightPositionIndex].direction != 0)
                        foresightData[foresightPositionIndex].direction = 0;
                    foresightPositionIndex++;
                    foresightSaveTimer = 5;
                }
                if (foresightPositionIndex >= foresightPositionIndexMax)
                {
                    applyingForesightPositions = false;
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                    foresightResetIndex = false;
                }
                /*if (foresightPositionIndex >= 49)       //a failsafe to prevent Index Out of Bounds in extended multiplayer timeskips
                {
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                }*/
                return false;
            }
            if (taggedByButterfly)
            {
                if (Main.rand.Next(0, 3 + 1) == 0)
                {
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, DustID.IchorTorch, SpeedY: Main.rand.NextFloat(-1.1f, -0.6f + 1f), Scale: Main.rand.NextFloat(1.1f, 2.4f + 1f));
                    Main.dust[dustIndex].noGravity = true;
                }
            }
            if (spawnedByDeathLoop)
            {
                deathTimer++;
                if ((deathTimer >= 30 && DeathLoop.Looping10x) || (deathTimer >= 60 && DeathLoop.Looping3x))
                {
                    if (npc.immortal || npc.hide)
                    {
                        npc.StrikeNPCNoInteraction(999999999, 0f, 1, noEffect: true);
                    }
                    if (!npc.immortal)
                    {
                        npc.StrikeNPCNoInteraction(npc.lifeMax + 10, 0f, 1, noEffect: true);
                    }
                    deathTimer = 0;
                }
            }
            if (npc.HasBuff(ModContent.BuffType<Locked>()))
            {
                npc.lifeRegen = -4;
                npc.velocity *= 0.95f;
                lockRegenCounter++;
                npc.defense = (int)(npc.defense * 0.95);
                if (lockRegenCounter >= 60)    //increases lifeRegen damage every second
                {
                    lockRegenCounter = 0;
                    lifeRegenIncrement += 2;
                    npc.StrikeNPC(lifeRegenIncrement, 0f, 1);
                }
            }

            if (npc.HasBuff(ModContent.BuffType<RedBindDebuff>()))
            {
                npc.velocity.X = 0f;
                if (npc.velocity.Y > 8f)
                    npc.velocity.Y += 0.3f;
                return false;
            }
            if (stunnedByBindingEmerald)
            {
                if (bindingEmeraldDurationTimer > 0)
                    bindingEmeraldDurationTimer--;
                else
                    stunnedByBindingEmerald = false;

                if (npc.boss)
                {
                    npc.velocity *= 0.8f;
                    return true;
                }

                npc.velocity.X = 0f;
                return false;
            }
            if (boundByStrings)
            {
                if (npc.boss)
                {
                    npc.velocity *= 0.6f;
                    return true;
                }

                npc.velocity.X = 0f;
                if (npc.velocity.Y > 8f)
                    npc.velocity.Y += 0.3f;
                return false;
            }
            if (npc.HasBuff(ModContent.BuffType<Stolen>()))
                return false;
            if (grabbedByHermitPurple)
                return false;
            return true;
        }

        public override void OnKill(NPC npc)
        {
            if (taggedForDeathLoop)
            {
                for (int p = 0; p < Main.maxPlayers; p++)       //Searches if any player has death loop on
                {
                    Player player = Main.player[p];
                    if (!player.active)
                        continue;

                    MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
                    if (npc.boss && mPlayer.deathLoopActive && DeathLoop.deathNPCType == 0)
                    {
                        DeathLoop.deathNPCType = npc.type;
                        DeathLoop.deathPosition = npc.position;
                        DeathLoop.Looping3x = true;
                        DeathLoop.Looping10x = false;
                    }
                    if (!npc.boss && mPlayer.deathLoopActive && DeathLoop.deathNPCType == 0 && !npc.friendly && npc.lifeMax > 5)
                    {
                        DeathLoop.deathNPCType = npc.type;
                        DeathLoop.deathPosition = npc.position;
                        DeathLoop.Looping3x = false;
                        DeathLoop.Looping10x = true;
                    }
                }
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.epitaphForesightActive || applyingForesightPositions)
            {
                for (int i = 0; i < foresightPositionIndexMax; i++)
                {
                    SpriteEffects effects = SpriteEffects.None;
                    if (foresightData[i].direction == 1)
                        effects = SpriteEffects.FlipHorizontally;

                    float alpha = Math.Abs(foresightPositionIndex - i) / 10f;
                    alpha = Math.Clamp(alpha, 0f, 1f);
                    if (foresightData[i].position != Vector2.Zero)
                        spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, foresightData[i].position - Main.screenPosition, foresightData[i].frame, Color.DarkRed * alpha, foresightData[i].rotation, Vector2.Zero, npc.scale, effects, 0f);
                }
            }
            if (player.backToZeroActive && btzTotalRewindTimer != 0)
            {
                for (int a = 0; a < BtZPositions.Length - 1; a++)
                {
                    float alpha = 1f / (Math.Abs((float)(btzTotalRewindTimer - (5 * a))) + 1f);
                    SpriteEffects spriteEffect = SpriteEffects.None;
                    if (npc.direction == 1)
                        spriteEffect = SpriteEffects.FlipHorizontally;

                    Vector2 position = BtZPositions[a] - Main.screenPosition;
                    spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, position, npc.frame, Color.White * alpha, npc.rotation, Vector2.Zero, npc.scale, spriteEffect, 0f);
                }
            }
            if (taggedByKillerQueen)
            {
                Texture2D bombTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/Bomb").Value;
                Vector2 position = npc.Center - new Vector2(bombTexture.Width / 2f, (npc.height / 2f) + 18f);
                spriteBatch.Draw(bombTexture, position - Main.screenPosition, Color.White);
            }
            return true;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (taggedForDeathLoop)
                drawColor = Color.Purple;

            if (highlightedByTheHandMarker)
            {
                highlightedByTheHandMarker = false;
                drawColor = Color.LightBlue;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<RedBindDebuff>()))
            {
                Texture2D texture = ModContent.Request<Texture2D>("JoJoStands/Extras/BoundByRedBind").Value;
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), npc.scale, SpriteEffects.None, 0);
            }
            if (stunnedByBindingEmerald)
            {
                Texture2D emeraldStringWebTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/EmeraldStringWeb").Value;
                int textureWidth = 19;
                int textureHeight = 19;

                Vector2 position = npc.Center - Main.screenPosition;
                Vector2 origin = new Vector2(textureWidth / 2f, textureHeight / 2f);
                float scale = npc.width;
                if (npc.height > npc.width)
                    scale = npc.height;

                scale -= 4f;
                scale /= (float)textureWidth;
                scale += (float)Math.Abs(Math.Sin(bindingEmeraldDurationTimer / 100f)) * 0.5f;
                spriteBatch.Draw(emeraldStringWebTexture, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0);
            }
            if (boundByStrings)
            {
                Texture2D bombTexture = ModContent.Request<Texture2D>("JoJoStands/Extras/BoundByStrings").Value;
                Vector2 scale = new Vector2(npc.width, npc.height) / new Vector2(22, 14);
                Vector2 origin = new Vector2(11, 7);
                spriteBatch.Draw(bombTexture, npc.Center - Main.screenPosition, null, npc.color, npc.rotation, origin, scale, SpriteEffects.None, 0);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            int standSlotType = mPlayer.StandSlot.SlotItem.type;
            if (target.HasBuff(ModContent.BuffType<BacktoZero>()))     //only affects the ones with the buff, everyone's bool should turn on and save positions normally
            {
                npc.AddBuff(ModContent.BuffType<AffectedByBtZ>(), 2);
                npc.StrikeNPC(damage, npc.knockBackResist, -npc.direction);
                btzTotalRewindTime = 5 * btzPositionIndex;
                btzTotalRewindTimer = 5 * btzPositionIndex;
            }
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT1>())
            {
                npc.StrikeNPC((int)(npc.damage * 0.35f), 4f, -npc.direction);       //Dolly dagger is reflecting 35% of damage here, 70% in tier 2
            }
            if (mPlayer.StandSlot.SlotItem.type == ModContent.ItemType<DollyDaggerT2>())
            {
                npc.StrikeNPC((int)(npc.damage * 0.7f), 6f, -npc.direction);
            }
            if (npc.boss)
            {
                if (standSlotType == ModContent.ItemType<LockT1>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 3 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockT2>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 6 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockT3>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 9 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockFinal>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 12 * 60);
                }
            }
            else
            {
                if (standSlotType == ModContent.ItemType<LockT1>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 5 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockT2>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 10 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockT3>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 15 * 60);
                }
                if (standSlotType == ModContent.ItemType<LockFinal>())
                {
                    npc.AddBuff(ModContent.BuffType<Locked>(), 20 * 60);
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (forceDeath)
            {
                npc.lifeRegen = -4;
            }
        }
    }
}