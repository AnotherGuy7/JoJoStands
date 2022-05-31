using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Chlorosite
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorositeHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Helmet (Neutral)");
            Tooltip.SetDefault("A helmet that is made with Chlorophyte infused with an otherworldly virus.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 4, silver: 75);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ChlorositeChestplate>() && legs.type == ModContent.ItemType<ChlorositeLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a Viral Crystal";
            player.AddBuff(ModContent.BuffType<ViralCrystalBuff>(), 2);
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 2)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetLong>();
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetLong>());
            }
            if (mPlayer.standType == 1)
            {
                Item.type = ModContent.ItemType<ChlorositeHelmetShort>();
                Item.SetDefaults(ModContent.ItemType<ChlorositeHelmetShort>());
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ChlorositeBar>(), 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}