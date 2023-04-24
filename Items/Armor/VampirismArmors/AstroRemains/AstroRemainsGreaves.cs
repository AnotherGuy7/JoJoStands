using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.AstroRemains
{
    [AutoloadEquip(EquipType.Legs)]
    public class AstroRemainsGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Greaves that have seen the depths of the Dungeon and the expanse of the universe.\n+10% Movement Speed while using Vampiric Items");
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 65);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            if (player.HeldItem.ModItem is VampireDamageClass)
                player.moveSpeed *= 1.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 8)
                .AddIngredient(ItemID.Bone, 30)
                .AddIngredient(ItemID.HellstoneBar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}