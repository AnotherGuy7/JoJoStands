using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.VampirismArmors.Visceral
{
    [AutoloadEquip(EquipType.Body)]
    public class VisceralChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A chestplate decorated with sharp bones and seemingly living flesh.\n+12% Vampiric Knockback\nBeing hit causes a 12% to inflict Lacerated! on enemies. Hitting enemies affected with Lacerated doubles blood suck gains.");
            SacrificeTotal = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            vPlayer.vampiricKnockbackMultiplier += 0.12f;
            vPlayer.wearingVisceralChestplate = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 18)
                .AddIngredient(ItemID.Vertebrae, 28)
                .AddIngredient(ItemID.TissueSample, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}