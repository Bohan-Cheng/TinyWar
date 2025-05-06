using UnityEngine;

public class SelectionManager : MonoBehaviour
{
  private BaseUnit selectedUnit;

  [SerializeField] private LayerMask unitLayerMask;
  [SerializeField] private LayerMask groundLayerMask;

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      HandleLeftClick();
    }
  }

  void HandleLeftClick()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out RaycastHit hit, 100f, unitLayerMask))
    {
      BaseUnit clickedUnit = hit.collider.GetComponent<BaseUnit>();

      if (clickedUnit != null)
      {
        if (clickedUnit.stats.isFriendly)
        {
          selectedUnit = clickedUnit;
          Debug.Log($"Selected unit: {selectedUnit.name}");
        }
        else if (selectedUnit != null && !clickedUnit.stats.isFriendly)
        {
          selectedUnit.Init(clickedUnit.transform);
          selectedUnit.SetToMove();
          Debug.Log($"Commanded {selectedUnit.name} to attack {clickedUnit.name}");
        }

        return;
      }
    }

    if (selectedUnit != null && Physics.Raycast(ray, out RaycastHit groundHit, 100f, groundLayerMask))
    {
      selectedUnit.ClearTarget();
      selectedUnit.MoveToPosition(groundHit.point);
      Debug.Log($"Commanded {selectedUnit.name} to move to {groundHit.point}");
    }
  }
}
