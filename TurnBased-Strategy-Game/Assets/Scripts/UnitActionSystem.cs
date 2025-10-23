using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] LayerMask unitLayerMask;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Handle mouse click down
        if (Input.GetMouseButtonDown(0))
        {
            // Try handle unit selection
            if (TryHandleUnitSelection()) return;

            // If there is no unit selection, it means the moues click is for movement
            selectedUnit.GetMoveAction().Move(MouseWorld.GetPosition());
        }
    }

    /// <summary>
    /// Try select a unit when mouse click on it
    /// </summary>
    /// <returns></returns>
    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, unitLayerMask))
        {
            if(rayCastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Set the selected unit and fire OnSelectedUnit event
    /// </summary>
    /// <param name="unit"></param>
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Get selected unit
    /// </summary>
    /// <returns></returns>
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
