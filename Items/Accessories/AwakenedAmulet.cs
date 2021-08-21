using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Accessories
{
    public class AwakenedAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(10, 8));
            Tooltip.SetDefault("An amulet that perfectly represents and enchances the form of the soul.\n30% increased Stand attack damage\n2 increased Stand Speed\n20% Stand Ability cooldown reduction\n30% increased Stand crit chance\nMakes melee stands inflict Infected on enemies.\nIncreased defense while the Stand is out");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 1;
            item.value = Item.buyPrice(1, 0, 0, 0);
            item.rare = ItemRarityID.Red;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.standDamageBoosts += 0.3f;
            mPlayer.standSpeedBoosts += 2;
            mPlayer.standCooldownReduction += 0.2f;
            mPlayer.standCritChangeBoosts += 30f;
            mPlayer.awakenedAmuletEquipped = true;
            if (mPlayer.standOut)
            {
                player.statDefense += 12;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("GoldAmuletOfManipulation"));
            recipe.AddIngredient(mod.ItemType("GoldAmuletOfServing"));
            recipe.AddIngredient(mod.ItemType("GoldAmuletOfAdapting"));
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 5);
            recipe.AddRecipeGroup(RecipeGroupID.Fragment, 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("PlatinumAmuletOfManipulation"));
            recipe.AddIngredient(mod.ItemType("PlatinumAmuletOfServing"));
            recipe.AddIngredient(mod.ItemType("PlatinumAmuletOfAdapting"));
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 5);
            recipe.AddRecipeGroup(RecipeGroupID.Fragment, 5);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}