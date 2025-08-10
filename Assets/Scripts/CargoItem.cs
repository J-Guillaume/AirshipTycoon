using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CargoItem : MonoBehaviour
{
     public TextMeshProUGUI nameText;
    public TextMeshProUGUI unitsText;
    public TextMeshProUGUI cargoText;
    public TextMeshProUGUI rewardText;
    public Image iconImage;

    public void Initialize(RouteCargoEntry entry)
    {
        nameText.text = entry.resource.resourceName;
        unitsText.text = $"Units: {entry.unitCount}";
        cargoText.text = $"Cargo: {entry.TotalCargo}";
        rewardText.text = $"Coins: {entry.TotalReward}";

        if (entry.resource.sprite != null)
        iconImage.sprite = entry.resource.sprite;
    }
}
