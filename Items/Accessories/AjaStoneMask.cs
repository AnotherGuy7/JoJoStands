using JoJoStands.Buffs.AccessoryBuff;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Head)]
    public class AjaStoneMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The mask now merged with the stone seems to give off tremendous power...");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(platinum: 1, gold: 50);
            Item.rare = 10;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
            player.AddBuff(BuffType<AjaVampire>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StoneMask>())
                .AddIngredient(ModContent.ItemType<AjaStone>())
                .AddTile(TileID.MythrilAnvil);
        }
    }
}