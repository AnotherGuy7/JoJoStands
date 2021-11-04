using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.Visceral
{
    [AutoloadEquip(EquipType.Body)]
    public class VisceralChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate decorated with sharp bones and seemingly living flesh.\n+12% Vampiric Knockback\nBeing hit causes a 12% to inflict Lacerated! on enemies. Hitting enemies affected with Lacerated doubles blood suck gains.");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = Item.buyPrice(gold: 1);
            item.rare = ItemRarityID.Orange;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricKnockbackMultiplier += 0.12f;
            vPlayer.wearingVisceralChestplate = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 18);
            recipe.AddIngredient(ItemID.Vertebrae, 28);
            recipe.AddIngredient(ItemID.TissueSample, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}