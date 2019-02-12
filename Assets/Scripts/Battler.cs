﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator), typeof(BattlerStatus))]
public class Battler : MonoBehaviour, ISerializationCallbackReceiver
{
    private Animator animator;
    private int Level = 2;

    public GameObject animationPrefab;
    public bool IsPlayer;

    public int BaseMaxHP;
    public int BaseMaxSP;
    public int Strength;
    public int Vitality;
    public int Intellect;
    public int Wisdom;
    public int Agility;

    public int CurrentHP;
    public int Hp
    {
        get { return CurrentHP; }
        set
        {
            if (value > MaxHP)
                value = MaxHP;
            if (value < 0)
                value = 0;
            CurrentHP = value;
        }
    }
    public int CurrentSP;
    public int Sp
    {
        get { return CurrentSP; }
        set
        {
            if (value > MaxSP)
                value = MaxSP;
            if (value < 0)
                value = 0;
            CurrentSP = value;
        }
    }

    public int MaxHP { get { return BaseMaxHP + Vitality / 4 * (Level - 1) + (Level - 1) * 10; } }
    public int MaxSP { get { return BaseMaxSP + Wisdom / 4 * (Level - 1) + (Level - 1) * 5; } }

    [ContextMenuItem("Reset", nameof(ResetParameters))]
    public Parameters parameters = new Parameters();
    private void ResetParameters() => parameters = new Parameters();

    [ContextMenuItem("Reset", nameof(ResetSecondaryParameters))]
    public SecondaryParameters secondaryParameters = new SecondaryParameters();
    private void ResetSecondaryParameters() => secondaryParameters = new SecondaryParameters();


    public IEnumerable<Trait> GetAllTraits()
    {
        return GetAllEquips().SelectMany(e => e.Traits);
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
        Hp = MaxHP;
        Sp = MaxSP;
        animator = GetComponent<Animator>();
        Update();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        animator.SetInteger("CurrentHp", CurrentHP);
    }

    #region CTB
    /// <summary>
    /// Counter for CTB.  Tell the number of tick to wait for the next action.
    /// </summary>
    public int CounterCTB;
    public float HasteStatus = 1;

    public int getTickSpeed()
    {
        int agility = Agility;

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
    public bool IsDead { get { return CurrentHP <= 0; } }
    public bool CantFight { get { return IsDead || BattlerStatus.HasStatus<StoneStatus>(); } }

    public int ActiveStrength
    {
        get
        {
            var buff = BattlerStatus.GetStatus<BuffStatus>(s => s.Stat == BuffStatus.Stats.Strength);
            return (int)(Strength * (buff?.Level > 0 ? 1 + buff.Level * 0.5 : 1));
        }
    }

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

        //return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalMinDamage));

        var key = TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalMinDamage);
        var value = secondaryParameters.SumAllTrait(GetAllTraits(), key);
        var minMaxValue = secondaryParameters[SecondaryParameters.SecondaryParameterIndex.PhysicalMinDamage];
        minMaxValue.Value += Strength / 4;
        return minMaxValue.Value;
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
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalMaxDamage));
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


        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalHitRate));
    }

    /// <summary>
    /// Get the maximum hit number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getAttackMultiplier()
    {
        //int attMul = Agility / 16 + Level / 16 + 1;
        //return attMul < 16 ? attMul : 16;
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalAttackMultiplier));
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
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalDefense));
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
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalEvadeRate));
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
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalDefenseMultiplier));
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
        return secondaryParameters.SumAllTrait(GetAllTraits(), TraitKey.BuildSecondaryParamTrait(SecondaryParameters.SecondaryParameterIndex.PhysicalMinDamage));
    }

    public int getMagicMaxBaseDamage(int spellDamage)
    {
        var weaponDamage = 0;

        if (Weapon != null)
            weaponDamage += Weapon.MagicalMaxDamage;

        //return (Intelligence / 2) + spellDamage; //FF3
        //switch (damageOption)
        //{
        //    case eMagicalDamageOption.BLACK:
        //        return (Intelligence / 4) + (Level / 4) + spellDamage;
        //    case eMagicalDamageOption.WHITE:
        //        return (Wisdom / 4) + (Level / 4) + spellDamage;
        //    default:
        //        return (Intelligence / 8) + (Wisdom / 8) + (Level / 4) + spellDamage;
        //}
        return Intellect / 8 + Wisdom / 8 + Level / 4 + weaponDamage + spellDamage;
    }

    /// <summary>
    /// Get the magic hit pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicHitPourc(int spellHitPourc)
    {
        int weaponHitPourc = 0;

        if (Weapon != null)
            weaponHitPourc += Weapon.MagicalAccuracy;
        // 80% barehanded
        //return (Intelligence / 2) + spellHitPourc; //FF3
        //switch (damageOption)
        //{
        //    case eMagicalDamageOption.BLACK:
        //        return (Intelligence / 8) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        //    case eMagicalDamageOption.WHITE:
        //        return (Wisdom / 8) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        //    default:
        //        return (Intelligence / 16) + (Wisdom / 16) + (Accuracy / 8) + (Level / 4) + spellHitPourc;
        //}
        return Intellect / 16 + Wisdom / 16 + Level / 4 + weaponHitPourc + spellHitPourc;
    }

    /// <summary>
    /// Get the maximum magic hit number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicAttackMultiplier()
    {
        int attMul = 0;
        //switch (damageOption)
        //{
        //    case eMagicalDamageOption.BLACK:
        //        attMul = (Intelligence / 16) + (Level / 16) + 1;
        //        break;
        //    case eMagicalDamageOption.WHITE:
        //        attMul = (Wisdom / 16) + (Level / 16) + 1;
        //        break;
        //    default:
        //        attMul = (Intelligence / 32) + (Wisdom / 32) + (Level / 16) + 1;
        //        break;
        //}
        attMul = Intellect / 32 + Wisdom / 32 + Level / 16 + 1;
        return attMul < 16 ? attMul : 16;
    }

    /// <summary>
    /// Get the magic defense of the current job with the current equipement.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseDamage()
    {
        int armorsDefense = 0;

        if (Head != null)
            armorsDefense += Head.MagicalArmor;
        if (Body != null)
            armorsDefense += Body.MagicalArmor;
        if (Feet != null)
            armorsDefense += Feet.MagicalArmor;
        if (Offhand is ArmorData)
            armorsDefense += ((ArmorData)Offhand).MagicalArmor;

        return Math.Max(Wisdom / 2 + armorsDefense, 0);
    }

    /// <summary>
    /// Get the magic evade pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicEvadePourc()
    {
        int armorsEvadePourc = 0;

        if (Head != null)
            armorsEvadePourc += Head.MagicalEvasion;
        if (Body != null)
            armorsEvadePourc += Body.MagicalEvasion;
        if (Feet != null)
            armorsEvadePourc += Feet.MagicalEvasion;
        if (Offhand is ArmorData)
            armorsEvadePourc += ((ArmorData)Offhand).MagicalEvasion;

        return Math.Max(Agility / 8 + Wisdom / 8 + armorsEvadePourc, 0);
    }

    /// <summary>
    /// Get the maximum magic block number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseMultiplier()
    {
        return getNbShield() + 1;
        //return getNbShield() > 0 ? (Agility / 32 + Wisdom / 32 + Level / 16 + 1) * getNbShield() :
        //    Agility / 64 + Wisdom / 64 + Level / 32;
        //return (Agility / 32) + (Wisdom / 16); //FF3
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
        Damage damage;
        data.Effect.CalculateDamage(this, target, out damage, nbTarget);
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
        var damage = new Damage();
        damage.Type = Damage.eDamageType.HP;
        damage.User = this;
        damage.Target = target;

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
        defense *= BattlerStatus.HasStatus<GuardStatus>() ? 4 : 1;
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
        var damage = new Damage();
        damage.Type = Damage.eDamageType.HP;
        damage.User = this;
        damage.Target = target;

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
        if (parameters.Count != Enum.GetValues(typeof(Parameters.ParameterIndex)).Length)
        {
            var oldValues = parameters;
            parameters = new Parameters();
            for (int i = 0; i < oldValues.Count; i++)
            {
                parameters.ChangeValue((Parameters.ParameterIndex)i, oldValues[i].BaseValue);
            }
        }

        if (secondaryParameters.Count != Enum.GetValues(typeof(SecondaryParameters.SecondaryParameterIndex)).Length)
        {
            var oldValues = secondaryParameters;
            secondaryParameters = new SecondaryParameters();
            for (int i = 0; i < oldValues.Count; i++)
            {
                secondaryParameters.ChangeValue((SecondaryParameters.SecondaryParameterIndex)i, oldValues[i].Value);
            }
        }
    }
}
