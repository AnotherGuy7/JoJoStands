using Terraria.ID;
using Terraria;
using JoJoStands.Items.Hamon;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Head)]
    public class AjaStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Red Stone of Aja");
            Tooltip.SetDefault("Wear this stone to walk on water, increase your regenerative capabilities, and increase your max hamon!");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.maxStack = 1;
            item.value = Item.buyPrice(gold: 25);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
        }

        public override void UpdateEquip(Player player)
        {
            HamonPlayer hamonPlayer = player.GetModPlayer<HamonPlayer>();
            hamonPlayer.hamonDamageBoosts += 0.5f;
            hamonPlayer.hamonKnockbackBoosts += 0.5f;
            if (Main.dayTime == true)
            {
                player.meleeDamage += 0.23f;
                player.meleeSpeed += 0.18f;
                player.magicDamage *= 2;
                player.manaRegen *= 3;
                player.lifeRegen += 2;
                player.waterWalk = true;
                player.waterWalk2 = true;
            }
            player.lifeRegen += 1;
            player.waterWalk = true;
            player.waterWalk2 = true;
            Lighting.AddLight(player.Center, 1f, 0f, 0f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LargeRuby, 1);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 12);
            recipe.AddIngredient(ItemID.SunStone, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
