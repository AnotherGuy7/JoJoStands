using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Items;

namespace JoJoStands.NPCs
{
    public class JoJoGlobalNPC : GlobalNPC
    {
        public bool affectedbyBtz = false;
        public bool taggedByButterfly = false;
        public int BtZSaveTimer = 0;
        public int aiStyleSave = 0;
        public bool forceDeath = false;
        public int indexPosition = 0;
        public bool spawnedByDeathLoop = false;
        public int deathTimer = 0;
        public Vector2 playerPositionOnSkip = Vector2.Zero;
        public Vector2[] BtZPositions = new Vector2[999];

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore && Main.rand.NextFloat() < 0.0574f) //5.74% is .0574f chance
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RequiemArrow"));
            }

            if(Main.hardMode && Main.rand.NextFloat() < 0.0238f) //should be a 4.38% chance of dropping from any enemy in hardmode
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SoulofTime"), Main.rand.Next(1,3));      //mininum amount = 1, maximum amount = 3
            }

            if(Main.dayTime && Main.rand.NextFloat() < 0.0327f)     //should be a 3.27% chance of dropping from an enemy
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SunDroplet"), Main.rand.Next(1,3));     //Main.rand.Next counts zeroes as well. Now changed to a minimum value of 1 and a maximum value of 3.
            }
            if ((npc.type == NPCID.Bird || npc.type == NPCID.BirdBlue || npc.type == NPCID.BirdRed || npc.type == NPCID.GoldBird) && Main.rand.NextFloat() < 0.0301f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WrappedPicture"));
            }
            if ((npc.type == NPCID.Zombie || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinScout || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinTinkerer || npc.type == NPCID.GoblinWarrior || npc.townNPC) && Main.rand.NextFloat() < 0.0121f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("Hand"));
            }
        }

        public override bool SpecialNPCLoot(NPC npc)
        {
            if (taggedByButterfly)      //increases the drop chances of loot by calling it again when called, cause it's gonna normally call NPCLoot and call it again here
            {
                npc.NPCLoot();
                npc.value = 0;
            }
            return base.SpecialNPCLoot(npc);
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == NPCID.Nurse && Main.rand.Next(0, 100) <= 5)
            {
                chat = "I heard there was a surgeon fired for killing patients and recording it, there are some sick people in this world.";
            }
            if (npc.type == NPCID.Demolitionist && Main.rand.Next(0, 100) <= 10)
            {
                chat = Main.LocalPlayer.name + " do you know what a 'Killer Queen' is? I heard it can make anything explode...";
            }
        }

        public override bool PreAI(NPC npc)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.TheWorldEffect)
            {
                npc.velocity.X *= 0f;
                npc.velocity.Y *= 0f;               //negative X is to the left, negative Y is UP
                npc.frameCounter = 1;
                if (npc.noGravity == false)
                {
                    npc.velocity.Y -= 0.3f;     //the default gravity value, so that if enemies have gravity enabled, this velocity counters that gravity
                }
                npc.netUpdate = true;
                return false;
            }
            if (player.BackToZero)
            {
                if (!affectedbyBtz)
                {
                    BtZSaveTimer--;
                    if (BtZSaveTimer <= 0)
                    {
                        if (indexPosition < 999)
                        {
                            indexPosition += 1;
                            BtZPositions[indexPosition] = npc.position;
                            BtZSaveTimer = 30;
                        }
                    }
                }
                if (affectedbyBtz)
                {
                    BtZSaveTimer--;
                    if (BtZSaveTimer <= 0)
                    {
                        if (indexPosition > 1)
                        {
                            indexPosition -= 1;
                            npc.position = BtZPositions[indexPosition];
                            if (npc.position == BtZPositions[indexPosition])
                            {
                                System.Array.Clear(BtZPositions, indexPosition, 1);
                            }
                            BtZSaveTimer = 5;
                        }
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(BuffID.Confused, 600);
                    return false;
                }
            }
            if (!player.BackToZero)
            {
                if (npc.HasBuff(mod.BuffType("AffectedByBtZ")))
                {
                    int b = npc.FindBuffIndex(mod.BuffType("AffectedByBtZ"));
                    npc.DelBuff(b);
                }
                BtZSaveTimer = 0;
                indexPosition = 0;
                affectedbyBtz = false;
            }
            if (player.TimeSkipPreEffect)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (Main.player[i].active && playerPositionOnSkip == Vector2.Zero && Main.player[i].GetModPlayer<MyPlayer>().TimeSkipPreEffect)
                    {
                        playerPositionOnSkip = Main.player[i].position;
                    }
                }
            }
            if (player.TimeSkipEffect && !npc.townNPC && !npc.friendly && !npc.boss)
            {
                if (playerPositionOnSkip == Vector2.Zero)
                {
                    playerPositionOnSkip = Main.player[Buffs.ItemBuff.PreTimeSkip.userIndex].position;
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
                        int num17 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int num18 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)) && !Collision.SolidTilesVersatile(num17 - npc.direction * 2, num17 - npc.direction, num18 - 5, num18 - 1) && !Collision.SolidTiles(num17, num17, num18 - 5, num18 - 3) && npc.ai[1] == 0f)
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                    }
                    return true;
                }
            }
            if (player.TimeSkipEffect && npc.boss)
            {
                npc.defense /= 2;
                return true;
            }
            if (!player.TimeSkipEffect && npc.aiStyle == 0)
            {
                playerPositionOnSkip = Vector2.Zero;
                npc.aiStyle = aiStyleSave;
                aiStyleSave = 0;
                return true;
            }
            if (spawnedByDeathLoop)
            {
                deathTimer++;
                if ((deathTimer >= 30 && Buffs.ItemBuff.DeathLoop.Looping10x) || (deathTimer >= 60 && Buffs.ItemBuff.DeathLoop.Looping3x))
                {
                    if (npc.immortal || npc.hide)
                    {
                        npc.StrikeNPCNoInteraction(999999999, 0f, 1, false, true, false);
                    }
                    if (!npc.immortal)
                    {
                        npc.StrikeNPCNoInteraction(npc.lifeMax + 10, 0f, 1, false, true, false);
                    }
                    deathTimer = 0;
                }
            }
            return true;
        }

        public override bool CheckDead(NPC npc)     //check if this updates every frame, if not, move what's in this method to PreAI()
        {
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && npc.boss && Main.player[i].GetModPlayer<MyPlayer>().DeathLoop && Buffs.ItemBuff.DeathLoop.LoopNPC == 0)
                {
                    Buffs.ItemBuff.DeathLoop.LoopNPC = npc.type;
                    Buffs.ItemBuff.DeathLoop.deathPositionX = npc.position.X;
                    Buffs.ItemBuff.DeathLoop.deathPositionY = npc.position.Y;
                    Buffs.ItemBuff.DeathLoop.Looping3x = true;
                    Buffs.ItemBuff.DeathLoop.Looping10x = false;
                }
                if (Main.player[i].active && !npc.boss && Main.player[i].GetModPlayer<MyPlayer>().DeathLoop && Buffs.ItemBuff.DeathLoop.LoopNPC == 0 && !npc.friendly && npc.lifeMax > 5)
                {
                    Buffs.ItemBuff.DeathLoop.LoopNPC = npc.type;
                    Buffs.ItemBuff.DeathLoop.deathPositionX = npc.position.X;
                    Buffs.ItemBuff.DeathLoop.deathPositionY = npc.position.Y;
                    Buffs.ItemBuff.DeathLoop.Looping3x = false;
                    Buffs.ItemBuff.DeathLoop.Looping10x = true;
                }
            }
            return true;
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (target.HasBuff(mod.BuffType("BacktoZero")))     //only affects the ones with the buff, everyone's bool should turn on and save positions normally
            {
                npc.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
                damage = 0;
                if (damage < npc.life)
                {
                    npc.life -= damage;
                }
                if (damage >= npc.life)
                {
                    damage = 1;
                    npc.life = 1;
                    forceDeath = true;
                }
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (projectile.type == mod.ProjectileType("GEButterfly"))
            {
                taggedByButterfly = true;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (forceDeath)
            {
                npc.lifeRegen = -4;
            }
            base.UpdateLifeRegen(npc, ref damage);
        }
    }
}