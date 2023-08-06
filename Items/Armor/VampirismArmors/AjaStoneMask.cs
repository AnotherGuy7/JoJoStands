using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors
{
    [AutoloadEquip(EquipType.Head)]
    public class AjaStoneMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("The mask now merged with the stone seems to give off tremendous power...");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.value = Item.buyPrice(platinum: 2, gold: 50);
            Item.rare = ItemRarityID.Red;
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
            player.AddBuff(ModContent.BuffType<AjaVampire>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<StoneMask>())
                .AddIngredient(ModContent.ItemType<AjaStone>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}