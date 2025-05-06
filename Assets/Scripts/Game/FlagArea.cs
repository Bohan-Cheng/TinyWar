using UnityEngine;

public class FlagArea : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    BaseUnit unit = other.GetComponent<BaseUnit>();
    if (unit == null) return;

    if (!unit.stats.isFriendly)
    {
      GameManager.Instance?.LoseGame();
    }
  }
}
