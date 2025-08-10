using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarUI : MonoBehaviour
{
    public Image progressFill;
    public TextMeshProUGUI countdownText;
    public Image shipIcon;

    private float totalDuration;
    private float endTime;
    private bool boosted = false;

    public void Initialize(Sprite shipSprite, float durationSeconds, bool isBoosted)
    {
        shipIcon.sprite = shipSprite;
        totalDuration = durationSeconds;
        boosted = isBoosted;
        endTime = Time.time + (boosted ? durationSeconds * 0.5f : durationSeconds);
    }

    public void UpdateTimer()
    {
        float timeLeft = endTime - Time.time;
        if (timeLeft < 0f) timeLeft = 0f;

        float progress = 1f - Mathf.Clamp01(timeLeft / totalDuration);
        progressFill.fillAmount = progress;
        countdownText.text = FormatTime(timeLeft);
        Debug.Log("Progress Fill: " + progressFill);

        // Move ship icon
        float width = ((RectTransform)progressFill.transform).rect.width;
        Vector3 pos = shipIcon.rectTransform.localPosition;
        pos.x = Mathf.Lerp(0, width, progress);
        shipIcon.rectTransform.localPosition = pos;
    }

    string FormatTime(float t)
    {
        int h = Mathf.FloorToInt(t / 3600);
        int m = Mathf.FloorToInt((t % 3600) / 60);
        int s = Mathf.FloorToInt(t % 60);
        return $"{h:D2}:{m:D2}:{s:D2}";
    }

    public void ApplyBoost()
    {
        if (!boosted)
        {
            boosted = true;
            float remaining = endTime - Time.time;
            endTime = Time.time + (remaining * 0.5f); // Reduce remaining time by 50%
        }
    }

    public bool IsRouteComplete()
    {
        return Time.time >= endTime;
    }

}
