using JoJoStands.Items.CraftingMaterials;
using JoJoStands.Items.Hamon;
using JoJoStands.Items.Vampire;
using JoJoStands.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Accessories
{
    [AutoloadEquip(EquipType.Front)]
    public class BloodiedMonocle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodied Monocle");
            Tooltip.SetDefault("While undead and worn, enemies whose blood has not been consumed are highlighted.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 28;
            Item.value = Item.buyPrice(silver: 10, copper: 50);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            VampirePlayer vPlayer = player.GetModPlayer<VampirePlayer>();
            if (vPlayer.anyMaskForm)
            {
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (npc.active && (!npc.boss && vPlayer.enemyTypesKilled[npc.type] < 10) || (npc.boss && vPlayer.enemyTypesKilled[npc.type] == 0))
                        npc.GetGlobalNPC<JoJoGlobalNPC>().zombieHightlightTimer = 2;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 2)
                .AddIngredient(ItemID.Glass, 2)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 2)
                .AddIngredient(ItemID.Glass, 2)
                .Register();
        }
    }
}
