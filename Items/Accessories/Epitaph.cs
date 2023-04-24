using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Hamon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class Epitaph : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Epitaph");
            // Tooltip.SetDefault("Wearing this makes you want to use a frog as a phone");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.wearingEpitaph = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            mPlayer.wearingEpitaph = true;
            if (hideVisual)
                mPlayer.wearingEpitaph = false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 2)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}
