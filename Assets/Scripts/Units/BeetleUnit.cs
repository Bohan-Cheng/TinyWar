using UnityEngine;

public class BeetleUnit : BaseUnit
{
  public override void Move(Vector3 targetPosition)
  {
    if (stats == null) return;

    Vector3 direction = (targetPosition - transform.position).normalized;
    transform.Translate(direction * stats.moveSpeed * Time.deltaTime, Space.World);
  }
}
