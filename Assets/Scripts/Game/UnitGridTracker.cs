using UnityEngine;

public class UnitGridTracker : MonoBehaviour
{
  private Vector3 lastPosition;
  private BaseUnit unit;

  private void Awake()
  {
    unit = GetComponent<BaseUnit>();
  }

  private void OnEnable()
  {
    Register();
  }

  private void OnDisable()
  {
    Unregister();
  }

  private void Update()
  {
    if ((transform.position - lastPosition).sqrMagnitude > 0.01f)
    {
      GridManager.UnitGrid.Move(unit, lastPosition, transform.position);
      lastPosition = transform.position;
    }
  }

  private void Register()
  {
    lastPosition = transform.position;
    GridManager.UnitGrid.RegisterUnit(transform.position, unit);
  }

  private void Unregister()
  {
    GridManager.UnitGrid.UnregisterUnit(unit);
  }
}
