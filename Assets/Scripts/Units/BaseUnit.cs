using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  [SerializeField] protected UnitStatsConfig stats;

  protected float currentHealth;
  protected Transform target;

  protected virtual void Awake()
  {
    if (stats != null)
      currentHealth = stats.maxHealth;
  }

  public void Init(Transform target)
  {
    this.target = target;
    LookAtTarget();
  }

  public void LookAtTarget()
  {
    if (target == null) return;

    Vector3 direction = target.position - transform.position;
    direction.y = 0f; // Keep unit upright
    if (direction != Vector3.zero)
      transform.rotation = Quaternion.LookRotation(direction);
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
