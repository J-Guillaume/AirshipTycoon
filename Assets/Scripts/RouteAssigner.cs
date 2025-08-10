using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RouteAssigner
{
    public static void AssignToSlot(RouteData route, Ship ship, bool autoRerun)
    {
        // ✅ Mark the ship as assigned
        ship.isAssigned = true;

        // ✅ Find the first available small route slot
        SmallRouteSlotUI[] slots = Object.FindObjectsOfType<SmallRouteSlotUI>();

        foreach (var slot in slots)
        {
            if (!slot.IsActive) // Assuming you have a method to check if slot is in use
            {
                slot.Initialize(ship, (float)route.BaseDuration.TotalSeconds, autoRerun, route.TotalReward);
                Debug.Log($"Assigned {ship.shipName} to route: {route.routeTitle}");
                return;
            }
        }

        Debug.LogWarning("No free Small Route slot available to assign this route.");
    }
}
