using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "TinyWar/Unit Stats", order = 0)]
public class UnitStatsConfig : ScriptableObject
{
  public string unitName;
  public bool isFriendly;
  public float moveSpeed;
  public float maxHealth;
  public float damage;
  public int value;
  public int flagRisk;
  public UnitType unitType; // Enum to identify unit kind
}

public enum UnitType
{
  Ant,
  Aphid,
  Ladybug,
  Beetle
}
