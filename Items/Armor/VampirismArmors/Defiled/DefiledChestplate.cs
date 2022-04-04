using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.Defiled
{
    [AutoloadEquip(EquipType.Body)]
    public class DefiledChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate wrapped in corruption stingers, perfect for keeping off the enemies.\n+12% Vampiric Knockback\nApplies a thorns effect while the chestplate is being worn.");
        }

        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 36;
            item.value = Item.buyPrice(gold: 1);
            item.rare = ItemRarityID.Orange;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricKnockbackMultiplier += 0.12f;
            player.AddBuff(BuffID.Thorns, 2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 18);
            recipe.AddIngredient(ItemID.RottenChunk, 25);
            recipe.AddIngredient(ItemID.ShadowScale, 10);
            recipe.AddIngredient(ItemID.WormTooth, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}