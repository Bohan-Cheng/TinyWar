using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefenderCentralAIController : MonoBehaviour
{
  [Header("Detection Settings")]
  [SerializeField] private float detectionRadius = 10f;
  [SerializeField] private Transform flagPosition;
  [SerializeField] private float decisionInterval = 1f;

  private float decisionTimer = 0f;
  private List<BaseUnit> allDefenders = new List<BaseUnit>();

  private void Update()
  {
    if (!GameManager.Instance.isAIMode) return;

    decisionTimer += Time.deltaTime;
    if (decisionTimer >= decisionInterval)
    {
      decisionTimer = 0f;
      EvaluateThreatsAndAssignDefenders();
    }
  }

  public void RegisterDefender(BaseUnit defender)
  {
    if (defender != null && !allDefenders.Contains(defender))
    {
      allDefenders.Add(defender);
    }
  }

  private void EvaluateThreatsAndAssignDefenders()
  {
    // Get nearby enemy units within detection radius using grid system
    List<BaseUnit> enemies = GridManager.UnitGrid.GetNearby(flagPosition.position, detectionRadius)
        .Where(u => u != null && !u.stats.isFriendly && u.currentHealth > 0)
        .ToList();

    if (enemies.Count == 0) return;

    // Sort enemies by descending threat level
    enemies.Sort((a, b) => ThreatScore(b).CompareTo(ThreatScore(a)));

    HashSet<BaseUnit> assignedDefenders = new HashSet<BaseUnit>();

    foreach (BaseUnit enemy in enemies)
    {
      if (enemy == null || enemy.currentHealth <= 0) continue;

      // Determine required defenders (1 per 2 HP of enemy)
      // To distribute units by portion based on enemy health
      int defendersNeeded = Mathf.CeilToInt(enemy.currentHealth / 2f);
      int assigned = 0;

      // Set to target filtered enemy
      foreach (BaseUnit ant in allDefenders)
      {
        if (assigned >= defendersNeeded) break;
        if (ant == null || ant.currentHealth <= 0 || assignedDefenders.Contains(ant) || ant.isCombating) continue;

        ant.Init(enemy.transform);
        ant.SetToMove();
        assignedDefenders.Add(ant);
        assigned++;
      }
    }
  }

  // Calculates a composite threat score for prioritizing targets
  private float ThreatScore(BaseUnit enemy)
  {
    float distanceToFlag = Vector3.Distance(enemy.transform.position, flagPosition.position);
    float speedScore = enemy.stats.moveSpeed;
    float healthScore = enemy.currentHealth;
    float valueScore = enemy.stats.value;

    return (100f / (distanceToFlag + 1)) + speedScore + healthScore + valueScore;
  }
}
