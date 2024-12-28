using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Requiem
{
    [AutoloadEquip(EquipType.Head)]
    public class RequiemCrownNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Requiem Crown (Neutral)");
            // Tooltip.SetDefault("A crown made from the finest materials space has offered so far.");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 17, silver: 50);
            Item.rare = ItemRarityID.Red;
            Item.defense = 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<RequiemChestplate>() && legs.type == ModContent.ItemType<RequiemGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Viral Beetles float around you, defending you from anything that comes your way.";
            player.AddBuff(ModContent.BuffType<ViralBeetleBuff>(), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<RequiemCrownLong>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownLong>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<RequiemCrownShort>();
                Item.SetDefaults(ModContent.ItemType<RequiemCrownShort>());
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LunarBar, 17)
                .AddIngredient(ItemID.FragmentStardust, 6)
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 6)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}