using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class BulletKickFrenzy : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bullet Kick Frenzy");
            Description.SetDefault("The Sex Pistols are ready to kick any amount of bullets!");
            Main.persistentBuff[Type] = true;
            Main.debuff[Type] = true;
            canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] <= 1)
            {
                player.AddBuff(mod.BuffType("AbilityCooldown"), player.GetModPlayer<MyPlayer>().AbilityCooldownTime(120));
            }
        }
    }
}