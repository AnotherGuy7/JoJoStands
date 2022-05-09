using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.AstroRemains
{
    [AutoloadEquip(EquipType.Body)]
    public class AstroRemainsChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate that follows your skeletal outlines. Made from the bones of your enemies and an ore that's traveled the universe.\n+12% Vampiric Damage");
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 24;
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricDamageMultiplier += 0.12f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MeteoriteBar, 12)
                .AddIngredient(ItemID.Bone, 30)
                .AddIngredient(ItemID.HellstoneBar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}