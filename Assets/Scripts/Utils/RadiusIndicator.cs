using UnityEngine;

[ExecuteAlways]
public class RadiusIndicator : MonoBehaviour
{
  public float radius = 1f;
  public Color color = Color.green;

  void OnDrawGizmos()
  {
    Gizmos.color = color;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
}
