using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.Defiled
{
    [AutoloadEquip(EquipType.Legs)]
    public class DefiledGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Dark greaves constructed of feared life.\nIncreases jump speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.buyPrice(silver: 80);
            item.rare = ItemRarityID.Orange;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.jumpSpeedBoost += 1.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 14);
            recipe.AddIngredient(ItemID.RottenChunk, 20);
            recipe.AddIngredient(ItemID.ShadowScale, 8);
            recipe.AddIngredient(ItemID.WormTooth, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}