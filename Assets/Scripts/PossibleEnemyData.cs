using System;

[Serializable]
public class PossibleEnemyData
{
    public string EnemyName;
    public int Frequency;

    public PossibleEnemyData(string enemyName, int frequency)
    {
        EnemyName = enemyName;
        Frequency = frequency;
    }
}
