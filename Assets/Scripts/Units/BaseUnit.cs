using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
  public UnitStatsConfig stats;

  public float currentHealth;
  protected bool isMoving = false;
  public bool isCombating = false;
  protected Transform target;
  protected Vector3? moveToPosition = null;

  protected BaseUnit currentTarget;
  protected float attackCooldown = 1.0f;
  protected float attackTimer = 0f;
  public float attackRange = 1f;

  private float walkAnimTimer = 0f;
  private Vector3 originalScale;

  [Header("Animation Settings")]
  [SerializeField] private float animScale = 0.065f;
  [SerializeField] private float animSpeed = 10f;

  // Indicates whether the unit is currently busy with movement but not combat
  public bool IsBusy() => isMoving || currentTarget != null;

  protected virtual void Awake()
  {
    if (stats != null)
      currentHealth = stats.maxHealth;

    originalScale = transform.localScale;
  }

  public void SetToMove() => isMoving = true;

  public virtual void OnTargetDeath()
  {
    currentTarget = null;
    isMoving = !stats.isFriendly;
    isCombating = false;
    attackTimer = 0f;
  }

  public void Init(Transform target)
  {
    this.target = target;
    moveToPosition = null;

    if (!isMoving)
      OnMove();

    LookAtTarget();

    if (!stats.isFriendly)
      isMoving = true;
  }

  protected virtual void Update()
  {
    HandleMovement();
    HandleCombat();
  }

  // Handles movement logic toward a target or position
  private void HandleMovement()
  {
    if (isMoving)
    {
      if (target != null)
        Move(target.position);
      else if (moveToPosition.HasValue)
        Move(moveToPosition.Value);

      AnimateBounce();

      // Stop if close enough to the destination
      if (moveToPosition.HasValue && Vector3.Distance(transform.position, moveToPosition.Value) < 0.1f)
      {
        moveToPosition = null;
        isMoving = false;
        OnStop();
      }
    }
    else
    {
      // When no target or position set, stop moving
      isMoving = false;
      OnStop();
      transform.localScale = originalScale;
      walkAnimTimer = 0f;
    }
  }

  // Handles attacking logic if a target is assigned
  private void HandleCombat()
  {
    if (currentTarget == null) return;

    LookAtTarget();
    attackTimer += Time.deltaTime;

    // Stop if target out of range
    if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackRange)
    {
      currentTarget = null;
      isMoving = true;
      attackTimer = 0f;
      return;
    }

    // Apply damage if cooldown is ready
    if (attackTimer >= attackCooldown)
    {
      currentTarget.TakeDamage(stats.damage);
      attackTimer = 0f;
    }
  }

  private void AnimateBounce()
  {
    walkAnimTimer += Time.deltaTime * animSpeed;
    float scaleY = 1f + Mathf.Sin(walkAnimTimer) * animScale;
    transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleY, originalScale.z);
  }

  public void LookAtTarget()
  {
    if (target != null)
      LookAtPosition(target.position);
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
      Die();
  }

  protected virtual void Die()
  {
    currentTarget?.OnTargetDeath();

    if (!stats.isFriendly)
      GameManager.Instance?.OnEnemyDefeated(stats.value);

    Destroy(gameObject);
  }

  // Stop movement manually
  public virtual void Stop()
  {
    isMoving = false;
    target = null;
    moveToPosition = null;
    isCombating = false;
    attackTimer = 0f;

    OnStop();
  }

  public abstract void Move(Vector3 targetPosition);

  public void MoveToPosition(Vector3 position)
  {
    LookAtPosition(position);
    moveToPosition = position;

    if (!isMoving)
      OnMove();

    isMoving = true;
    target = null;
    isCombating = false;
  }

  public void ClearTarget()
  {
    target = null;
    moveToPosition = null;
    isMoving = false;
    isCombating = false;
  }

  public void EngageCombat(Transform target)
  {
    if (target == null || currentHealth <= 0) return;

    isCombating = true;
    isMoving = false;

    currentTarget = target.GetComponent<BaseUnit>();
    currentTarget?.ReceiveAggro(this);
  }

  public virtual void ReceiveAggro(BaseUnit attacker)
  {
    isMoving = false;
    currentTarget = attacker;
  }

  // Used by ants right now for indication display
  protected virtual void OnMove() { }
  protected virtual void OnStop() { }
}
