using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.StandArmors.BootlegCosplay
{
    [AutoloadEquip(EquipType.Head)]
    public class BootlegCosplayHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bootleg Cosplay Hat");
            Tooltip.SetDefault("A hat that tends to merge with your hair.\n+4% Stand Critical Hit Chance");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = Item.buyPrice(0, 0, 10, 0);
            item.rare = ItemRarityID.Blue;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standCritChangeBoosts += 4f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BootlegCosplayCoat") && legs.type == mod.ItemType("BootlegCosplayPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+10% Stand Damage";
            player.GetModPlayer<MyPlayer>().standDamageBoosts += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.IronBar, 3);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddIngredient(ItemID.LeadBar, 3);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}