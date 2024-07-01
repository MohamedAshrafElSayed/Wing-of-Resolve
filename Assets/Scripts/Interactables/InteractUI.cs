using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject _containerGO;
    [SerializeField] private TextMeshProUGUI _tooltipText;

    public void Show()
    {
        _containerGO.SetActive(true);
    }

    public void Hide()
    {
        _containerGO.SetActive(false);
        _tooltipText.SetText("");
    }

    public void SetTooltip(string tooltip)
    {
        _tooltipText.SetText(tooltip);
    }
}
