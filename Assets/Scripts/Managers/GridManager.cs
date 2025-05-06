using UnityEngine;

public class GridManager : MonoBehaviour
{
  public static Grid<BaseUnit> UnitGrid { get; private set; }

  [Header("Grid Settings")]
  [SerializeField] private float cellSize = 2f;
  [SerializeField] private Vector2Int gridDimensions = new Vector2Int(50, 50);
  [SerializeField] private bool visualizeGrid = true;

  private void Awake()
  {
    UnitGrid = new Grid<BaseUnit>(cellSize);
  }

  private void OnDrawGizmos()
  {
    if (!visualizeGrid) return;

    Gizmos.color = Color.gray;
    for (int x = 0; x < gridDimensions.x; x++)
    {
      for (int y = 0; y < gridDimensions.y; y++)
      {
        Vector3 cellCenter = new Vector3(x * cellSize + cellSize / 2f, 0, y * cellSize + cellSize / 2f);
        Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 0.1f, cellSize));
      }
    }
  }
}
