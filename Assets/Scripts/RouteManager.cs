using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteManager : MonoBehaviour
{
    public Text routeInfoText;
    public Text routeTimerText;
    public Button startRouteBtn;

    public FleetManager fleetManager; // Drag in from scene
    public Route sampleRoute; // You can add more later
    private bool routeInProgress = false;
    private float routeEndTime;

    private void Start()
    {
        // Sample route for World 1
        sampleRoute = new Route(
            "World 1: Diamond Haul",
            275, // cargo
            200, // combat
            420, // duration in minutes (7 hours)
            20, 60,   // Hazard Damage Range
            80, 120,  // Attack Power Range
            500  // reward in coins
        );

        startRouteBtn.onClick.AddListener(StartRoute);
        DisplayRouteInfo();
    }

    void DisplayRouteInfo()
    {
        routeInfoText.text =
        $"{sampleRoute.routeName}\n" +
        $"Trip Duration: {sampleRoute.durationMinutes} min (before speed bonus)\n" +
        $"Cargo Required: {sampleRoute.requiredCargo}\n" +
        $"Recommended Combat Power: {sampleRoute.recommendedCombat}\n" +
        $"Hazard Damage: {sampleRoute.hazardMinDamage} - {sampleRoute.hazardMaxDamage}\n" +
        $"Enemy Attack Power: {sampleRoute.monsterMinPower} - {sampleRoute.monsterMaxPower}\n" +
        $"Reward: {sampleRoute.coinReward} coins";
    }

    void StartRoute()
    {
        if (routeInProgress) return;

        Ship currentShip = fleetManager.GetCurrentShip();

        if (currentShip.cargo < sampleRoute.requiredCargo)
        {
            Debug.Log("Not enough cargo capacity.");
            return;
        }

        if (currentShip.durability <= 0)
        {
            Debug.Log("Ship is too damaged.");
            return;
        }

        // Adjust duration by speed (faster = shorter)
        float speedModifier = 1f - (currentShip.speed / 200f); // Cap at 50% reduction
        float finalDuration = sampleRoute.durationMinutes * 60f * Mathf.Clamp(speedModifier, 0.5f, 1f);

        routeEndTime = Time.time + finalDuration;
        routeInProgress = true;
        StartCoroutine(RouteTimer(finalDuration, currentShip));
    }

    IEnumerator RouteTimer(float duration, Ship ship)
    {
        while (Time.time < routeEndTime)
        {
            float remaining = routeEndTime - Time.time;
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            routeTimerText.text = $"Time Left: {minutes:D2}:{seconds:D2}";
            yield return null;
        }

        CompleteRoute(ship);
    }

    void CompleteRoute(Ship ship)
    {
        routeInProgress = false;

        // Random hazard damage
        int hazardDamage = Random.Range(sampleRoute.hazardMinDamage, sampleRoute.hazardMaxDamage + 1);
        ship.durability -= hazardDamage;

        // Random attack power
        int attackPower = Random.Range(sampleRoute.monsterMinPower, sampleRoute.monsterMaxPower + 1);
        int durabilityLoss = CalculateCombatDurabilityLoss(ship.combatPower, attackPower);
        ship.durability -= durabilityLoss;

        if (ship.durability <= 0)
        {
            Debug.Log("Ship broke mid-route. Returned with nothing.");
        }
        else
        {
            Debug.Log($"Route Complete! Earned {sampleRoute.coinReward} coins.");
            GameManager.Instance.AddCoins(sampleRoute.coinReward);
        }
    }
    
    int CalculateCombatDurabilityLoss(int shipCombat, int attackPower)
    {
        int difference = shipCombat - attackPower;

        if (difference < -50) return 200;
        else if (difference < 0) return 100;
        else if (difference < 50) return 50;
        else return 0;
    }

}
