using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class SoulOfTimeCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops while in Hardmode and in Space";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return Main.hardMode && info.player.ZoneNormalSpace;

            return false;
        }
    }
}
