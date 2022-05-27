using JoJoStands.Buffs.AccessoryBuff;
using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Vampire;
using Terraria;
using Terraria.Audio;
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
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 99;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.value = Item.buyPrice(0, 0, 2, 50);
            Item.rare = ItemRarityID.LightPurple;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (vPlayer.zombie || vPlayer.vampire)
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " will to return to humanity was rejectd by his vampiric antics."), player.statLife / 2, player.direction);      //Tried to make a spin on that "Reject humanity" meme

            vPlayer.zombie = false;
            vPlayer.vampire = false;
            vPlayer.perfectBeing = false;
            vPlayer.anyMaskForm = false;
            SoundEngine.PlaySound(SoundID.Item3, player.Center);
            //SoundEngine.PlaySound(2, player.Center, 3);


            if (player.HasBuff(ModContent.BuffType<Buffs.AccessoryBuff.Vampire>()))
                player.ClearBuff(ModContent.BuffType<Buffs.AccessoryBuff.Vampire>());
            if (player.HasBuff(ModContent.BuffType<Zombie>()))
                player.ClearBuff(ModContent.BuffType<Zombie>());
            if (player.HasBuff(ModContent.BuffType<AjaVampire>()))
                player.ClearBuff(ModContent.BuffType<AjaVampire>());
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HealingPotion)
                .AddIngredient(ItemID.Sunflower, 3)
                .AddIngredient(ItemID.Daybloom, 2)
                .AddIngredient(ModContent.ItemType<SunDroplet>(), 2)
                .AddTile(TileID.Bottles)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}
