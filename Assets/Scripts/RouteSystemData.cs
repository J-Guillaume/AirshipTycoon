using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResourceType { Common, Valuable, Perishable }
public enum EncounterType { Storm, Bandits, Monsters }

[Serializable]
public class RouteResource
{
    public string resourceName;
    public ResourceType resourceType;
    public int cargoPerUnit;
    public int coinPerUnit;
    public Sprite sprite;
    public int worldUnlocked;
}

[Serializable]
public class RouteCargoEntry
{
    public RouteResource resource;
    public int unitCount;

    public int TotalCargo => resource.cargoPerUnit * unitCount;
    public int TotalReward => resource.coinPerUnit * unitCount;
}

[Serializable]
public class RouteEncounter
{
    public EncounterType type;
    public int strength;
}

[Serializable]
public class RouteData
{
    public string routeTitle;
    public List<RouteCargoEntry> cargoManifest = new();
    public List<RouteEncounter> encounters = new();

    public int dangerStars; // 1 to 3
    public int recommendedCombatPower;
    public int minRequiredSpeed;
    public bool requiresSpeed;
    public int durabilityCost;
    

    public int TotalCargo => cargoManifest.Sum(entry => entry.TotalCargo);
    public int TotalReward => cargoManifest.Sum(entry => entry.TotalReward);
    public TimeSpan BaseDuration => RouteDurationCalculator.CalculateDuration(TotalCargo);
}

public static class RouteDurationCalculator
{
    public static TimeSpan CalculateDuration(int cargo)
    {
        int minutes = (cargo % 100) / 10;
        int hours = (cargo % 1000) / 100;
        int days = cargo / 1000;
        return new TimeSpan(days, hours, minutes, 0);
    }
}
