using UnityEngine;
using System.Collections.Generic;

public class GridDebugVisualizer : MonoBehaviour
{
  [SerializeField] private float radius = 5f;
  [SerializeField] private Color unitColor = Color.red;
  [SerializeField] private bool showGizmos = true;

  private void OnDrawGizmos()
  {
    if (!showGizmos || GridManager.UnitGrid == null)
      return;

    List<BaseUnit> nearby = GridManager.UnitGrid.GetNearby(transform.position, radius);

    Gizmos.color = unitColor;
    foreach (var unit in nearby)
    {
      if (unit != null)
      {
        Gizmos.DrawSphere(unit.transform.position + Vector3.up * 0.5f, 0.2f);
      }
    }

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
}
