using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.AstroRemains
{
    [AutoloadEquip(EquipType.Body)]
    public class AstroRemainsChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate that follows your skeletal outlines. Made from the bones of your enemies and an ore that's traveled the universe.\n+12% Vampiric Damage");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 24;
            item.value = Item.buyPrice(silver: 80);
            item.rare = ItemRarityID.Orange;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricDamageMultiplier += 0.12f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 18);
            recipe.AddIngredient(ItemID.MudBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 18);
            recipe.AddIngredient(ItemID.MudBlock, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}