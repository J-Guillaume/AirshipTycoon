using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneSetUp : MonoBehaviour
{
    public FleetManager fleetManager;
    public HomePageManager homePageManager;

    private void Start()
    {
        // ✅ 1. Make sure FleetManager has test ships
        if (fleetManager.fleet.Count == 0)
        {
            fleetManager.fleet.Add(new Ship("Test Ship Alpha", 50, 100, 200, 100));
            fleetManager.fleet.Add(new Ship("Test Ship Beta", 70, 80, 150, 90));
            fleetManager.fleet.Add(new Ship("Test Ship Gamma", 60, 120, 180, 110));
        }

        // ✅ 2. Ensure no ship is marked assigned
        foreach (var ship in fleetManager.fleet)
            ship.isAssigned = false;

        // ✅ 3. Initialize the Home Page UI if available
        if (homePageManager != null)
        {
            homePageManager.OpenHomePage();
            Debug.Log("✅ Test Scene Setup: Home Page opened with test ships.");
        }
        else
        {
            Debug.LogWarning("⚠️ HomePageManager not assigned in TestSceneSetup.");
        }
    }
}
