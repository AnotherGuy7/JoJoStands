using JoJoStands.Buffs.ItemBuff;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class Hamon : HamonDamageClass
    {
        public override bool affectedByHamonScaling => true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon");
            Tooltip.SetDefault("Punch enemies with the power that circulates in you. \nExperience goes up after each conquer...\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 14;
            Item.width = 32;
            Item.height = 32;        //hitbox's width and height when the Item is in the world
            Item.maxStack = 1;
            Item.noUseGraphic = true;
            Item.knockBack = 1f;
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<HamonPunches>();
            Item.useTurn = true;
            Item.noWet = true;
            Item.useAnimation = 25;
            Item.useTime = 15;
            Item.useStyle = 5;
            Item.channel = true;
            Item.noMelee = true;
            Item.autoReuse = false;
            Item.shootSpeed = 15f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if (hamonPlayer.learnedHamonSkills.ContainsKey(HamonPlayer.HamonItemHealing) && hamonPlayer.learnedHamonSkills[HamonPlayer.HamonItemHealing])
            {
                TooltipLine tooltipAddition = new TooltipLine(Mod, "Damage", "Hold Right-Click for more than " + (4 / hamonPlayer.hamonSkillLevels[HamonPlayer.HamonItemHealing]) + " seconds to heal life! (Requires more than " + hamonPlayer.hamonAmountRequirements[HamonPlayer.HamonItemHealing] + " Hamon)");
                tooltips.Add(tooltipAddition);
            }
        }

        private int healTimer = 0;

        public override void HoldItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            ChargeHamon();
            if (player.whoAmI == Main.myPlayer && hamonPlayer.learnedHamonSkills.ContainsKey(HamonPlayer.HamonItemHealing) && hamonPlayer.learnedHamonSkills[HamonPlayer.HamonItemHealing])
            {
                if (Main.mouseRight && hamonPlayer.amountOfHamon >= hamonPlayer.hamonAmountRequirements[HamonPlayer.HamonItemHealing])
                {
                    healTimer++;
                    if (Main.rand.Next(0, 2) == 0)
                    {
                        int dustIndex = Dust.NewDust(player.position, player.width, player.height, 169, SpeedY: Main.rand.NextFloat(-0.6f, 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
                if (healTimer >= (4 * 60) / hamonPlayer.hamonSkillLevels[HamonPlayer.HamonItemHealing])
                {
                    int healamount = Main.rand.Next(10 + (5 * hamonPlayer.hamonSkillLevels[HamonPlayer.HamonItemHealing]), 20 * hamonPlayer.hamonSkillLevels[HamonPlayer.HamonItemHealing]);
                    player.HealEffect(healamount);
                    player.statLife += healamount;
                    hamonPlayer.amountOfHamon -= hamonPlayer.hamonAmountRequirements[HamonPlayer.HamonItemHealing];
                    healTimer = 0;
                }
                if (Main.mouseRightRelease)
                    healTimer = 0;
            }
            if (player.statLife <= 25)
                player.AddBuff(ModContent.BuffType<RUUUN>(), 360);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 25)
                .Register();
        }
    }
}
