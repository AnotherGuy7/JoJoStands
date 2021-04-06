using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace JoJoStands.Items.Hamon
{
    public class SilkGloves : HamonDamageClass
    {
        public override bool affectedByHamonScaling => true;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Punch enemies with these Hamon-Infused silk gloves!\nExperience goes up after each conquer...\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 21;
            item.width = 22;
            item.height = 24;        //hitbox's width and height when the item is in the world
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.knockBack = 8f;
            item.rare = ItemRarityID.Pink;
            item.useTurn = true;
            item.noWet = true;
            item.useAnimation = 2;
            item.useTime = 2;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noMelee = true;
            item.autoReuse = false;
        }

        private int useCool = 0;
        private int punchCounter = 0;
        private int numberOfPunches = 0;
        private int heldEnemyIndex = -1;
        private int heldEnemyTimer = 0;
        private bool enemyBeingGrabbed = false;
        private bool canPunchAgain = false;

        public override void HoldItem(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            if (punchCounter > 0)
                punchCounter--;

            if (useCool > 0)
                useCool--;

            if (punchCounter <= 0)
            {
                numberOfPunches = 0;
            }
            ChargeHamon();

            if (mPlayer.StandOut && !mPlayer.StandAutoMode)
                return;

            hamonPlayer.usingItemThatIgnoresEnemyDamage = true;
            if (Main.mouseLeft && canPunchAgain && useCool <= 0 && player.ownedProjectileCounts[mod.ProjectileType("SilkPunches")] <= 0)
            {
                useCool += item.useTime;
                canPunchAgain = false;
                if (Main.MouseWorld.X - player.position.X >= 0)
                {
                    player.direction = 1;
                }
                else
                {
                    player.direction = -1;
                }
                if (Math.Abs(player.velocity.X) <= 4f)
                    player.velocity.X += 1f * player.direction;

                if (numberOfPunches > 2)
                {
                    punchCounter = 0;
                    hamonPlayer.amountOfHamon -= 2;
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("SilkPunches"), item.damage, item.knockBack, item.owner, numberOfPunches);
                    numberOfPunches = 0;
                }
                else
                {
                    punchCounter += 15;
                    hamonPlayer.amountOfHamon -= 1;
                    Main.PlaySound(SoundID.Item1, player.Center);
                    Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("SilkPunches"), item.damage, item.knockBack, item.owner, numberOfPunches);
                    numberOfPunches += 1;
                }
            }
            if (Main.mouseLeftRelease)
            {
                canPunchAgain = true;
            }
            if (Main.mouseRight && hamonPlayer.amountOfHamon >= 5)
            {
                if (!enemyBeingGrabbed)
                {
                    for (int n = 0; n < Main.maxNPCs; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active)
                        {
                            if (player.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                enemyBeingGrabbed = true;
                                heldEnemyIndex = npc.whoAmI;
                                hamonPlayer.enemyToIgnoreDamageFromIndex = npc.whoAmI;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    NPC heldNPC = Main.npc[heldEnemyIndex];
                    if (hamonPlayer.enemyToIgnoreDamageFromIndex == -1 || !heldNPC.active)
                    {
                        hamonPlayer.enemyToIgnoreDamageFromIndex = -1;
                        enemyBeingGrabbed = false;
                        heldEnemyIndex = -1;
                        heldEnemyTimer = 0;
                        item.reuseDelay += 120;
                        return;
                    }

                    player.controlUp = false;
                    player.controlDown = false;
                    player.controlLeft = false;
                    player.controlRight = false;
                    player.controlJump = false;
                    player.velocity = Vector2.Zero;
                    player.itemRotation = MathHelper.ToRadians(30f);

                    heldNPC.direction = -player.direction;
                    heldNPC.position = player.position + new Vector2(5f * player.direction, -2f - heldNPC.height / 3f);
                    heldNPC.velocity = Vector2.Zero;
                    hamonPlayer.enemyToIgnoreDamageFromIndex = heldNPC.whoAmI;

                    heldEnemyTimer++;
                    if (heldEnemyTimer >= 60)
                    {
                        hamonPlayer.amountOfHamon -= 5;
                        heldNPC.StrikeNPC(item.damage, 0f, player.direction);
                        heldNPC.AddBuff(mod.BuffType("Sunburn"), 3 * 60);
                        Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 3, 1f, -0.8f);
                        heldEnemyTimer = 0;
                    }
                }
            }
            if (!Main.mouseRight && hamonPlayer.enemyToIgnoreDamageFromIndex != -1)
            {
                hamonPlayer.enemyToIgnoreDamageFromIndex = -1;
                enemyBeingGrabbed = false;
                heldEnemyIndex = -1;
                heldEnemyTimer = 0;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddIngredient(ItemID.PinkGel, 3);
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddIngredient(ItemID.PinkGel, 3);
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
