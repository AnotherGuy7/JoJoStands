using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Items.Armor
{
    public class CenturyBoy : ModItem
    {
        int limitTimer = 36000;       //like 10 minutes
        int breathSave = 0;

        public void SetStaticDefault()
        {
            DisplayName.SetDefault("20th Century Boy");
            Tooltip.SetDefault("Use the special ability key to make yourself immune to damage, but unable to move or use items.");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.accessory = true;
            item.rare = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (JoJoStands.AccessoryHotKey.Current)
            {
                player.controlUseItem = false;
                player.dash *= 0;
                player.bodyVelocity = Vector2.Zero;
                player.controlLeft = false;
                player.controlJump = false;
                player.controlRight = false;
                player.controlDown = false;
                player.controlQuickHeal = false;
                player.controlQuickMana = false;
                player.controlRight = false;
                player.controlUseTile = false;
                player.controlUp = false;
                player.maxRunSpeed = 0f;
                player.moveSpeed = 0f;
                player.noFallDmg = true;
                player.AddBuff(mod.BuffType("CenturyBoyBuff"), 2, true);
                mod.GetEquipSlot("CenturyBoy_Body", EquipType.Body);
                limitTimer--;
                if (player.wet && player.ZoneSnow)
                {
                    limitTimer -= 6;
                    if (breathSave == 0)
                    {
                        breathSave = player.breath;
                    }
                }
                if (player.wet)
                {
                    limitTimer -= 3;
                    if (breathSave == 0)
                    {
                        breathSave = player.breath;
                    }
                }
                if (breathSave != 0)
                {
                    player.breath = breathSave;
                }
                if (limitTimer <= 0)
                {
                    if (player.wet || (player.wet && player.ZoneSnow))
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason("The water kept it's constant rythm and " + player.name + " has stopped waiting. And stopped thinking."), player.statLife - 1, 1);
                    }
                    else
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " has stopped thinking."), player.statLife - 1, 1);
                    }
                }
            }
            else
            {
                limitTimer = 36000;
                breathSave = 0;
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StandArrow"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}