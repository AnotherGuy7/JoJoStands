using JoJoStands.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    public class Polaroid : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Polaroid");
            /* Tooltip.SetDefault("Stand kills build up to 5 tokens. Tokens grant 50% damage negation for a single hit.
             * Allows Stand Attacks to perform life steal.
               40% increased damage to user when not carrying a token.
               Gaining tokens has a cooldown of 5s.
            */
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 10);
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().polaroidEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<VampiricBangle>())
                .AddIngredient(ModContent.ItemType<FamilyPhoto>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}