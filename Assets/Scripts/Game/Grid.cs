using System.Collections.Generic;
using UnityEngine;

public class Grid<TCell>
{
  private readonly Dictionary<Vector2Int, List<TCell>> grid = new();
  private readonly float cellSize;

  public Grid(float cellSize)
  {
    this.cellSize = cellSize;
  }

  private Vector2Int GetCellCoords(Vector3 worldPosition)
  {
    return new Vector2Int(
        Mathf.FloorToInt(worldPosition.x / cellSize),
        Mathf.FloorToInt(worldPosition.z / cellSize)
    );
  }

  public void AddCell(int i, int j, TCell cell)
  {
    Vector2Int coords = new(i, j);
    if (!grid.ContainsKey(coords))
      grid[coords] = new List<TCell>();

    grid[coords].Add(cell);
  }

  public TCell GetCell(int i, int j)
  {
    Vector2Int coords = new(i, j);
    return grid.TryGetValue(coords, out var list) && list.Count > 0 ? list[0] : default;
  }

  public void DeleteCell(int i, int j)
  {
    grid.Remove(new Vector2Int(i, j));
  }

  public void DeleteCell(TCell cell)
  {
    foreach (var kvp in grid)
    {
      if (kvp.Value.Remove(cell))
        return;
    }
  }

  public void RegisterUnit(Vector3 position, TCell cell)
  {
    AddCell(GetCellCoords(position).x, GetCellCoords(position).y, cell);
  }

  public void UnregisterUnit(TCell cell)
  {
    DeleteCell(cell);
  }

  public void Move(TCell cell, Vector3 oldPos, Vector3 newPos)
  {
    Vector2Int oldCoords = GetCellCoords(oldPos);
    Vector2Int newCoords = GetCellCoords(newPos);

    if (oldCoords != newCoords)
    {
      if (grid.TryGetValue(oldCoords, out var list))
        list.Remove(cell);

      AddCell(newCoords.x, newCoords.y, cell);
    }
  }

  public List<TCell> GetNearby(Vector3 position, float radius)
  {
    List<TCell> result = new();
    Vector2Int center = GetCellCoords(position);
    int range = Mathf.CeilToInt(radius / cellSize);

    for (int x = -range; x <= range; x++)
    {
      for (int y = -range; y <= range; y++)
      {
        Vector2Int check = center + new Vector2Int(x, y);
        if (grid.TryGetValue(check, out var list))
        {
          result.AddRange(list);
        }
      }
    }

    return result;
  }
}
