using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePageManager : MonoBehaviour
{
    [Header("Main Route Section")]
    public MainRouteManager mainRouteManager;

    [Header("Small Route Slots")]
    public List<SmallRouteSlotUI> smallRouteSlots;
    public List<GameObject> addRouteButtons; // if no ship assigned

    [Header("Ship Panel")]
    public ShipPanelUI shipPanelUI;
    private List<Ship> unassignedShips;
    private int currentShipIndex = 0;

    public ShipSelectionPopup shipSelectionPopup;

    public static HomePageManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Load all unassigned ships
        unassignedShips = FleetManager.Instance.GetUnassignedShips();
        currentShipIndex = 0;

        if (unassignedShips.Count > 0)
            shipPanelUI.ShowShip(unassignedShips[0], unassignedShips);

        // Initialize Main Route Section
        mainRouteManager.Initialize(worldIndex: 1, routeSegment: 1, duration: 3600f, ship: unassignedShips[0]);
    }

    void Update()
    {
        mainRouteManager.Tick(); // Calls UpdateTimer() inside MainRoute
        foreach (var slot in smallRouteSlots)
        {
            slot.Tick();
        }

        foreach (var slot in smallRouteSlots)
        {
            if (slot.IsActive)
                slot.Tick();
        }
    }

    public void ShowNextShip()
    {
        currentShipIndex = (currentShipIndex + 1) % unassignedShips.Count;
        shipPanelUI.ShowShip(unassignedShips[currentShipIndex], unassignedShips);
    }

    public void ShowPreviousShip()
    {
        currentShipIndex = (currentShipIndex - 1 + unassignedShips.Count) % unassignedShips.Count;
        shipPanelUI.ShowShip(unassignedShips[currentShipIndex], unassignedShips);
    }

    public void AssignSmallRoute(int slotIndex, Ship ship, float duration)
    {
        smallRouteSlots[slotIndex].Initialize(ship, duration, false, reward: 200);
        addRouteButtons[slotIndex].SetActive(false);
    }

    public void ShowAddRouteButtonIfEmpty(int slotIndex)
    {
        if (!smallRouteSlots[slotIndex].IsActive)
        {
            addRouteButtons[slotIndex].SetActive(true);
        }
    }

    public void OpenShipSelection(string title, Action<Ship> onShipSelected)
    {
        var unassignedShips = FleetManager.Instance.GetUnassignedShips();
        shipSelectionPopup.Open(title, unassignedShips, onShipSelected);
    }
    
    public void OpenHomePage()
    {
        gameObject.SetActive(true);
        Debug.Log("✅ Home Page opened.");
    }
}
