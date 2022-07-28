using JoJoStands.Buffs.Debuffs;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class ThreadedGloves : HamonDamageClass
    {
        public override bool affectedByHamonScaling => true;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Left-click to punch enemies with these boxing gloves wrapped around Hamon-Infused silk strings and right-click to grab enemies and inject Hamon into them!\nSpecial: Hamon Breathing");
            SacrificeTotal = 1;
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 62;
            Item.width = 22;
            Item.height = 24;        //hitbox's width and height when the Item is in the world
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 8f;
            Item.rare = ItemRarityID.Pink;
            Item.useTurn = true;
            Item.noWet = true;
            Item.useAnimation = 2;
            Item.useTime = 2;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shootSpeed = 15f;
        }

        private int punchCounter = 0;
        private int numberOfPunches = 0;
        private int heldEnemyIndex = -1;
        private int heldEnemyTimer = 0;
        private bool enemyBeingGrabbed = false;
        private int useCool = 0;
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
                numberOfPunches = 0;

            ChargeHamon();
            if (mPlayer.standOut && !mPlayer.standAutoMode)
                return;

            hamonPlayer.usingItemThatIgnoresEnemyDamage = true;
            if (Main.mouseLeft && canPunchAgain && useCool <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<ThreadedGlovePunches>()] <= 0)
            {
                useCool += Item.useTime;
                canPunchAgain = false;
                if (Main.MouseWorld.X - player.position.X >= 0)
                    player.direction = 1;
                else
                    player.direction = -1;
                if (Math.Abs(player.velocity.X) <= 4f)
                    player.velocity.X += 1f * player.direction;

                if (numberOfPunches > 2)
                {
                    punchCounter = 0;
                    hamonPlayer.amountOfHamon -= 2;
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<ThreadedGlovePunches>(), Item.damage, Item.knockBack, player.whoAmI, numberOfPunches);
                    numberOfPunches = 0;
                }
                else
                {
                    punchCounter += 15;
                    hamonPlayer.amountOfHamon -= 1;
                    SoundEngine.PlaySound(SoundID.Item1, player.Center);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<ThreadedGlovePunches>(), Item.damage, Item.knockBack, player.whoAmI, numberOfPunches);
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
                        Item.reuseDelay += 120;
                        return;
                    }

                    Item.reuseDelay = 3;
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
                        heldNPC.StrikeNPC(Item.damage, 0f, player.direction);
                        heldNPC.AddBuff(ModContent.BuffType<Sunburn>(), 5 * 60);
                        SoundStyle item3 = SoundID.Item3;
                        item3.Pitch = -0.8f;
                        SoundEngine.PlaySound(item3, player.Center);
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
                Item.reuseDelay += 120;
                return;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsCobalt-TierBar", 4)
                .AddRecipeGroup("JoJoStandsIron-TierBar", 12)
                .AddIngredient(ItemID.WhiteString, 3)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
