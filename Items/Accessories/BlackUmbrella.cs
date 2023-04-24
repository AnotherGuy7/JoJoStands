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
            // Tooltip.SetDefault("A black umbrella... Useful for blocking off the sunlight!\nCan also be worn as a hat!");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 5);
            Item.accessory = true;
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
            player.GetModPlayer<VampirePlayer>().blackUmbrellaEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup("JoJoStandsIron-TierBar", 3)
                .AddIngredient(ItemID.Silk, 2)
                .AddIngredient(ModContent.ItemType<Sunscreen>())
                .Register();

        }
    }
}