using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Projectiles.PlayerStands.SoftAndWetGoBeyond;
using JoJoStands.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class SoftAndWetGoBeyond : StandItemClass
    {
        public override int StandSpeed => 9;
        public override int StandType => 1;
        public override string StandProjectileName => "SoftAndWet";
        public override int StandTier => 5;
        public override Color StandTierDisplayColor => Color.LightBlue;

        public override string Texture
        {
            get { return Mod.Name + "/Items/SoftAndWetT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soft and Wet (Go Beyond!)");
            Tooltip.SetDefault("Punch enemies at a fast rate and hold right-click to create a non-existent bubble!" +
                "\nSpecial: Select a target to track with bubbles" +
                "\nSecond Special: Bubble Barrier!" +
                "\nPassive: Bubble Generation & Bubble Offensive" +
                "\nBubble Generation allows Soft & Wet to generate bubbles while barraging." +
                "\nBubble Offensive creates bubbles around Soft & Wet that can be manipulated." +
                "\nWhile tracking an enemy with bubbles, bubbles formed by Bubble Offensive will explode on contact with that enemy." +
                "\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            Item.damage = 109;
            Item.width = 32;
            Item.height = 32;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.LightPurple;
        }


        public override bool ManualStandSpawning(Player player)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.position, player.velocity, ModContent.ProjectileType<SoftAndWetGoBeyondStand>(), 0, 0f, Main.myPlayer);
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RequiemArrow>())
                .AddIngredient(ModContent.ItemType<SoftAndWetFinal>())
                .AddIngredient(ItemID.Bomb, 10)
                .AddIngredient(ModContent.ItemType<RighteousLifeforce>())
                .AddTile(ModContent.TileType<RemixTableTile>())
                .Register();
        }
    }
}
