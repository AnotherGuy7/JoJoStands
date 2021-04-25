using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Items.Armor.StandArmors.Viral
{
	[AutoloadEquip(EquipType.Head)]
	public class ViralArmorHelmetNeutral : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Viral Helmet (Neutral)");
            Tooltip.SetDefault("A helmet created from a meteor, powered up by a strange virus. \nThe helmet seems to morph depending on your soul...\nStand stat buffs change depending on stand type.");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.value = Item.buyPrice(0, 3, 50, 0);
            item.rare = ItemRarityID.Green;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();

            if (mPlayer.standType == 1)
            {
                item.type = mod.ItemType("ViralArmorHelmetMelee");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetMelee"));
            }
            if (mPlayer.standType == 2)
            {
                item.type = mod.ItemType("ViralArmorHelmetRanged");
                item.SetDefaults(mod.ItemType("ViralArmorHelmetRanged"));
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ViralMeteoriteBar"), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}