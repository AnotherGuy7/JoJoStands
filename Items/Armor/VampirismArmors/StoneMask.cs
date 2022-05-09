using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors
{
    [AutoloadEquip(EquipType.Head)]
    public class StoneMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("An odd archaeological find. When worn, you gain superhuman abilities, but burns you in the sunlight.");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(1);
            Item.rare = 8;
            Item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.AccessoryBuff.Vampire>(), 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient(ItemID.SoulofFright, 30)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}