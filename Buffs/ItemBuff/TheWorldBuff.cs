using Terraria;
using Terraria.ModLoader;
 
namespace JoJoStands.Buffs.ItemBuff
{
    public class TheWorldBuff : ModBuff
    {
        public override void SetDefaults()
        {
			DisplayName.SetDefault("The World");
            Description.SetDefault("Time... has been stopped");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;       //so that it can't be canceled
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.HasBuff(mod.BuffType("TheWorldBuff")))
            {
                player.GetModPlayer<MyPlayer>().TheWorldEffect = true;
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/sound/timestop_stop"));
                player.AddBuff(mod.BuffType("TimeCooldown"), 1800);
                player.GetModPlayer<MyPlayer>().TheWorldEffect = false;
            }
        }
    }
}