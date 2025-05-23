using System.Collections.Generic;
using UnityEngine;

public class AntUnit : BaseUnit
{
  [SerializeField] private float searchGridRadius = 0.5f;
  [SerializeField] private AudioSource walkingAudio;
  [SerializeField] private SelectionIndicator indicator;

  private Vector3 originalPosition;

  public override void Move(Vector3 targetPosition)
  {
    if (stats == null) return;

    Vector3 direction = targetPosition - transform.position;
    float distance = direction.magnitude;

    if (distance > 0.1f)
    {
      transform.Translate(direction.normalized * stats.moveSpeed * Time.deltaTime, Space.World);
    }

    // Initiate combat when in range of target
    if (target && Vector3.Distance(transform.position, target.position) < attackRange)
    {
      EngageCombat(target);
    }
  }

  protected override void OnMove()
  {
    if (!walkingAudio.isPlaying)
      walkingAudio.Play();

    indicator.ClearIndicators();

    if (target)
      indicator.StartTarget(target);
  }

  protected override void OnStop()
  {
    if (walkingAudio.isPlaying)
      walkingAudio.Stop();
  }

  public override void OnTargetDeath()
  {
    base.OnTargetDeath();
    indicator.ClearIndicators();
  }

  public void OnSelect()
  {
    indicator.StartSelect();
  }

  protected override void Awake()
  {
    base.Awake();
    originalPosition = transform.position;
  }

  protected override void Update()
  {
    base.Update();

    // Clear visual indicator if target is out of range
    if (currentTarget && Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
    {
      indicator.ClearIndicators();
    }

    // ant idle and look for nearby enemies
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

    // If no enemies found and in AI mode, return to original position of the ant
    if (GameManager.Instance.isAIMode && closest == null)
    {
      float returnDist = (transform.position - originalPosition).sqrMagnitude;
      if (returnDist > 0.01f)
      {
        MoveToPosition(originalPosition);
      }
    }

    return closest;
  }
}
