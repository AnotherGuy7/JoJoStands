using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Hamon
{
    public class HamonizedFirework : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Light these Hamon-Infused fireworks and burn your enemies!\nRequires 7 or more Hamon to be used.\nSpecial: Hamon Breathing");
        }

        public override void SetDefaults()
        {
            item.ranged = true;
            item.damage = 23;
            item.width = 26;
            item.height = 30;
            item.useTime = 40;
            item.useAnimation = 40;
            item.maxStack = 999;
            item.knockBack = 2f;
            item.rare = ItemRarityID.Pink;
            item.noWet = true;
            item.useTurn = true;
            item.autoReuse = false;
            item.noUseGraphic = true;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.shoot = mod.ProjectileType("HamonizedFireworkProjectile");
            item.shootSpeed = 15f;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<HamonPlayer>().amountOfHamon >= 7;
        }

        public override bool UseItem(Player player)
        {
            if (player.whoAmI == item.owner)
            {
                player.GetModPlayer<HamonPlayer>().amountOfHamon -= 7;
                player.ConsumeItem(item.type);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bomb, 2);
            recipe.AddIngredient(ItemID.Gel, 8);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 4);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }
    }
}
