using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Alloys
{
    [AutoloadEquip(EquipType.Head)]
    public class TitaniumAlloyMaskNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Titanium Alloy Mask (Neutral)");
            // Tooltip.SetDefault("A mask fused with Viral Meteorite to empower the user.\nNote: This helmet makes a set when paired with the rest of the default Titanium set.\nStand stat buffs change depending on stand type.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 18;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskShort>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskShort>());
            }
            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<TitaniumAlloyMaskLong>();
                Item.SetDefaults(ModContent.ItemType<TitaniumAlloyMaskLong>());
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 10)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 12)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}