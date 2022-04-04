using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class AntiVampireSerum : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Anti-Vampirism Serum");
            Tooltip.SetDefault("A serum infused with Hamon made to cure those who have fallen to the power of the masks.");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.maxStack = 99;
            item.useTime = 20;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 0, 2, 50);
            item.rare = ItemRarityID.LightPurple;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (vPlayer.zombie || vPlayer.vampire)
            {
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " will to return to humanity was rejectd by his vampiric antics."), player.statLife / 2, player.direction);      //Tried to make a spin on that "Reject humanity" meme
            }
            vPlayer.zombie = false;
            vPlayer.vampire = false;
            vPlayer.perfectBeing = false;
            vPlayer.anyMaskForm = false;
            Main.PlaySound(2, player.Center, 3);

            if (player.HasBuff(mod.BuffType("Vampire")))
                player.ClearBuff(mod.BuffType("Vampire"));
            if (player.HasBuff(mod.BuffType("Zombie")))
                player.ClearBuff(mod.BuffType("Zombie"));
            if (player.HasBuff(mod.BuffType("AjaVampire")))
                player.ClearBuff(mod.BuffType("AjaVampire"));
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HealingPotion);
            recipe.AddIngredient(ItemID.Sunflower, 3);
            recipe.AddIngredient(ItemID.Daybloom, 2);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 2);
            recipe.AddTile(TileID.Bottles);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
