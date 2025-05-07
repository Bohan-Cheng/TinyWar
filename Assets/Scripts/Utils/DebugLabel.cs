using TMPro;
using UnityEngine;

public class DebugLabel : MonoBehaviour
{
  [SerializeField] private Transform backgroundQuadParent;
  [SerializeField] private TextMeshPro text;

  public void Init(Vector2Int coords, float cellSize)
  {
    // Scale quad to match cell size
    if (backgroundQuadParent != null)
    {
      backgroundQuadParent.transform.localScale = new Vector3(cellSize, cellSize, cellSize);
    }

    // Optional: label the coordinates
    gameObject.name = $"DebugLabel_{coords.x}_{coords.y}";
  }

  public void UpdateText(string content)
  {
    if (text != null)
      text.text = content;
  }
}
