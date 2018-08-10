﻿using System.Linq;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Battler : MonoBehaviour
{
    private Animator animator;
    private int Level = 2;

    public GameObject animationPrefab;
    public GameObject targetPrefab;
    public bool IsPlayer;

    public int BaseMaxHP;
    public int BaseMaxSP;
    public int Strength;
    public int Vitality;
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
    public int MaxSP { get { return BaseMaxSP + Vitality / 4 * (Level - 1) + (Level - 1) * 5; } }//TODO: Remplacer Vitality par Wisdom

    public SkillData[] skills;
    public string animationAttack;
    public BattleAction? lastAction;

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

        // Put skills by Unity in the factory
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i] = SkillFactory.Build(skills[i].Name);
        }
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
        //CurrentHP = Hp;
        //CurrentSP = Sp;
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
    public bool CantFight { get { return IsDead; } }//TODO: Add "or HasStatus=Stone"

    /// <summary>
    /// Get the base damage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <returns></returns>
    public int getBaseDamage()
    {
        int weaponDamage = 0;

        //if (damageOption != ePhysicalDamageOption.LEFT && RightHand is Weapon)
        //    weaponDamage += ((Weapon)RightHand).Damage;
        //if (damageOption != ePhysicalDamageOption.RIGHT && LeftHand is Weapon)
        //    weaponDamage += ((Weapon)LeftHand).Damage;
        //if (RightHand == null && LeftHand == null) // Est-ce que Shield est barehand ?
        weaponDamage = 1; // Barehand

        return Strength / 4 + Level / 4 + weaponDamage;
    }

    /// <summary>
    /// Get the hit pourcentage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <returns></returns>
    public int getHitPourc()
    {
        int weaponHitPourc = 0;

        //if (damageOption != ePhysicalDamageOption.LEFT && RightHand is Weapon)
        //    weaponHitPourc += ((Weapon)RightHand).HitPourc;
        //if (damageOption != ePhysicalDamageOption.RIGHT && LeftHand is Weapon)
        //    weaponHitPourc += ((Weapon)LeftHand).HitPourc;
        //if (damageOption == ePhysicalDamageOption.BOTH && RightHand is Weapon && LeftHand is Weapon)
        //    weaponHitPourc /= 2; // On a additionné 2 fois un 100%, donc on remet sur 100%
        //if (RightHand == null && LeftHand == null) // Est-ce que Shield est barehand ?
        weaponHitPourc = 80; // Barehand

        return weaponHitPourc;
    }

    /// <summary>
    /// Get the maximum hit number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getAttackMultiplier()
    {
        int attMul = Agility / 16 + Level / 16 + 1;
        return attMul < 16 ? attMul : 16;
    }

    /// <summary>
    /// Get the defense of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getDefenseDamage()
    {
        int armorsDefense = 0;

        //if (Head != null)
        //    armorsDefense += Head.DefenseValue;
        //if (Body != null)
        //    armorsDefense += Body.DefenseValue;
        //if (Arms != null)
        //    armorsDefense += Arms.DefenseValue;
        //if (Feet != null)
        //    armorsDefense += Feet.DefenseValue;
        //if (RightHand is Shield)
        //    armorsDefense += ((Shield)RightHand).DefenseValue;
        //if (LeftHand is Shield)
        //    armorsDefense += ((Shield)LeftHand).DefenseValue;

        return Vitality / 2 + armorsDefense;
    }

    /// <summary>
    /// Get the evade pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getEvadePourc()
    {
        int armorsEvadePourc = 0;

        //if (Head != null)
        //    armorsEvadePourc += Head.EvadePourc;
        //if (Body != null)
        //    armorsEvadePourc += Body.EvadePourc;
        //if (Arms != null)
        //    armorsEvadePourc += Arms.EvadePourc;
        //if (Feet != null)
        //    armorsEvadePourc += Feet.EvadePourc;
        //if (RightHand is Shield)
        //    armorsEvadePourc += ((Shield)RightHand).EvadePourc;
        //if (LeftHand is Shield)
        //    armorsEvadePourc += ((Shield)LeftHand).EvadePourc;

        return Agility / 4 + armorsEvadePourc;
    }

    /// <summary>
    /// Get the number of shield currently equiped.
    /// </summary>
    /// <returns></returns>
    private int getNbShield()
    {
        int nbShield = 0;

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
        return getNbShield() > 0 ? (Agility / 16 + Level / 16 + 1) * getNbShield() :
            Agility / 32 + Level / 32;
    }

    /// <summary>
    /// Get the magic base damage of the current job with the current equipment.
    /// </summary>
    /// <param name="damageOption"></param>
    /// <param name="spellDamage"></param>
    /// <returns></returns>
    public int getMagicBaseDamage(int spellDamage)
    {
        //TODO: Revert to old formula.
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
        return getBaseDamage() + spellDamage;
    }

    /// <summary>
    /// Get the magic hit pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicHitPourc(int spellHitPourc)
    {
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
        return getHitPourc() + spellHitPourc;
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
        attMul = getAttackMultiplier();
        return attMul < 16 ? attMul : 16;
    }

    /// <summary>
    /// Get the magic defense of the current job with the current equipement.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseDamage()
    {
        int armorsDefense = 0;

        //if (Head != null)
        //    armorsDefense += Head.MagicDefenseValue;
        //if (Body != null)
        //    armorsDefense += Body.MagicDefenseValue;
        //if (Arms != null)
        //    armorsDefense += Arms.MagicDefenseValue;
        //if (Feet != null)
        //    armorsDefense += Feet.MagicDefenseValue;
        //if (RightHand is Shield)
        //    armorsDefense += ((Shield)RightHand).MagicDefenseValue;
        //if (LeftHand is Shield)
        //    armorsDefense += ((Shield)LeftHand).MagicDefenseValue;

        //return (Wisdom / 2) + armorsDefense;
        return getDefenseDamage();
    }

    /// <summary>
    /// Get the magic evade pourcentage of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicEvadePourc()
    {
        int armorsEvadePourc = 0;

        //if (Head != null)
        //    armorsEvadePourc += Head.MagicEvadePourc;
        //if (Body != null)
        //    armorsEvadePourc += Body.MagicEvadePourc;
        //if (Arms != null)
        //    armorsEvadePourc += Arms.MagicEvadePourc;
        //if (Feet != null)
        //    armorsEvadePourc += Feet.MagicEvadePourc;
        //if (RightHand is Shield)
        //    armorsEvadePourc += ((Shield)RightHand).MagicEvadePourc;
        //if (LeftHand is Shield)
        //    armorsEvadePourc += ((Shield)LeftHand).MagicEvadePourc;

        //return (Agility / 8) + (Wisdom / 8) + armorsEvadePourc;
        return getEvadePourc();
    }

    /// <summary>
    /// Get the maximum magic block number times of the current job with the current equipment.
    /// </summary>
    /// <returns></returns>
    public int getMagicDefenseMultiplier()
    {
        //return getNbShield() > 0 ? ((Agility / 32) + (Wisdom / 32) + (Level / 16) + 1) * getNbShield() :
        //    (Agility / 64) + (Wisdom / 64) + (Level / 32);
        //return (Agility / 32) + (Wisdom / 16); //FF3
        return getDefenseMultiplier();
    }
    #endregion

    #region Battle Methods
    public void Attacks(Battler target)
    {
        var damage = CalculatePhysicalDamage(target);
        Debug.LogFormat("{0} attacks {1} for {2}", name, target.name, damage);
        var taking = Instantiate(animationPrefab, target.transform).GetComponent<TakingDamage>();
        taking.damage = damage;

        if (animationsBundle == null)
            Debug.LogError("animationsBundle is null");
        taking.animationAttack = animationsBundle.LoadAsset<AnimatorOverrideController>(animationAttack);
    }

    public bool Casts(SkillData skill, out int skillLevel)
    {
        skillLevel = 1;
        if (!skills.Contains(skill) || Sp < skill.SpCost) return false;

        Sp -= skill.SpCost;
        return true;
    }

    public void Used(Battler target, SkillData skill, int skillLevel, int nbTarget)
    {
        Damage damage;
        skill.Effect.CalculateDamage(this, target, out damage);
        Debug.LogFormat("{0} used {3} on {1} for {2}", name, target.name, damage, skill.Name);
        var taking = Instantiate(animationPrefab, target.transform).GetComponent<TakingDamage>();
        taking.damage = damage;

        if (animationsBundle == null)
            Debug.LogError("animationsBundle is null");
        taking.animationAttack = animationsBundle.LoadAsset<AnimatorOverrideController>(skill.AnimationName);
    }

    public Damage CalculatePhysicalDamage(Battler target)
    {
        var damage = new Damage();
        damage.Type = Damage.eDamageType.HP;
        damage.User = this;
        damage.Target = target;

        //Calculate min and max base damage
        int baseMinDmg = getBaseDamage();

        //Bonus on base damage for Attacker
        //baseMinDmg += HasCheer ? 10 * CheerLevel : 0;
        //ou
        //baseMinDmg += HasCheer ? baseMinDmg * CheerLevel / 15 : 0;
        //baseMinDmg *= IsAlly ? 2 : 1;
        //baseMinDmg *= ElementalEffect(attacker);
        //baseMinDmg *= IsMini || IsToad ? 2 : 1;
        //baseMinDmg *= attacker->IsMini || attacker->IsToad ? 0 : 1;

        int baseMaxDmg = (int)(baseMinDmg * 1.5);

        //Calculate hit%
        int hitPourc = getHitPourc();
        hitPourc = (hitPourc < 99 ? hitPourc : 99);
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
        //defense *= (IsDefending ? 4 : 1);
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

        damage.Value = (Random.Range(baseMinDmg, baseMaxDmg + 1) - defense) * damage.Multiplier;
        //damage *= AttackIsJump ? 3 : 1;

        //Validate final damage and multiplier
        if (damage.Value < 1) //Min 1 s'il tape au moins une fois
            damage.Value = 1;

        if (damage.Multiplier < 1) //Check s'il tape au moins une fois
            damage.Value = 0;

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
        int baseMinDmg = getMagicBaseDamage(spellDamage);

        //Bonus on base damage for Attacker
        //baseMinDmg *= ElementalEffect(attacker);
        //baseMinDmg *= IsMini || IsToad ? 2 : 1;

        int baseMaxDmg = (int)(baseMinDmg * 1.5);

        //Calculate hit%
        int hitPourc = 0;
        if (isItem)
            hitPourc = 100;
        else
        {
            hitPourc = getMagicHitPourc(spellHitPourc);
            hitPourc = (hitPourc < 99 ? hitPourc : 99);
        }

        //Calculate attack multiplier
        if (!isItem)
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
}
