using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace JoJoStands.Items
{
	public class Hamon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hamon");
			Tooltip.SetDefault("Punch enemies with the power that circulates in you. \nExperience goes up after each conquer...");
		}
		public override void SetDefaults()
		{
			item.damage = 28;
			item.width = 100;
			item.height = 8;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 3;
			item.melee = true;
			item.maxStack = 1;
			item.knockBack = 2;
			item.rare = 6;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/sound/ora.wav");
			item.autoReuse = true;
			item.shootSpeed = 50f;
            item.useTurn = true;
            item.noWet = true;
		}

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>(mod);
            if (modPlayer.AjaStone)
            {
                item.damage = 0;
            }
            if (NPC.downedBoss1)
            {
                item.damage = 36;
            }
            if(NPC.downedBoss2 && NPC.downedBoss1)
            {
                item.damage = 64;
            }
            if(NPC.downedBoss3 && NPC.downedBoss2 && NPC.downedBoss1)
            {
                item.damage = 94;
            }
            if(NPC.downedMechBossAny && NPC.downedBoss3 && NPC.downedBoss2 && NPC.downedBoss1)
            {
                item.damage = 137;
            }
            if(NPC.downedPlantBoss && NPC.downedMechBossAny && NPC.downedBoss3 && NPC.downedBoss2 && NPC.downedBoss1)
            {
                item.damage = 164;
            }
            if(NPC.downedGolemBoss && NPC.downedPlantBoss && NPC.downedMechBossAny && NPC.downedBoss3 && NPC.downedBoss2 && NPC.downedBoss1)
            {
                item.damage = 186;
            }
        }

        public override void HoldItem(Player player)
        {
            Dust.NewDust(item.position, item.width, item.height, 169);
            player.waterWalk = true;
            player.waterWalk2 = true;
		}

        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (target.HasBuff(mod.BuffType("Vampire")))
            {
                item.damage += 50;
            }
        }

        public override bool UseItem(Player player)
        {
            
            return base.UseItem(player);
        }

        public override bool UseItemFrame(Player player)
        {
            return base.UseItemFrame(player);
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SunDroplet"), 8);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
