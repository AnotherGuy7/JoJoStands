using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace JoJoStands.Items.Armor.VampirismArmors.AstroRemains
{
    [AutoloadEquip(EquipType.Head)]
    public class AstroRemainsSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A deer skull with a surprisingly sturdy exterior.");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 44;
            item.value = Item.buyPrice(silver: 70);
            item.rare = ItemRarityID.Orange;
            item.defense = 8;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("AstroRemainsChestplate") && legs.type == mod.ItemType("AstroRemainsGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            player.setBonus = "80% Sunburn Damage Reduction\nWhile using Vampiric Abilities: 20% to dodge incoming attacks";
            vPlayer.sunburnDamageMultiplier *= 0.2f;
            vPlayer.wearingAstroRemainsSet = true;
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