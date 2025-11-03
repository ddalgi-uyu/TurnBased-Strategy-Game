using System;
using TMPro;
using UnityEngine;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;
    [SerializeField] private Unit unit;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        UpdateActionPointText();
    }

    private void UpdateActionPointText()
    {
        actionPointText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs args)
    {
        UpdateActionPointText();
    }
}
