using UnityEngine;

public class Battler : MonoBehaviour
{
    private int Level = 2;

    public int BaseMaxHP;
    public int BaseMaxSP;
    public int Strength;
    public int Vitality;
    public int Agility;

    public int CurrentHP;
    public int CurrentSP;

    public int MaxHP { get { return BaseMaxHP + Vitality / 4 * (Level - 1) + (Level - 1) * 10; } }
    public int MaxSP { get { return BaseMaxSP + Vitality / 4 * (Level - 1) + (Level - 1) * 5; } }//TODO: Remplacer Vitality par Wisdom

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
    #endregion
}
