using System;

[System.Serializable]
public class Route
{
    public string routeName;
    public int requiredCargo;
    public int recommendedCombat;
    public int durationMinutes; // before speed modifier
    public int hazardMinDamage;
    public int hazardMaxDamage;
    public int monsterMinPower;
    public int monsterMaxPower;
    public int coinReward;

    public Route(string name, int cargo, int combat, int duration, int hazardMin, int hazardMax, int monsterMin, int monsterMax, int reward)
    {
        routeName = name;
        requiredCargo = cargo;
        recommendedCombat = combat;
        durationMinutes = duration;
        hazardMinDamage = hazardMin;
        hazardMaxDamage = hazardMax;
        monsterMinPower = monsterMin;
        monsterMaxPower = monsterMax;
        coinReward = reward;
    }
}
