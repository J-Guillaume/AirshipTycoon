using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainRouteManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI routeNameText;
    public ProgressBarUI progressBarUI;
    public BoostButton boostButton;

    [Header("Route Info")]
    private int worldIndex;
    private int routeSegment;
    private float routeDuration;
    private Ship assignedShip;

    private bool initialized = false;
    public GameObject collectRewardButton;
    [SerializeField] private int routeReward = 250;
    public GameObject startRouteButton;
    public HomePageManager homePageManager;


    public void Initialize(int worldIndex, int routeSegment, float duration, Ship ship)
    {
        this.worldIndex = worldIndex;
        this.routeSegment = routeSegment;
        this.routeDuration = duration;
        this.assignedShip = ship;
        collectRewardButton.SetActive(false); // make sure it starts hidden
        startRouteButton.SetActive(false);

        // Set "World x-y" route name
        routeNameText.text = $"World {worldIndex}-{routeSegment}";

        // Initialize progress bar UI
        if (progressBarUI != null)
            progressBarUI.Initialize(ship.shipSprite, duration, false); // No boost by default

        // Connect boost button event
        if (boostButton != null)
            boostButton.OnBoostClicked += ApplyBoost;

        initialized = true;
    }

    /// <summary>
    /// Called every frame by HomePageManager to update the route timer.
    /// </summary>
    public void Tick()
    {
        if (!initialized) return;

        progressBarUI.UpdateTimer();

        // OPTIONAL: Handle route completion
        if (progressBarUI.IsRouteComplete())
        {
            OnRouteComplete();
        }
    }

    private void ApplyBoost()
    {
        if (progressBarUI != null)
        {
            progressBarUI.ApplyBoost();
        }
    }

    private void OnRouteComplete()
    {
        // Example logic — adjust as needed
        Debug.Log($"Route World {worldIndex}-{routeSegment} is complete!");
        assignedShip.isAssigned = false;
        collectRewardButton.SetActive(true);
        GameManager.Instance.AddCoins(500); // or reward based on route data
    }

    public void ShowStartRouteButton()
    {
        startRouteButton.SetActive(true);
    }

    public void CollectReward()
    {
        GameManager.Instance.AddCoins(routeReward);
        assignedShip.isAssigned = false;
        collectRewardButton.SetActive(false);
        Debug.Log($"Player collected reward for World {worldIndex}-{routeSegment}!");
        ShowStartRouteButton();

    }

    public void AssignNextRoute()
    {
        routeSegment++;

        Ship newShip = FleetManager.Instance.GetUnassignedShips().FirstOrDefault();
        if (newShip == null)
        {
            Debug.LogWarning("No unassigned ships available!");
            return;
        }

        assignedShip = newShip;
        assignedShip.isAssigned = true;

        Initialize(worldIndex, routeSegment, routeDuration, newShip);
    }

    public void StartNextRoute()
    {
        homePageManager.OpenShipSelection(
            $"Select a ship for World {worldIndex}-{routeSegment + 1}",
            (Ship selectedShip) =>
            {
                routeSegment++;
                Initialize(worldIndex, routeSegment, routeDuration, selectedShip);
            }
        );
    }

    private void Awake()
    {
        if (homePageManager == null)
            homePageManager = FindObjectOfType<HomePageManager>();
    }
}
