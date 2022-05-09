using JoJoStands.Items.CraftingMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
    public class ViralHammer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A hammer that, when wielded, hits with tremendous force.\nHurts enemies near the player when used.");
        }

        public override void SetDefaults()
        {
            Item.damage = 21;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.hammer = 70;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 1, 25, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active)
                    {
                        if (npc.Distance(player.position) <= 8f * 16f && npc.lifeMax > 5 && !npc.friendly && !npc.immortal && !npc.hide)
                            npc.StrikeNPC(18, 12f, player.direction);
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ViralMeteoriteBar>(), 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
