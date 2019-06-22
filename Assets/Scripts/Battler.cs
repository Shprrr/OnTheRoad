using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator), typeof(BattlerStatus))]
public class Battler : MonoBehaviour, ISerializationCallbackReceiver
{
#pragma warning disable IDE1006
    private Animator animator;
    private const int Level = 2;

    public GameObject animationPrefab;
    public bool IsPlayer;

    [SerializeField]
    private int CurrentHP;
    public int Hp
    {
        get { return CurrentHP; }
        set
        {
            var maxHp = GetMaxHP();
            if (value > maxHp)
                value = maxHp;
            if (value < 0)
                value = 0;
            CurrentHP = value;
        }
    }
    [SerializeField]
    private int CurrentSP;
    public int Sp
    {
        get { return CurrentSP; }
        set
        {
            var maxSp = GetMaxSP();
            if (value > maxSp)
                value = maxSp;
            if (value < 0)
                value = 0;
            CurrentSP = value;
        }
    }

    public int GetMaxHP() => (int)GetCalculatedStat(CharacteristicFactory.MaxHPId);
    public int GetMaxSP() => (int)GetCalculatedStat(CharacteristicFactory.MaxSPId);
    public int GetStrength() => (int)GetCalculatedStat(CharacteristicFactory.StrengthId);
    public int GetVitality() => (int)GetCalculatedStat(CharacteristicFactory.VitalityId);
    public int GetIntellect() => (int)GetCalculatedStat(CharacteristicFactory.IntellectId);
    public int GetWisdom() => (int)GetCalculatedStat(CharacteristicFactory.WisdomId);
    public int GetAgility() => (int)GetCalculatedStat(CharacteristicFactory.AgilityId);

    [ContextMenuItem("Reset Base Stats Traits", nameof(ResetBaseStatsTraits))]
    [ContextMenuItem("Set Base Stats Traits", nameof(SetBaseStatsTraits))]
    public List<Trait> baseTraits = new List<Trait>();
    private Trait[] baseStatsTraits = new[]
        {
            new Trait(CharacteristicFactory.MaxHPId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.MaxSPId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.StrengthId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.VitalityId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.IntellectId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.WisdomId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.AgilityId, 0, TraitOperator.Addition),
            new Trait(CharacteristicFactory.PhysDefMultiplierId, 1, TraitOperator.Addition),
            new Trait(CharacteristicFactory.MagDefMultiplierId, 1, TraitOperator.Addition)
        };
    private void ResetBaseStatsTraits() => baseTraits = baseStatsTraits.ToList();
    private void SetBaseStatsTraits()
    {
        for (int i = 0; i < baseStatsTraits.Length; i++)
        {
            if (!baseTraits.Exists(t => t.Characteristic.Id == baseStatsTraits[i].Characteristic.Id && t.Operator == baseStatsTraits[i].Operator))
                baseTraits.Insert(i, new Trait(baseStatsTraits[i].Characteristic.Id, baseStatsTraits[i].Value, baseStatsTraits[i].Operator));
        }
    }

    public float GetCalculatedStat(string characteristicId, params Trait[] temporaryTraits)
    {
        return CalculatorTrait.CalculateTraits(GetAllTraits().Concat(temporaryTraits))[characteristicId];
    }

    public IEnumerable<Trait> GetAllTraits()
    {
        return baseTraits.Concat(GetAllEquips().SelectMany(e => e.Traits)).Concat(BattlerStatus.ActiveStatuses.SelectMany(s => s.GetTraits()));
    }

    public WeaponData Weapon;
    public EquipableData Offhand;
    public ArmorData Head;
    public ArmorData Body;
    public ArmorData Feet;
    public EquipableData Neck;
    public EquipableData Finger1;
    public EquipableData Finger2;
    public EquipableData[] GetAllEquips()
    {
        var list = new List<EquipableData>();
        if (!string.IsNullOrEmpty(Weapon?.Id)) list.Add(Weapon);
        if (!string.IsNullOrEmpty(Offhand?.Id)) list.Add(Offhand);
        if (!string.IsNullOrEmpty(Head?.Id)) list.Add(Head);
        if (!string.IsNullOrEmpty(Body?.Id)) list.Add(Body);
        if (!string.IsNullOrEmpty(Feet?.Id)) list.Add(Feet);
        if (!string.IsNullOrEmpty(Neck?.Id)) list.Add(Neck);
        if (!string.IsNullOrEmpty(Finger1?.Id)) list.Add(Finger1);
        if (!string.IsNullOrEmpty(Finger2?.Id)) list.Add(Finger2);
        return list.ToArray();
    }

    public SkillData[] Skills;
    public string AnimationAttack { get { return Weapon == null ? "AnimationAttack1" : Weapon.AnimationNameAttack; } }
    public BattleAction? lastAction;

    public BattlerStatus BattlerStatus;

    // Awake est appelé quand l'instance de script est chargée
    public static AssetBundle animationsBundle;
    private void Awake()
    {
        if (animationsBundle == null)
        {
            animationsBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "animations"));
            if (animationsBundle == null)
                Debug.LogError("animationsBundle is null");
        }

        Weapon = GenerateEquipableData(Weapon) as WeaponData;
        Offhand = GenerateEquipableData(Offhand);
        Head = GenerateEquipableData(Head) as ArmorData;
        Body = GenerateEquipableData(Body) as ArmorData;
        Feet = GenerateEquipableData(Feet) as ArmorData;
        Neck = GenerateEquipableData(Neck);
        Finger1 = GenerateEquipableData(Finger1);
        Finger2 = GenerateEquipableData(Finger2);

        // Put skills by Unity in the factory
        for (int i = 0; i < Skills.Length; i++)
        {
            Skills[i] = SkillFactory.Build(Skills[i].Id);
        }
#if UNITY_EDITOR
        var skills = Skills.Append(SkillFactory.Build("buffStr"));
        skills = skills.Append(SkillFactory.Build("buffVit"));
        skills = skills.Append(SkillFactory.Build("buffInt"));
        skills = skills.Append(SkillFactory.Build("buffWis"));
        skills = skills.Append(SkillFactory.Build("buffAgi"));
        Skills = skills.ToArray();
#endif

        BattlerStatus = GetComponent<BattlerStatus>();
    }

    private EquipableData GenerateEquipableData(EquipableData equipableData)
    {
        return string.IsNullOrEmpty(equipableData?.Id) ? null : ItemFactory.Build(equipableData.Id) as EquipableData;
    }

    // Start est appelé juste avant qu'une méthode Update soit appelée pour la première fois
    private void Start()
    {
        Hp = GetMaxHP();
        Sp = GetMaxSP();
        animator = GetComponent<Animator>();
        Update();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        animator.SetInteger("CurrentHp", Hp);
    }

    #region CTB
    /// <summary>
    /// Counter for CTB.  Tell the number of tick to wait for the next action.
    /// </summary>
    public int CounterCTB;
    public float HasteStatus = 1;

    public int getTickSpeed()
    {
        int agility = (int)GetCalculatedStat(CharacteristicFactory.AgilityId); ;

        if (agility == 0)
            return 28;

        if (agility == 1)
            return 26;

        if (agility == 2)
            return 24;

        if (agility == 3)
            return 22;

        if (agility == 4)
            return 20;

        if (agility >= 5 && agility <= 6)
            return 16;

        if (agility >= 7 && agility <= 9)
            return 15;

        if (agility >= 10 && agility <= 11)
            return 14;

        if (agility >= 12 && agility <= 14)
            return 13;

        if (agility >= 15 && agility <= 16)
            return 12;

        if (agility >= 17 && agility <= 18)
            return 11;

        if (agility >= 19 && agility <= 22)
            return 10;

        if (agility >= 23 && agility <= 28)
            return 9;

        if (agility >= 29 && agility <= 34)
            return 8;

        if (agility >= 35 && agility <= 43)
            return 7;

        if (agility >= 44 && agility <= 61)
            return 6;

        if (agility >= 62 && agility <= 97)
            return 5;

        if (agility >= 98 && agility <= 169)
            return 4;

        if (agility >= 170 && agility <= 255)
            return 3;

        return 0;
    }

    /// <summary>
    /// Calculate the Initial Counter Value for the CTB system.
    /// </summary>
    public void CalculateICV()
    {
        int TS = getTickSpeed();
        int minICV = 3 * TS;
        int maxICV = 30 * TS / 9;

        CounterCTB = Random.Range(minICV, maxICV + 1);
    }

    public int getCounterValue(int rank)
    {
        return (int)(getTickSpeed() * rank * HasteStatus);
    }
    #endregion

    #region BattleStats
    public bool IsDead { get { return Hp <= 0; } }
    public bool CantFight { get { return IsDead || !BattlerStatus.IsAlive(); } }

    /// <summary>
    /// Get the base damage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <returns></returns>
    public int getMinBaseDamage()
    {
        //int weaponDamage = 0;

        //if (Weapon != null)
        //    weaponDamage += Weapon.PhysicalMinDamage;

        ////if (damageOption != ePhysicalDamageOption.LEFT && RightHand is Weapon)
        ////    weaponDamage += ((Weapon)RightHand).Damage;
        ////if (damageOption != ePhysicalDamageOption.RIGHT && LeftHand is Weapon)
        ////    weaponDamage += ((Weapon)LeftHand).Damage;
        ////if (RightHand == null && LeftHand == null) // Est-ce que Shield est barehand ?
        //if (Weapon == null)
        //    weaponDamage = 1; // Barehand

        //return ActiveStrength / 4 + Level / 4 + weaponDamage;

        return (int)GetCalculatedStat(CharacteristicFactory.PhysMinDamageId);
    }

    public int getMaxBaseDamage()
    {
        //int weaponDamage = 0;

        //if (Weapon != null)
        //    weaponDamage += Weapon.PhysicalMaxDamage;

        ////if (damageOption != ePhysicalDamageOption.LEFT && RightHand is Weapon)
        ////    weaponDamage += ((Weapon)RightHand).Damage;
        ////if (damageOption != ePhysicalDamageOption.RIGHT && LeftHand is Weapon)
        ////    weaponDamage += ((Weapon)LeftHand).Damage;
        ////if (RightHand == null && LeftHand == null) // Est-ce que Shield est barehand ?
        //if (Weapon == null)
        //    weaponDamage = 2; // Barehand

        //return ActiveStrength / 4 + Level / 4 + weaponDamage;
        return (int)GetCalculatedStat(CharacteristicFactory.PhysMaxDamageId);
    }

    /// <summary>
    /// Get the hit pourcentage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <returns></returns>
    public int getHitPourc()
    {
        //int weaponHitPourc = 0;

        //if (Weapon != null)
        //    weaponHitPourc += Weapon.PhysicalAccuracy;
        ////if (damageOption != ePhysicalDamageOption.LEFT && RightHand is Weapon)
        ////    weaponHitPourc += ((Weapon)RightHand).HitPourc;
        ////if (damageOption != ePhysicalDamageOption.RIGHT && LeftHand is Weapon)
        ////    weaponHitPourc += ((Weapon)LeftHand).HitPourc;
        ////if (damageOption == ePhysicalDamageOption.BOTH && RightHand is Weapon && LeftHand is Weapon)
        ////    weaponHitPourc /= 2; // On a additionné 2 fois un 100%, donc on remet sur 100%
        ////if (RightHand == null && LeftHand == null) // Est-ce que Shield est barehand ?
        //if (Weapon == null)
        //    weaponHitPourc = 80; // Barehand

        //return weaponHitPourc;

        return (int)GetCalculatedStat(CharacteristicFactory.PhysHitRateId);
    }

    /// <summary>
    /// Get the maximum hit number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getAttackMultiplier()
    {
        //int attMul = Agility / 16 + Level / 16 + 1;
        //return attMul < 16 ? attMul : 16;
        return (int)GetCalculatedStat(CharacteristicFactory.PhysAttMultiplierId);
    }

    /// <summary>
    /// Get the defense of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getDefenseDamage()
    {
        //int armorsDefense = 0;

        //if (Head != null)
        //    armorsDefense += Head.PhysicalArmor;
        //if (Body != null)
        //    armorsDefense += Body.PhysicalArmor;
        //if (Feet != null)
        //    armorsDefense += Feet.PhysicalArmor;
        //if (Offhand is ArmorData)
        //    armorsDefense += ((ArmorData)Offhand).PhysicalArmor;

        //return Math.Max(Vitality / 2 + armorsDefense, 0);
        return (int)GetCalculatedStat(CharacteristicFactory.PhysDefenseId);
    }

    /// <summary>
    /// Get the evade pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getEvadePourc()
    {
        //int armorsEvadePourc = 0;

        //if (Head != null)
        //    armorsEvadePourc += Head.PhysicalEvasion;
        //if (Body != null)
        //    armorsEvadePourc += Body.PhysicalEvasion;
        //if (Feet != null)
        //    armorsEvadePourc += Feet.PhysicalEvasion;
        //if (Offhand is ArmorData)
        //    armorsEvadePourc += ((ArmorData)Offhand).PhysicalEvasion;

        //return Math.Max(Agility / 4 + armorsEvadePourc, 0);
        return (int)GetCalculatedStat(CharacteristicFactory.PhysEvadeRateId);
    }

    /// <summary>
    /// Get the number of shield currently equiped.
    /// </summary>
    /// <returns></returns>
    private int getNbShield()
    {
        int nbShield = 0;

        if (Offhand is ArmorData)
            nbShield++;
        //if (RightHand is Shield)
        //    nbShield++;
        //if (LeftHand is Shield)
        //    nbShield++;

        return nbShield;
    }

    /// <summary>
    /// Get the maximum block number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getDefenseMultiplier()
    {
        //return getNbShield() + 1;
        ////return getNbShield() > 0 ? (Agility / 16 + Level / 16 + 1) * getNbShield() :
        ////    Agility / 32 + Level / 32;
        return (int)GetCalculatedStat(CharacteristicFactory.PhysDefMultiplierId);
    }

    /// <summary>
    /// Get the magic base damage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <param name="spellDamage"></param>
    /// <returns></returns>
    public int getMagicMinBaseDamage(int spellDamage)
    {
        //var weaponDamage = 0;

        //if (Weapon != null)
        //    weaponDamage += Weapon.MagicalMinDamage;

        ////return (Intelligence / 2) + spellDamage; //FF3
        ////switch (damageOption)
        ////{
        ////    case eMagicalDamageOption.BLACK:
        ////        return (Intelligence / 4) + (Level / 4) + spellDamage;
        ////    case eMagicalDamageOption.WHITE:
        ////        return (Wisdom / 4) + (Level / 4) + spellDamage;
        ////    default:
        ////        return (Intelligence / 8) + (Wisdom / 8) + (Level / 4) + spellDamage;
        ////}
        //return Intellect / 8 + Wisdom / 8 + Level / 4 + weaponDamage + spellDamage;
        return (int)GetCalculatedStat(CharacteristicFactory.MagMinDamageId, new Trait(CharacteristicFactory.MagMinDamageId, spellDamage, TraitOperator.Addition));
    }

    public int getMagicMaxBaseDamage(int spellDamage)
    {
        //var weaponDamage = 0;

        //if (Weapon != null)
        //    weaponDamage += Weapon.MagicalMaxDamage;

        ////return (Intelligence / 2) + spellDamage; //FF3
        ////switch (damageOption)
        ////{
        ////    case eMagicalDamageOption.BLACK:
        ////        return (Intelligence / 4) + (Level / 4) + spellDamage;
        ////    case eMagicalDamageOption.WHITE:
        ////        return (Wisdom / 4) + (Level / 4) + spellDamage;
        ////    default:
        ////        return (Intelligence / 8) + (Wisdom / 8) + (Level / 4) + spellDamage;
        ////}
        //return Intellect / 8 + Wisdom / 8 + Level / 4 + weaponDamage + spellDamage;
        return (int)GetCalculatedStat(CharacteristicFactory.MagMaxDamageId, new Trait(CharacteristicFactory.MagMaxDamageId, spellDamage, TraitOperator.Addition));
    }

    /// <summary>
    /// Get the magic hit pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicHitPourc(int spellHitPourc)
    {
        //int weaponHitPourc = 0;

        //if (Weapon != null)
        //    weaponHitPourc += Weapon.MagicalAccuracy;
        //// 80% barehanded
        ////return (Intelligence / 2) + spellHitPourc; //FF3
        ////switch (damageOption)
        ////{
        ////    case eMagicalDamageOption.BLACK:
        ////        return (Intelligence / 8) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        ////    case eMagicalDamageOption.WHITE:
        ////        return (Wisdom / 8) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        ////    default:
        ////        return (Intelligence / 16) + (Wisdom / 16) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        ////}
        //return Intellect / 16 + Wisdom / 16 + Level / 4 + weaponHitPourc + spellHitPourc;

        return (int)GetCalculatedStat(CharacteristicFactory.MagHitRateId, new Trait(CharacteristicFactory.MagHitRateId, spellHitPourc, TraitOperator.PercentMultiplication));
    }

    /// <summary>
    /// Get the maximum magic hit number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicAttackMultiplier()
    {
        //int attMul = 0;
        ////switch (damageOption)
        ////{
        ////    case eMagicalDamageOption.BLACK:
        ////        attMul = (Intelligence / 16) + (Level / 16) + 1;
        ////        break;
        ////    case eMagicalDamageOption.WHITE:
        ////        attMul = (Wisdom / 16) + (Level / 16) + 1;
        ////        break;
        ////    default:
        ////        attMul = (Intelligence / 32) + (Wisdom / 32) + (Level / 16) + 1;
        ////        break;
        ////}
        //attMul = Intellect / 32 + Wisdom / 32 + Level / 16 + 1;
        //return attMul < 16 ? attMul : 16;
        return (int)GetCalculatedStat(CharacteristicFactory.MagAttMultiplierId);
    }

    /// <summary>
    /// Get the magic defense of the current job with the current equipement.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseDamage()
    {
        //int armorsDefense = 0;

        //if (Head != null)
        //    armorsDefense += Head.MagicalArmor;
        //if (Body != null)
        //    armorsDefense += Body.MagicalArmor;
        //if (Feet != null)
        //    armorsDefense += Feet.MagicalArmor;
        //if (Offhand is ArmorData)
        //    armorsDefense += ((ArmorData)Offhand).MagicalArmor;

        //return Math.Max(Wisdom / 2 + armorsDefense, 0);
        return (int)GetCalculatedStat(CharacteristicFactory.MagDefenseId);
    }

    /// <summary>
    /// Get the magic evade pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicEvadePourc()
    {
        //int armorsEvadePourc = 0;

        //if (Head != null)
        //    armorsEvadePourc += Head.MagicalEvasion;
        //if (Body != null)
        //    armorsEvadePourc += Body.MagicalEvasion;
        //if (Feet != null)
        //    armorsEvadePourc += Feet.MagicalEvasion;
        //if (Offhand is ArmorData)
        //    armorsEvadePourc += ((ArmorData)Offhand).MagicalEvasion;

        //return Math.Max(Agility / 8 + Wisdom / 8 + armorsEvadePourc, 0);
        return (int)GetCalculatedStat(CharacteristicFactory.MagEvadeRateId);
    }

    /// <summary>
    /// Get the maximum magic block number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseMultiplier()
    {
        //return getNbShield() + 1;
        ////return getNbShield() > 0 ? (Agility / 32 + Wisdom / 32 + Level / 16 + 1) * getNbShield() :
        ////    Agility / 64 + Wisdom / 64 + Level / 32;
        ////return (Agility / 32) + (Wisdom / 16); //FF3
        return (int)GetCalculatedStat(CharacteristicFactory.MagDefMultiplierId);
    }
    #endregion

    #region Battle Methods
    public void Attacks(Battler target)
    {
        var damage = CalculatePhysicalDamage(target);
        damage.Name = "Attack";
        Debug.LogFormat("{0} attacks {1} for {2}", name, target.name, damage);
        InstantiateTakingDamage(target.transform, damage, AnimationAttack, 1);
    }

    public bool Casts(SkillData skill, out int skillLevel)
    {
        skillLevel = 1;
        if (!Skills.Contains(skill) || Sp < skill.SpCost) return false;

        Sp -= skill.SpCost;
        return true;
    }

    public void Used(Battler target, IDataEffect data, int nbTarget)
    {
        data.Effect.CalculateDamage(this, target, out Damage damage, nbTarget);
        damage.Name = data.Name;
        Debug.LogFormat("{0} used {3} on {1} for {2}", name, target.name, damage, data.Name);
        InstantiateTakingDamage(target.transform, damage, data.AnimationName, nbTarget);
    }

    private void InstantiateTakingDamage(Transform targetTransform, Damage damage, string animationName, int nbTarget)
    {
        // When targetting more than one target, the animation plays on the party, but damages is showing on each targets.
        if (nbTarget > 1)
        {
            Transform partyTransform = damage.Target.IsPlayer ? targetTransform.parent : targetTransform.parent.parent;

            // Create juste one for the party.
            if (partyTransform.GetComponentInChildren<TakingDamage>() == null)
            {
                var takingParty = Instantiate(animationPrefab, partyTransform).GetComponent<TakingDamage>();
                takingParty.showAnimation = true;
                takingParty.showDamage = false;
                takingParty.damage = Damage.Empty; // To not taking damage.
                takingParty.damage.Name = damage.Name; // But to show the name.

                if (animationsBundle == null)
                    Debug.LogError("animationsBundle is null");
                takingParty.animationAttack = animationsBundle.LoadAsset<AnimatorOverrideController>(animationName);
            }
        }

        var taking = Instantiate(animationPrefab, targetTransform).GetComponent<TakingDamage>();
        taking.showAnimation = nbTarget == 1;
        taking.showDamage = true;
        taking.damage = damage;

        if (animationsBundle == null)
            Debug.LogError("animationsBundle is null");
        taking.animationAttack = animationsBundle.LoadAsset<AnimatorOverrideController>(animationName);
    }

    public Damage CalculatePhysicalDamage(Battler target, int spellDamage = 0, bool alwaysHit = false, int nbTarget = 1)
    {
        var damage = new Damage(Damage.DamageType.HP, 0, 0, this, target);

        //Calculate min and max base damage
        int baseMinDmg = getMinBaseDamage() + spellDamage;

        //Bonus on base damage for Attacker
        //baseMinDmg += HasCheer ? 10 * CheerLevel : 0;
        //ou
        //baseMinDmg += HasCheer ? baseMinDmg * CheerLevel / 15 : 0;
        //baseMinDmg *= IsAlly ? 2 : 1;
        //baseMinDmg *= ElementalEffect(attacker);
        //baseMinDmg *= IsMini || IsToad ? 2 : 1;
        //baseMinDmg *= attacker->IsMini || attacker->IsToad ? 0 : 1;

        //int baseMaxDmg = (int)(baseMinDmg * 1.5);
        int baseMaxDmg = getMaxBaseDamage() + spellDamage;

        //Calculate hit%
        int hitPourc = getHitPourc();
        hitPourc = hitPourc < 99 ? hitPourc : 99;
        //hitPourc /= (attacker.IsFrontRow || weapon.IsLongRange ? 1 : 2);
        //hitPourc /= (blindStatus ? 2 : 1);
        //hitPourc /= (IsFrontRow || weapon.IsLongRange ? 1 : 2);

        //Calculate attack multiplier
        damage.Multiplier = 0;
        for (int i = 0; i < getAttackMultiplier(); i++)
            if (Random.Range(0, 100) < hitPourc)
                damage.Multiplier++;

        //Bonus on defense for Target
        int defense = target.getDefenseDamage();
        //defense *= BattlerStatus.HasStatus<GuardStatus>() ? 4 : 1;
        //defense *= (IsAlly ? 0 : 1);
        //defense *= (IsRunning ? 0 : 1);
        //defense *= (IsMini || IsToad ? 0 : 1);

        //Calculate defense multiplier
        int defenseMul = target.getDefenseMultiplier();
        //defenseMul *= (IsAlly ? 0 : 1);
        //defenseMul *= (IsRunning ? 0 : 1);
        //defenseMul *= (IsMini || IsToad ? 0 : 1);

        //Calculate multiplier and final damage
        for (int i = 0; i < defenseMul; i++)
            if (Random.Range(0, 100) < target.getEvadePourc())
                damage.Multiplier--;

        if (alwaysHit && damage.Multiplier < 1)
            damage.Multiplier = 1;

        damage.Value = (Random.Range(baseMinDmg, baseMaxDmg + 1) - defense) * damage.Multiplier;
        //damage *= AttackIsJump ? 3 : 1;
        damage.Value /= nbTarget;

        //Validate final damage and multiplier
        if (damage.Value < 1) //Min 1 s'il tape au moins une fois
            damage.Value = 1;

        if (damage.Multiplier < 1) //Check s'il tape au moins une fois
        {
            damage.Multiplier = 0;
            damage.Value = 0;
        }

        // Check le Damage 0 no miss
        //Debug.AssertFormat(damage.Multiplier > 0 && damage.Value != 0 || damage.Multiplier == 0 && damage.Value == 0, "Incohérence dans Damage (Multiplier={0}, Value={1})", damage.Multiplier, damage.Value);

        return damage;
    }

    public Damage CalculateMagicalDamage(Battler target, int spellDamage, int spellHitPourc, int nbTarget)
    {
        var damage = new Damage(Damage.DamageType.HP, 0, 0, this, target);

        bool isItem = spellHitPourc == 101;

        //Calculate min and max base damage
        int baseMinDmg = getMagicMinBaseDamage(spellDamage);

        //Bonus on base damage for Attacker
        //baseMinDmg *= ElementalEffect(attacker);
        //baseMinDmg *= IsMini || IsToad ? 2 : 1;

        //int baseMaxDmg = (int)(baseMinDmg * 1.5);
        int baseMaxDmg = getMagicMaxBaseDamage(spellDamage);

        //Calculate hit%
        int hitPourc = 0;
        if (isItem)
            hitPourc = 100;
        else
        {
            hitPourc = getMagicHitPourc(spellHitPourc);
            hitPourc = hitPourc < 99 ? hitPourc : 99;
        }

        //Calculate attack multiplier
        if (isItem)
            damage.Multiplier = 1;
        else
        {
            damage.Multiplier = 0;
            for (int i = 0; i < getMagicAttackMultiplier(); i++)
                if (Random.Range(0, 100) < hitPourc)
                    damage.Multiplier++;
        }

        //Bonus on defense for Target
        int defense = target.getMagicDefenseDamage();
        //defense *= (IsDefending ? 4 : 1);
        //defense *= (IsAlly ? 0 : 1);
        //defense *= (IsRunning ? 0 : 1);
        //defense *= (IsMini || IsToad ? 0 : 1);

        //Calculate defense multiplier
        int defenseMul = target.getMagicDefenseMultiplier();
        //defenseMul *= (IsAlly ? 0 : 1);
        //defenseMul *= (IsRunning ? 0 : 1);
        //defenseMul *= (IsMini || IsToad ? 0 : 1);

        //Calculate multiplier and final damage
        if (!isItem)
            for (int i = 0; i < defenseMul; i++)
                if (Random.Range(0, 100) < target.getMagicEvadePourc())
                    damage.Multiplier--;

        damage.Value = (Random.Range(baseMinDmg, baseMaxDmg + 1) - defense) * damage.Multiplier;

        damage.Value /= nbTarget;

        //Validate final damage and multiplier
        if (damage.Value < 1) //Min 1 s'il tape au moins une fois
            damage.Value = 1;

        if (damage.Multiplier < 1) //Check s'il tape au moins une fois
            damage.Value = 0;

        return damage;
    }
    #endregion

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        //if (parameters.Count != Enum.GetValues(typeof(Parameters.ParameterIndex)).Length)
        //{
        //    var oldValues = parameters;
        //    parameters = new Parameters();
        //    for (int i = 0; i < oldValues.Count; i++)
        //    {
        //        parameters.ChangeValue(i, oldValues[i].BaseValue);
        //    }
        //}

        //if (secondaryParameters.Count != Enum.GetValues(typeof(SecondaryParameters.SecondaryParameterIndex)).Length)
        //{
        //    var oldValues = secondaryParameters;
        //    secondaryParameters = new SecondaryParameters();
        //    for (int i = 0; i < oldValues.Count; i++)
        //    {
        //        secondaryParameters.ChangeValue(i, oldValues[i].Value);
        //    }
        //}
    }
}
