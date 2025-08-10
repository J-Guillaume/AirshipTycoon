using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RouteGeneratorClass
{
    public static RouteData GenerateRandomRoute(int worldIndex)
    {
        RouteData route = new RouteData();

        route.routeTitle = GenerateTitle(worldIndex);
        route.cargoManifest = GenerateRandomManifest(worldIndex, out int totalCargo);
        route.dangerStars = CalculateDangerLevel(totalCargo);
        route.encounters = GenerateEncounters(route.cargoManifest, route.BaseDuration, worldIndex);
        route.durabilityCost = CalculateDurabilityCost(route);

        route.recommendedCombatPower = EstimateCombatPower(route.encounters);
        route.requiresSpeed = route.cargoManifest.Any(r => r.resource.resourceType == ResourceType.Perishable);
        route.minRequiredSpeed = route.requiresSpeed ? UnityEngine.Random.Range(20, 60) : 0;

        return route;
    }

    private static List<RouteCargoEntry> GenerateRandomManifest(int worldIndex, out int totalCargo)
    {
        var availableResources = ResourceDatabase.GetResourcesForWorld(worldIndex); // Implement this
        int resourceCount = UnityEngine.Random.Range(1, 6);

        List<RouteCargoEntry> manifest = new();
        totalCargo = 0;

        for (int i = 0; i < resourceCount; i++)
        {
            RouteResource resource = availableResources[UnityEngine.Random.Range(0, availableResources.Count)];
            int unitCount = UnityEngine.Random.Range(5, 25);

            // ✅ Adjusted coin value based on world progression
            int adjustedCoinValue = CalculateAdjustedReward(resource, worldIndex);

            // Clone the resource data for this entry to avoid modifying the original
            RouteResource adjustedResource = new RouteResource
            {
                resourceName = resource.resourceName,
                resourceType = resource.resourceType,
                cargoPerUnit = resource.cargoPerUnit,
                coinPerUnit = adjustedCoinValue,
                sprite = resource.sprite,
                worldUnlocked = resource.worldUnlocked
            };

            var entry = new RouteCargoEntry
            {
                resource = resource,
                unitCount = unitCount
            };

            manifest.Add(entry);
            totalCargo += entry.TotalCargo;
        }

        return manifest;
    }

    private static List<RouteEncounter> GenerateEncounters(List<RouteCargoEntry> manifest, TimeSpan duration, int worldIndex)
    {
        List<RouteEncounter> encounters = new();
        int count = GetEncounterCount(duration);

        bool carryingPerishable = manifest.Any(e => e.resource.resourceType == ResourceType.Perishable);
        bool carryingValuable = manifest.Any(e => e.resource.resourceType == ResourceType.Valuable);
        bool carryingLivestock = manifest.Any(e => e.resource.resourceName.ToLower().Contains("livestock"));

        for (int i = 0; i < count; i++)
        {
            EncounterType type;

            if (i == 0 && carryingValuable) type = EncounterType.Bandits;
            else if (i == 0 && carryingLivestock) type = EncounterType.Monsters;
            else if (carryingPerishable && UnityEngine.Random.value < 0.4f) type = EncounterType.Monsters;
            else if (carryingValuable && UnityEngine.Random.value < 0.4f) type = EncounterType.Bandits;
            else type = EncounterType.Storm;

            int strength = UnityEngine.Random.Range(10 + worldIndex * 5, 30 + worldIndex * 10);
            if (carryingLivestock && type == EncounterType.Monsters)
                strength = (int)(strength * 1.5f);

            encounters.Add(new RouteEncounter { type = type, strength = strength });
        }

        return encounters;
    }

    private static int GetEncounterCount(TimeSpan duration)
    {
        double minutes = duration.TotalMinutes;
        if (minutes >= 1440) return 5;
        if (minutes >= 60 * 12) return 4;
        if (minutes >= 60) return 3;
        if (minutes >= 15) return 2;
        return 1;
    }

    private static int CalculateDangerLevel(int totalCargo)
    {
        // You can refine this based on route length, encounter count, etc.
        if (totalCargo > 2000) return 3;
        if (totalCargo > 1000) return 2;
        return 1;
    }

    private static int CalculateDurabilityCost(RouteData route)
    {
        return 5 + route.encounters.Count * 2 + (int)(route.BaseDuration.TotalHours / 2);
    }

    private static string GenerateTitle(int worldIndex)
    {
        string[] baseNames = { "Scout Run", "Supply Drop", "Danger Route", "Valley Run", "Mystic Errand" };
        string name = baseNames[UnityEngine.Random.Range(0, baseNames.Length)];
        return $"W{worldIndex}-{UnityEngine.Random.Range(1, 100)}: {name}";
    }

    private static int EstimateCombatPower(List<RouteEncounter> encounters)
    {
        // Basic logic: sum strength of all non-storm encounters
        int combatPower = 0;

        foreach (var e in encounters)
        {
            if (e.type == EncounterType.Bandits || e.type == EncounterType.Monsters)
            {
                combatPower += e.strength;
            }
        }

        return Mathf.Max(10, combatPower); // Minimum recommended CP
    }

    private static int CalculateAdjustedReward(RouteResource resource, int currentWorld)
    {
        float multiplier = 1f + (currentWorld - resource.worldUnlocked) * 0.25f;
        return Mathf.RoundToInt(resource.coinPerUnit * multiplier);
    }
}
