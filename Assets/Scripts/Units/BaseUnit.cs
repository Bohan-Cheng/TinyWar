using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  [SerializeField] protected UnitStatsConfig stats;

  protected float currentHealth;
  protected Transform target;

  private float walkAnimTimer = 0f;
  private Vector3 originalScale;


  protected virtual void Awake()
  {
    if (stats != null)
      currentHealth = stats.maxHealth;

    originalScale = transform.localScale;
  }


  public void Init(Transform target)
  {
    this.target = target;
    LookAtTarget();
  }

  protected virtual void Update()
  {
    if (target != null && !stats.isFriendly)
    {
      Move(target.position);
      // Walking bounce effect
      walkAnimTimer += Time.deltaTime * 10f; // Speed of bounce
      float scaleY = 1f + Mathf.Sin(walkAnimTimer) * 0.05f; // 5% bounce
      transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleY, originalScale.z);
    }
    else
    {
      // Reset scale when idle
      transform.localScale = originalScale;
      walkAnimTimer = 0f;
    }
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
