using System.Collections.Generic;
using CF = CharacteristicFactory;
using TL = TraitByLevel;
using TO = TraitOperator;

public static class StatusTypeFactory
{
    private static readonly Dictionary<string, StatusType> datas;

    static StatusTypeFactory()
    {
#pragma warning disable IDE0028
        datas = new Dictionary<string, StatusType>(20 - 14 + 1);
        datas.Add(GuardId, new StatusType(GuardId, "Guard", 1, "Defensive stance that augments physical and magical defenses while lowering damages.", new TL[] { new TL(1, CF.PhysDefMultiplierId, 1, TO.Addition), new TL(1, CF.PhysDefenseId, 200, TO.PercentMultiplication), new TL(1, CF.MagDefMultiplierId, 1, TO.Addition), new TL(1, CF.MagDefenseId, 200, TO.PercentMultiplication), new TL(1, CF.PhysMinDamageId, 50, TO.PercentMultiplication), new TL(1, CF.PhysMaxDamageId, 50, TO.PercentMultiplication), new TL(1, CF.MagMinDamageId, 50, TO.PercentMultiplication), new TL(1, CF.MagMaxDamageId, 50, TO.PercentMultiplication) }));
        datas.Add(BuffStrengthId, new StatusType(BuffStrengthId, "Buff Str.", 3, "Buff strength.", new TL[] { new TL(1, CF.StrengthId, 25, TO.PercentAddition), new TL(2, CF.StrengthId, 50, TO.PercentAddition), new TL(3, CF.StrengthId, 100, TO.PercentAddition) }));
        datas.Add(BuffVitalityId, new StatusType(BuffVitalityId, "Buff Vit.", 3, "Buff vitality.", new TL[] { new TL(1, CF.VitalityId, 25, TO.PercentAddition), new TL(2, CF.VitalityId, 50, TO.PercentAddition), new TL(3, CF.VitalityId, 100, TO.PercentAddition) }));
        datas.Add(BuffIntellectId, new StatusType(BuffIntellectId, "Buff Int.", 3, "Buff intellect.", new TL[] { new TL(1, CF.IntellectId, 25, TO.PercentAddition), new TL(2, CF.IntellectId, 50, TO.PercentAddition), new TL(3, CF.IntellectId, 100, TO.PercentAddition) }));
        datas.Add(BuffWisdomId, new StatusType(BuffWisdomId, "Buff Wis.", 3, "Buff wisdom.", new TL[] { new TL(1, CF.WisdomId, 25, TO.PercentAddition), new TL(2, CF.WisdomId, 50, TO.PercentAddition), new TL(3, CF.WisdomId, 100, TO.PercentAddition) }));
        datas.Add(BuffAgilityId, new StatusType(BuffAgilityId, "Buff Agi.", 3, "Buff agility.", new TL[] { new TL(1, CF.AgilityId, 25, TO.PercentAddition), new TL(2, CF.AgilityId, 50, TO.PercentAddition), new TL(3, CF.AgilityId, 100, TO.PercentAddition) }));
        datas.Add(SleepId, new StatusType(SleepId, "Sleep", 1, "Sleeping.", new TL[0], RestrictionType.CannotMove));
        //datas.Add(StoneId, new StatusType(StoneId, "Stone", 3, "Turned into a rock.", new TL[0], RestrictionType.CannotMove, false));
    }

    public const string SleepId = "sleep";
    public const string GuardId = "guard";
    public const string BuffStrengthId = "buffStr";
    public const string BuffVitalityId = "buffVit";
    public const string BuffIntellectId = "buffInt";
    public const string BuffWisdomId = "buffWis";
    public const string BuffAgilityId = "buffAgi";

    public static StatusType Build(string id)
    {
        return datas[id];
    }
}
