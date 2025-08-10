using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RouteOptionCardUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI rewardText;
    public Image[] starIcons;

    private RouteData routeData;
    private AddRoutePanelManager parentPanel;
    private RouteInfoPanelUI detailsPanel;

    public void Initialize(RouteData data, AddRoutePanelManager panel)
    {
        routeData = data;
        parentPanel = panel;
        detailsPanel = panel.detailedRouteInfoUI;

        titleText.text = data.routeTitle;
        durationText.text = FormatDuration(data.BaseDuration);
        rewardText.text = $"Reward: {data.TotalReward}";

        for (int i = 0; i < starIcons.Length; i++)
            starIcons[i].enabled = i < data.dangerStars;
    }

    private string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
            return $"{(int)duration.TotalDays}d {(int)duration.Hours}h {(int)duration.Minutes}m";
        else if (duration.TotalHours >= 1)
            return $"{(int)duration.TotalHours}h {(int)duration.Minutes}m";
        else
            return $"{(int)duration.TotalMinutes}m";
    }

    public void OnCardClicked()
    {
        detailsPanel.Open(routeData, (route, ship, autoRerun) =>
        {
            // Handle assigning the route
            RouteAssigner.AssignToSlot(route, ship, autoRerun);
        });
    }
}
