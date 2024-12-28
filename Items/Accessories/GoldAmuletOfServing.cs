﻿using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Tiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class GoldAmuletOfServing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
            // DisplayName.SetDefault("Amulet of Serving");
            // Tooltip.SetDefault("2 increased Stand Speed\nIncreased defense while the Stand is out");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<MyPlayer>().standSpeedBoosts += 2;
            player.GetModPlayer<MyPlayer>().standAccessoryDefense += 12;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            bool alternateAmuletEquipped = false;
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item Item = player.armor[i];
                if (Item.type == ModContent.ItemType<PlatinumAmuletOfServing>())
                {
                    alternateAmuletEquipped = true;
                    break;
                }
            }
            return !alternateAmuletEquipped;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<GreaterGoldAmuletOfFight>())
                .AddIngredient(ModContent.ItemType<GreaterGoldAmuletOfProtect>())
                .AddIngredient(ModContent.ItemType<WillToFight>(), 2)
                .AddIngredient(ModContent.ItemType<WillToProtect>(), 2)
                .AddIngredient(ItemID.Ectoplasm, 5)
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
