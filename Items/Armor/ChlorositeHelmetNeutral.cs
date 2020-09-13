using Terraria.ID;
using Terraria;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ChlorositeHelmetNeutral : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chlorosite Helmet (Neutral)");
            Tooltip.SetDefault("A helmet that is made with Chlorophyte infused with an otherworldly virus.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ChlorositeChestplate") && legs.type == mod.ItemType("ChlorositeLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a Viral Crystal";
            player.AddBuff(mod.BuffType("ViralCrystalBuff"), 2);
        }
        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("ChlorositeHelmetLong");
                item.SetDefaults(mod.ItemType("ChlorositeHelmetLong"));
            }
            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("ChlorositeHelmetShort");
                item.SetDefaults(mod.ItemType("ChlorositeHelmetShort"));
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ChlorositeBar"), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}