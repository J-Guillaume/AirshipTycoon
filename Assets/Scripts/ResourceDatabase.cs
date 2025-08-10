using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceDatabase
{
    public static List<RouteResource> GetResourcesForWorld(int world)
    {
        return MockResources.FindAll(r => r.worldUnlocked <= world);
    }

    private static List<RouteResource> MockResources = new List<RouteResource>
    {
        // World 1 (basic resources)
        new RouteResource { resourceName = "Wood", resourceType = ResourceType.Common, cargoPerUnit = 1, coinPerUnit = 2, worldUnlocked = 1 },
        new RouteResource { resourceName = "Iron", resourceType = ResourceType.Common, cargoPerUnit = 1, coinPerUnit = 4, worldUnlocked = 1 },
        new RouteResource { resourceName = "Gems", resourceType = ResourceType.Valuable, cargoPerUnit = 3, coinPerUnit = 25, worldUnlocked = 1 },
        new RouteResource { resourceName = "Food", resourceType = ResourceType.Perishable, cargoPerUnit = 2, coinPerUnit = 10, worldUnlocked = 1 },
        
        // World 2 (fire theme)
        new RouteResource { resourceName = "Sulfur", resourceType = ResourceType.Common, cargoPerUnit = 3, coinPerUnit = 6, worldUnlocked = 2 },
        new RouteResource { resourceName = "Fire Crystals", resourceType = ResourceType.Valuable, cargoPerUnit = 4, coinPerUnit = 20, worldUnlocked = 2 },
        new RouteResource { resourceName = "Smoldering Meat", resourceType = ResourceType.Perishable, cargoPerUnit = 2, coinPerUnit = 12, worldUnlocked = 2 },

        // World 3 (water theme)
        new RouteResource { resourceName = "Fresh Fish", resourceType = ResourceType.Perishable, cargoPerUnit = 2, coinPerUnit = 10, worldUnlocked = 3 },
        new RouteResource { resourceName = "Pearls", resourceType = ResourceType.Valuable, cargoPerUnit = 1, coinPerUnit = 25, worldUnlocked = 3 },
        new RouteResource { resourceName = "Seaweed Bundles", resourceType = ResourceType.Common, cargoPerUnit = 1, coinPerUnit = 5, worldUnlocked = 3 },

        // World 4 (lightning theme)
        new RouteResource { resourceName = "Charged Ore", resourceType = ResourceType.Common, cargoPerUnit = 3, coinPerUnit = 8, worldUnlocked = 4 },
        new RouteResource { resourceName = "Thunder Gems", resourceType = ResourceType.Valuable, cargoPerUnit = 2, coinPerUnit = 30, worldUnlocked = 4 },
        new RouteResource { resourceName = "Lightning Rods", resourceType = ResourceType.Valuable, cargoPerUnit = 5, coinPerUnit = 40, worldUnlocked = 4 },
    };
}
