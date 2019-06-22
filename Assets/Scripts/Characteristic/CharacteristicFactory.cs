using System.Collections.Generic;
using System.Linq;

public static class CharacteristicFactory
{
    private static readonly Dictionary<string, Characteristic> datas;

    static CharacteristicFactory()
    {
        IBaseValueStrategy baseValueStrategy = new BaseBaseValueStrategy();
        IBaseValueStrategy physicalDamageBaseValueStrategy = new PhysicalDamageBaseValueStrategy();
        IBaseValueStrategy magicalDamageBaseValueStrategy = new MagicalDamageBaseValueStrategy();
        IBaseValueStrategy hitRateBaseValueStrategy = new HitRateBaseValueStrategy();
        IMinMaxStrategy baseMinMaxStategy = new BaseMinMaxStrategy();
        IMinMaxStrategy baseStatMinMaxStategy = new BaseStatMinMaxStrategy();
        IMinMaxStrategy damageMinMaxStrategy = new DamageMinMaxStrategy();
        IMinMaxStrategy percentMinMaxStrategy = new PercentageMinMaxStrategy();
        IMinMaxStrategy multiplierMinMaxStrategy = new MultiplierMinMaxStrategy();

#pragma warning disable IDE0028
        datas = new Dictionary<string, Characteristic>(41 - 21 + 1);
        datas.Add(MaxHPId, new Characteristic(MaxHPId, "MaxHP", "Maximum Hit Points.", new MaxHPBaseValueStrategy(), new MaxHPMinMaxStrategy()));
        datas.Add(MaxSPId, new Characteristic(MaxSPId, "MaxSP", "Maximum Skill Points.", new MaxSPBaseValueStrategy(), baseMinMaxStategy));
        datas.Add(StrengthId, new Characteristic(StrengthId, "Strength", "Strength.", baseValueStrategy, baseStatMinMaxStategy));
        datas.Add(VitalityId, new Characteristic(VitalityId, "Vitality", "Vitality.", baseValueStrategy, baseStatMinMaxStategy));
        datas.Add(IntellectId, new Characteristic(IntellectId, "Intellect", "Intellect.", baseValueStrategy, baseStatMinMaxStategy));
        datas.Add(WisdomId, new Characteristic(WisdomId, "Wisdom", "Wisdom.", baseValueStrategy, baseStatMinMaxStategy));
        datas.Add(AgilityId, new Characteristic(AgilityId, "Agility", "Agility.", baseValueStrategy, baseStatMinMaxStategy));
        datas.Add(PhysMinDamageId, new Characteristic(PhysMinDamageId, "Physical Minimum Damage", "Physical Minimum Damage.", physicalDamageBaseValueStrategy, damageMinMaxStrategy));
        datas.Add(PhysMaxDamageId, new Characteristic(PhysMaxDamageId, "Physical Maximum Damage", "Physical Maximum Damage.", physicalDamageBaseValueStrategy, new MaxPhysicalDamageMinMaxStrategy()));
        datas.Add(PhysAttMultiplierId, new Characteristic(PhysAttMultiplierId, "Physical Attack Multiplier", "Physical Attack Multiplier.", baseValueStrategy, multiplierMinMaxStrategy));
        datas.Add(PhysHitRateId, new Characteristic(PhysHitRateId, "Physical Hit Rate", "Physical Hit Rate.", hitRateBaseValueStrategy, percentMinMaxStrategy));
        datas.Add(PhysDefenseId, new Characteristic(PhysDefenseId, "Physical Defense", "Physical Defense.", new PhysicalDefenseBaseValueStrategy(), baseMinMaxStategy));
        datas.Add(PhysDefMultiplierId, new Characteristic(PhysDefMultiplierId, "Physical Defense Multiplier", "Physical Defense Multiplier.", baseValueStrategy, multiplierMinMaxStrategy));
        datas.Add(PhysEvadeRateId, new Characteristic(PhysEvadeRateId, "Physical Evade Rate", "Physical Evade Rate.", new PhysicalEvadeBaseValueStrategy(), percentMinMaxStrategy));
        datas.Add(MagMinDamageId, new Characteristic(MagMinDamageId, "Magical Minimum Damage", "Magical Minimum Damage.", magicalDamageBaseValueStrategy, damageMinMaxStrategy));
        datas.Add(MagMaxDamageId, new Characteristic(MagMaxDamageId, "Magical Maximum Damage", "Magical Maximum Damage.", magicalDamageBaseValueStrategy, new MaxMagicalDamageMinMaxStrategy()));
        datas.Add(MagAttMultiplierId, new Characteristic(MagAttMultiplierId, "Magical Attack Multiplier", "Magical Attack Multiplier.", baseValueStrategy, multiplierMinMaxStrategy));
        datas.Add(MagHitRateId, new Characteristic(MagHitRateId, "Magical Hit Rate", "Magical Hit Rate.", hitRateBaseValueStrategy, percentMinMaxStrategy));
        datas.Add(MagDefenseId, new Characteristic(MagDefenseId, "Magical Defense", "Magical Defense.", new MagicalDefenseBaseValueStrategy(), baseMinMaxStategy));
        datas.Add(MagDefMultiplierId, new Characteristic(MagDefMultiplierId, "Magical Defense Multiplier", "Magical Defense Multiplier.", baseValueStrategy, multiplierMinMaxStrategy));
        datas.Add(MagEvadeRateId, new Characteristic(MagEvadeRateId, "Magical Evade Rate", "Magical Evade Rate.", new MagicalEvadeBaseValueStrategy(), percentMinMaxStrategy));

        CalculatorTrait.UpdateDefinitions(datas.Values.ToArray());
    }

    public const string StrengthId = "str";
    public const string VitalityId = "vit";
    public const string IntellectId = "int";
    public const string WisdomId = "wis";
    public const string AgilityId = "agi";
    public const string MaxHPId = "maxHp";
    public const string MaxSPId = "maxSp";
    public const string PhysMinDamageId = "physMinDmg";
    public const string PhysMaxDamageId = "physMaxDmg";
    public const string PhysAttMultiplierId = "physAttMult";
    public const string PhysHitRateId = "physHitRate";
    public const string PhysDefenseId = "physDef";
    public const string PhysDefMultiplierId = "physDefMult";
    public const string PhysEvadeRateId = "physEvadeRate";
    public const string MagMinDamageId = "magMinDmg";
    public const string MagMaxDamageId = "magMaxDmg";
    public const string MagAttMultiplierId = "magAttMult";
    public const string MagHitRateId = "magHitRate";
    public const string MagDefenseId = "magDef";
    public const string MagDefMultiplierId = "magDefMult";
    public const string MagEvadeRateId = "magEvadeRate";

    public static string[] GetEveryIds() => datas.Keys.ToArray();

    public static Characteristic Build(string id)
    {
        return datas[id];
    }
}
