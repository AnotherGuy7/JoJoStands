using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.Crystal
{
    [AutoloadEquip(EquipType.Legs)]
    public class CrystalLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Leggings");
            // Tooltip.SetDefault("Leggings made out of crystal.\n12% movement speed");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 1, silver: 25);
            Item.rare = ItemRarityID.LightPurple;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed *= 1.12f;      //12% increase
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrystalShard, 6)
                .AddIngredient(ItemID.Silk, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}