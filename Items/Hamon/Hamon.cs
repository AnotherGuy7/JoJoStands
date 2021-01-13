using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class Hamon : HamonDamageClass
    {
        private int healTimer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hamon");
            Tooltip.SetDefault("Punch enemies with the power that circulates in you. \nExperience goes up after each conquer...\nSpecial: Hamon Breathing");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 14;
            item.width = 32;
            item.height = 32;        //hitbox's width and height when the item is in the world
            item.maxStack = 1;
            item.noUseGraphic = true;
            item.knockBack = 1f;
            item.rare = 5;
            item.shoot = mod.ProjectileType("HamonPunches");
            item.useTurn = true;
            item.noWet = true;
            item.useAnimation = 25;
            item.useTime = 15;
            item.useStyle = 5;
            item.channel = true;
            item.noMelee = true;
            item.autoReuse = false;
            item.shootSpeed = 15f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[item.owner];
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            if (hamonPlayer.learnedHamonSkills.ContainsKey(HamonPlayer.HamonItemHealing) && hamonPlayer.learnedHamonSkills[HamonPlayer.HamonItemHealing])
            {
                TooltipLine tooltipAddition = new TooltipLine(mod, "Damage", "Hold Right-Click for more than 3 seconds to heal life! (Require more than 5 Hamon)");
                tooltips.Add(tooltipAddition);
            }
        }


        public override void HoldItem(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();

            ChargeHamon();
            if (player.whoAmI == item.owner)
            {
                if (Main.mouseRight && hamonPlayer.learnedHamonSkills.ContainsKey(HamonPlayer.HamonItemHealing) && hamonPlayer.learnedHamonSkills[HamonPlayer.HamonItemHealing] && hamonPlayer.amountOfHamon >= 5)
                {
                    healTimer++;
                }
                if (healTimer >= 180)
                {
                    item.noUseGraphic = true;
                    int healamount = Main.rand.Next(10, 20);
                    player.HealEffect(healamount);
                    player.statLife += healamount;
                    hamonPlayer.amountOfHamon -= 5;
                    healTimer = 0;
                }
                if (Main.mouseRightRelease)
                {
                    healTimer = 0;
                }
            }
            if (player.statLife <= 25)
            {
                player.AddBuff(mod.BuffType("RUUUN"), 360);
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 25);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
