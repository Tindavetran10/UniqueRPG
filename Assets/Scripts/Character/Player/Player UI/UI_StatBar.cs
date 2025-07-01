using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;

    // Variable to Scale Bar size Depending on Stat
    [Header("Bar Options")]
    [SerializeField] protected bool scaleBarLengthWithStats = true;
    [SerializeField] protected float widthScaleMultiplier = 1;

    // Secondary Bar for Polish effect

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

        if (scaleBarLengthWithStats)
        {
            rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);

            PlayerUIManager.instance.playerUIHudManager.RefreshHUD();
        }
    }
}
