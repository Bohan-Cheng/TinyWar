using System.Collections.Generic;
using UnityEngine;

public class UnitWaveSpawner : MonoBehaviour
{
  [Header("Spawner Settings")]
  [SerializeField] private GameObject unitPrefab;
  [SerializeField] private int unitCount = 10;
  [SerializeField] private float spawnRadius = 10f;
  [SerializeField] private float dontSpawnRadius = 2f;
  [SerializeField] private float spawnGap = 1.5f;
  [SerializeField] private Transform targetPoint;

  [Header("Optional Debug")]
  [SerializeField] private bool autoSpawnOnStart = true;

  private List<Vector3> usedPositions = new List<Vector3>();

  private void Start()
  {
    if (autoSpawnOnStart)
      SpawnUnits();
  }

  public void SpawnUnits()
  {
    int spawned = 0;
    int attempts = 0;

    while (spawned < unitCount && attempts < unitCount * 10)
    {
      Vector3 randomOffset = Random.insideUnitSphere;
      randomOffset.y = 0f;
      Vector3 spawnPos = transform.position + randomOffset.normalized * Random.Range(dontSpawnRadius, spawnRadius);

      if (IsPositionValid(spawnPos))
      {
        usedPositions.Add(spawnPos);
        GameObject unit = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        BaseUnit baseUnit = unit.GetComponent<BaseUnit>();

        if (baseUnit && !baseUnit.stats.isFriendly)
        {
          GameManager.Instance.RegisterEnemy();
        }

        if (baseUnit != null && targetPoint != null)
          baseUnit.Init(targetPoint);

        spawned++;
      }

      attempts++;
    }
  }

  private bool IsPositionValid(Vector3 newPos)
  {
    foreach (Vector3 pos in usedPositions)
    {
      if (Vector3.Distance(pos, newPos) < spawnGap)
        return false;
    }
    return true;
  }
}
