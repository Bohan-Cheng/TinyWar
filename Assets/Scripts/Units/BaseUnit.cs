using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  [SerializeField] protected UnitStatsConfig stats;

  protected float currentHealth;

  protected virtual void Awake()
  {
    if (stats != null)
      currentHealth = stats.maxHealth;
  }

  public virtual void TakeDamage(float amount)
  {
    currentHealth -= amount;
    if (currentHealth <= 0)
    {
      Die();
    }
  }

  protected virtual void Die()
  {
    Destroy(gameObject);
  }

  public abstract void Move(Vector3 targetPosition);
}
