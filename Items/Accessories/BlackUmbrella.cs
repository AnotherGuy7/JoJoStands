using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class BlackUmbrella : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A black umbrella... Useful for blocking off the sunlight!\nCan also be worn as a hat!");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.rare = ItemRarityID.Green;
            item.value = Item.buyPrice(silver: 5);
            item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.noSunBurning = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.noSunBurning = true;
            player.GetModPlayer<MyPlayer>().blackUmbrellaEquipped = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddIngredient(mod.ItemType("Sunscreen"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 3);
            recipe.AddIngredient(ItemID.Silk, 2);
            recipe.AddIngredient(mod.ItemType("Sunscreen"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}