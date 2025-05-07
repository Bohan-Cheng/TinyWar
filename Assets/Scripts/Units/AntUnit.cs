using System.Collections.Generic;
using UnityEngine;

public class AntUnit : BaseUnit
{
  [SerializeField] private float searchGridRadius = 0.5f;

  private Vector3 originalPosition;

  public override void Move(Vector3 targetPosition)
  {
    if (stats == null) return;

    Vector3 direction = (targetPosition - transform.position).normalized;
    transform.Translate(direction * stats.moveSpeed * Time.deltaTime, Space.World);

    if (target && Vector3.Distance(transform.position, target.position) < attackRange)
    {
      EngageCombat(target);
    }
  }

  protected override void Awake()
  {
    base.Awake();
    originalPosition = transform.position;
  }

  protected override void Update()
  {
    base.Update();

    if (stats.isFriendly && !IsBusy())
    {
      BaseUnit nextTarget = FindClosestEnemy();
      if (nextTarget != null)
      {
        Init(nextTarget.transform);
        SetToMove();
      }
    }
  }


  private BaseUnit FindClosestEnemy()
  {
    List<BaseUnit> nearby = GridManager.UnitGrid.GetNearby(transform.position, searchGridRadius);

    BaseUnit closest = null;
    float closestDistSqr = float.MaxValue;

    foreach (BaseUnit unit in nearby)
    {
      if (unit == null || unit == this || unit.stats.isFriendly || unit.currentHealth <= 0)
        continue;

      float distSqr = (unit.transform.position - transform.position).sqrMagnitude;
      if (distSqr < closestDistSqr)
      {
        closest = unit;
        closestDistSqr = distSqr;
      }
    }

    if (GameManager.Instance.isAIMode)
    {
      // No enemies found — return to original position
      if (closest == null && Vector3.Distance(transform.position, originalPosition) > 0.1f)
      {
        MoveToPosition(originalPosition);
      }
    }

    return closest;
  }



}
