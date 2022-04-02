using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.IronSlop
{
    [AutoloadEquip(EquipType.Head)]
    public class IronSlopHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A scrappily crafted iron(?) helmet. It's dirty with mud everywhere.");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.value = Item.buyPrice(silver: 5);
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("IronSlopChestplate") && legs.type == mod.ItemType("IronSlopShorts");
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "+5% Vampiric Damage; 35% Sunburn Damage Reduction.";
            vPlayer.sunburnDamageMultiplier *= 0.65f;
            vPlayer.sunburnRegenTimeMultiplier *= 0.8f;
            vPlayer.vampiricDamageMultiplier += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 9);
            recipe.AddIngredient(ItemID.MudBlock, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LeadBar, 9);
            recipe.AddIngredient(ItemID.MudBlock, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}