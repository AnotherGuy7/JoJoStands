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
            Item.width = 42;
            Item.height = 36;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricKnockbackMultiplier += 0.12f;
            player.AddBuff(BuffID.Thorns, 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 18)
                .AddIngredient(ItemID.RottenChunk, 25)
                .AddIngredient(ItemID.ShadowScale, 10)
                .AddIngredient(ItemID.WormTooth, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}