using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  public UnitStatsConfig stats;

  public float currentHealth;
  protected bool isMoving = false;
  protected Transform target;
  protected Vector3? moveToPosition = null;

  protected BaseUnit currentTarget;
  protected float attackCooldown = 1.0f;
  protected float attackTimer = 0f;
  public float attackRange = 1f;

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

  public virtual void OnTargetDeath()
  {
    if (currentTarget != null)
    {
      currentTarget = null;
      isMoving = !stats.isFriendly;
      attackTimer = 0f;
    }
  }

  public void Init(Transform target)
  {
    this.target = target;
    moveToPosition = null;
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

    if (currentTarget != null)
    {
      LookAtTarget();
      attackTimer += Time.deltaTime;

      if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
      {
        currentTarget = null;
        isMoving = true;
        attackTimer = 0f;
        return;
      }

      if (attackTimer >= attackCooldown)
      {
        currentTarget.TakeDamage(stats.damage);
        attackTimer = 0f;
      }
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
    if (currentTarget != null)
    {
      currentTarget.OnTargetDeath();
    }

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

  // Combat
  public void EngageCombat(Transform target)
  {
    if (target == null || currentHealth <= 0) return;

    isMoving = false;
    currentTarget = target.GetComponent<BaseUnit>();
    currentTarget.ReceiveAggro(this);
  }
  public virtual void ReceiveAggro(BaseUnit attacker)
  {
    isMoving = false;
    currentTarget = attacker;
  }
}
