using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace JoJoStands.DropConditions
{
    public class WillToProtectCondition : IItemDropRuleCondition
    {
        public string GetConditionDescription()
        {
            return "Drops during nighttime while in the Forest";
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public bool CanDrop(DropAttemptInfo info)
        {
            if (!info.IsInSimulation)
                return !Main.dayTime && info.player.ZoneForest;

            return false;
        }
    }
}
