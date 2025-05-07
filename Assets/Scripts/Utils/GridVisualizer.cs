using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridVisualizer : MonoBehaviour
{
  public bool showGrid = true;
  public bool showUnits = true;
  public Color gridColor = Color.green;
  [SerializeField] private DebugLabel labelPrefab;
  [SerializeField] private float cellSize = 2f;
  [SerializeField] private Vector2Int gridSize = new Vector2Int(50, 50);

  private Dictionary<Vector2Int, DebugLabel> activeLabels = new();

  public bool IsVisible()
  {
    return showGrid && showUnits;
  }

  void Start()
  {
    GenerateAllLabels();
    InvokeRepeating(nameof(UpdateLabels), 0f, 1f); // Update every 1 sec
  }

  public void ToggleVisualizationFromUI()
  {
    bool newState = !showGrid;
    ToggleVisualization(newState);
  }

  public void ToggleVisualization(bool enable)
  {
    showGrid = enable;
    showUnits = enable;

    foreach (DebugLabel label in activeLabels.Values)
    {
      label.gameObject.SetActive(enable);
    }
  }

  private void GenerateAllLabels()
  {
    for (int x = 0; x < gridSize.x; x++)
    {
      for (int y = 0; y < gridSize.y; y++)
      {
        Vector2Int coords = new Vector2Int(x, y);
        Vector3 pos = new Vector3(x * cellSize + cellSize / 2f, 0.01f, y * cellSize + cellSize / 2f);

        DebugLabel label = Instantiate(labelPrefab, pos, Quaternion.identity, this.transform);
        label.Init(coords, cellSize);
        label.UpdateText("0");
        activeLabels[coords] = label;

      }
    }

    InvokeRepeating(nameof(UpdateCellCounts), 0f, 1f);
  }

  void UpdateCellCounts()
  {
    var allCells = GridManager.UnitGrid.GetAllCellsWithUnits();

    foreach (var kvp in activeLabels)
    {
      Vector2Int coords = kvp.Key;
      DebugLabel label = kvp.Value;

      int count = allCells.ContainsKey(coords) ? allCells[coords].Count : 0;
      label.UpdateText(count.ToString());
    }
  }




  void Update()
  {
    if (Input.GetKeyDown(KeyCode.V))
      showGrid = !showGrid;

    if (!showGrid) return;

    DrawGrid();
  }

  void DrawGrid()
  {
    foreach (var cell in GridManager.UnitGrid.GetAllCellsWithUnits())
    {
      Vector3 worldPos = new Vector3(cell.Key.x * 2f + 1f, 0.1f, cell.Key.y * 2f + 1f); // Adjust based on cell size
      Debug.DrawLine(worldPos + Vector3.left, worldPos + Vector3.right, gridColor, 0.1f);
      Debug.DrawLine(worldPos + Vector3.forward, worldPos + Vector3.back, gridColor, 0.1f);
    }
  }

  void UpdateLabels()
  {
    if (!showUnits) return;

    var allCells = GridManager.UnitGrid.GetAllCellsWithUnits();

    foreach (var kvp in activeLabels)
    {
      Vector2Int coords = kvp.Key;
      DebugLabel label = kvp.Value;

      int count = allCells.ContainsKey(coords) ? allCells[coords].Count : 0;
      label.UpdateText($"Unit: {count}");
    }
  }


}
