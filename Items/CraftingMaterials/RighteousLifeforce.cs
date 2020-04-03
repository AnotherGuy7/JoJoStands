using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace JoJoStands.Items.CraftingMaterials
{
	public class RighteousLifeforce : ModItem
	{
		public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 4));
			DisplayName.SetDefault("Righteous Lifeforce");
			Tooltip.SetDefault("The power of someone who sets things right");
		}

		public override void SetDefaults()
        {
			item.width = 20;
			item.height = 20;
			item.maxStack = 99;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(0, 0, 75, 0);
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Items/CraftingMaterials/" + Name);
            DrawAnimation drawAnim = Main.itemAnimations[item.type];
            Rectangle sourceRect = drawAnim.GetFrame(texture);
            spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0f, 1f), sourceRect, Color.White, 0f, new Vector2(texture.Width / 2f, item.height / 2f), 1f, SpriteEffects.None, 0);     //animates faster than the normal animation???
            return false;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            gravity = 3f;
            maxFallSpeed = 0f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("WillToFight"), 3);
            recipe.AddIngredient(mod.ItemType("WillToProtect"), 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddIngredient(ItemID.Ectoplasm, 4);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}