using UnityEngine;

[System.Serializable]
public class Ship
{
    public string shipName;
    public Sprite shipSprite;
    public bool isAssigned = false;

    // Base attributes
    public int speed;
    public int combatPower;
    public int cargo;
    public int durability;
    public int maxDurability;

    // Upgrade levels
    public int speedLevel = 1;
    public int combatLevel = 1;
    public int cargoLevel = 1;
    public int durabilityLevel = 1;

    // Constructor
    public Ship(string name, int speed, int combat, int cargo, int durability)
    {
        this.shipName = name;
        this.speed = speed;
        this.combatPower = combat;
        this.cargo = cargo;
        this.durability = durability;
        this.maxDurability = durability;
    }

    public void UpgradeSpeed()
    {
        speed += 5; // Adjust as needed
        speedLevel++;
    }

    public void UpgradeCombat()
    {
        combatPower += 10;
        combatLevel++;
    }

    public void UpgradeCargo()
    {
        cargo += 25;
        cargoLevel++;
    }

    public void UpgradeDurability()
    {
        maxDurability += 20;
        durability = maxDurability; // Refresh on upgrade
        durabilityLevel++;
    }
}
