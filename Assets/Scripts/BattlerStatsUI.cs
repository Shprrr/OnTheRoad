﻿using UnityEngine;
using UnityEngine.UI;

public class BattlerStatsUI : MonoBehaviour
{
    public Battler battler;

    public Text currentHP;
    public Text maxHP;
    public Text currentSP;
    public Text maxSP;

    public Text strength;
    public Text vitality;
    public Text intellect;
    public Text wisdom;
    public Text agility;

    public Text physicalDamage;
    public Text physicalAccuracy;
    public Text physicalDefense;
    public Text physicalEvasion;

    public Text magicalDamage;
    public Text magicalAccuracy;
    public Text magicalDefense;
    public Text magicalEvasion;

    public Button weapon;
    public Button offhand;
    public Button head;
    public Button body;
    public Button feet;
    public Button neck;
    public Button finger1;
    public Button finger2;

    // Awake est appelé quand l'instance de script est chargée
    private void Awake()
    {
        battler = GetComponent<Battler>();
    }

    // Update est appelé pour chaque trame, si le MonoBehaviour est activé
    private void Update()
    {
        currentHP.text = battler.CurrentHP.ToString("### ##0").Trim();
        maxHP.text = battler.MaxHP.ToString("### ##0").Trim();
        currentSP.text = battler.CurrentSP.ToString("##0");
        maxSP.text = battler.MaxSP.ToString("##0");

        if (strength != null) strength.text = battler.Strength.ToString("##0");
        if (vitality != null) vitality.text = battler.Vitality.ToString("##0");
        if (intellect != null) intellect.text = battler.Intellect.ToString("##0");
        if (wisdom != null) wisdom.text = battler.Wisdom.ToString("##0");
        if (agility != null) agility.text = battler.Agility.ToString("##0");

        if (physicalDamage != null)
        {
            var baseDamage = battler.getBaseDamage();
            physicalDamage.text = baseDamage.ToString("##0") + " - " + ((int)(baseDamage * 1.5)).ToString("##0");
        }

        if (physicalAccuracy != null)
            physicalAccuracy.text = battler.getAttackMultiplier().ToString("#0") + "x" + battler.getHitPourc().ToString("##0") + "%";

        if (physicalDefense != null)
            physicalDefense.text = battler.getDefenseDamage().ToString("##0");

        if (physicalEvasion != null)
            physicalEvasion.text = battler.getDefenseMultiplier().ToString("#0") + "x" + battler.getEvadePourc().ToString("##0") + "%";

        if (magicalDamage != null)
        {
            var baseDamage = battler.getMagicBaseDamage(0);
            magicalDamage.text = baseDamage.ToString("##0") + " - " + ((int)(baseDamage * 1.5)).ToString("##0");
        }

        if (magicalAccuracy != null)
            magicalAccuracy.text = battler.getMagicAttackMultiplier().ToString("#0") + "x" + battler.getMagicHitPourc(80).ToString("##0") + "%";

        if (magicalDefense != null)
            magicalDefense.text = battler.getMagicDefenseDamage().ToString("##0");

        if (magicalEvasion != null)
            magicalEvasion.text = battler.getMagicDefenseMultiplier().ToString("#0") + "x" + battler.getMagicEvadePourc().ToString("##0") + "%";

        if (weapon != null)
            weapon.GetComponentInChildren<Text>().text = battler.weapon?.Name;

        if (offhand != null)
            offhand.GetComponentInChildren<Text>().text = battler.offhand?.Name;

        if (head != null)
            head.GetComponentInChildren<Text>().text = battler.head?.Name;

        if (body != null)
            body.GetComponentInChildren<Text>().text = battler.body?.Name;

        if (feet != null)
            feet.GetComponentInChildren<Text>().text = battler.feet?.Name;

        if (neck != null)
            neck.GetComponentInChildren<Text>().text = battler.neck?.Name;

        if (finger1 != null)
            finger1.GetComponentInChildren<Text>().text = battler.finger1?.Name;

        if (finger2 != null)
            finger2.GetComponentInChildren<Text>().text = battler.finger2?.Name;
    }
}
