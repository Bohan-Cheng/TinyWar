using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
  [Header("Indicator References")]
  [SerializeField] private GameObject selectionIndicator;
  [SerializeField] private GameObject targetIndicator;

  private Transform target;

  void Start()
  {
    if (selectionIndicator != null)
      selectionIndicator.SetActive(false);

    if (targetIndicator != null)
      targetIndicator.SetActive(false);
  }

  void Update()
  {
    if (target != null && targetIndicator != null)
    {
      Vector3 targetPos = target.position;
      Vector3 indicatorPos = targetIndicator.transform.position;

      targetIndicator.transform.position = new Vector3(
          targetPos.x,
          indicatorPos.y, // Keep original Y
          targetPos.z
      );
    }
    else
    {
      ClearTargetIndicator();
    }
  }


  public void StartSelect()
  {
    if (selectionIndicator != null)
      selectionIndicator.SetActive(true);
  }

  public void StartTarget(Transform targetTransform)
  {
    target = targetTransform;

    if (targetIndicator != null)
      targetIndicator.SetActive(true);
  }

  public void ClearIndicators()
  {
    target = null;

    if (selectionIndicator != null)
      selectionIndicator.SetActive(false);

    ClearTargetIndicator();
  }

  private void ClearTargetIndicator()
  {
    if (targetIndicator != null)
      targetIndicator.SetActive(false);
  }
}
