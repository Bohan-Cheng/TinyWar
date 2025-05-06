using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  public UnitStatsConfig stats;

  protected float currentHealth;
  protected bool isMoving = false;
  protected Transform target;
  protected Vector3? moveToPosition = null;


  private float walkAnimTimer = 0f;
  private Vector3 originalScale;


  protected virtual void Awake()
  {
    if (stats != null)
      currentHealth = stats.maxHealth;

    originalScale = transform.localScale;
  }

  public void SetToMove()
  {
    isMoving = true;
  }


  public void Init(Transform target)
  {
    this.target = target;
    LookAtTarget();
    if (!stats.isFriendly)
    {
      isMoving = true;
    }
  }

  protected virtual void Update()
  {
    if (target != null && isMoving)
    {
      Move(target.position);
      AnimateBounce();
    }
    else if (moveToPosition.HasValue && isMoving)
    {
      Move(moveToPosition.Value);
      AnimateBounce();

      if (Vector3.Distance(transform.position, moveToPosition.Value) < 0.1f)
      {
        moveToPosition = null;
        isMoving = false;
      }
    }
    else
    {
      transform.localScale = originalScale;
      walkAnimTimer = 0f;
    }
  }

  private void AnimateBounce()
  {
    walkAnimTimer += Time.deltaTime * 10f;
    float scaleY = 1f + Mathf.Sin(walkAnimTimer) * 0.05f;
    transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleY, originalScale.z);
  }

  public void LookAtTarget()
  {
    if (target == null) return;

    Vector3 direction = target.position - transform.position;
    direction.y = 0f;
    if (direction != Vector3.zero)
      transform.rotation = Quaternion.LookRotation(direction);
  }

  public void LookAtPosition(Vector3 position)
  {
    Vector3 direction = position - transform.position;
    direction.y = 0f;
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

  public void MoveToPosition(Vector3 position)
  {
    LookAtPosition(position);
    moveToPosition = position;
    isMoving = true;
    target = null;
  }

  public void ClearTarget()
  {
    target = null;
    moveToPosition = null;
    isMoving = false;
  }

}
