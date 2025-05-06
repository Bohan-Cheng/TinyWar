using UnityEngine;

public class SimpleGrass : MonoBehaviour
{
  private Quaternion originalRot;
  private Quaternion targetRot;
  private float speed;

  void Start()
  {
    originalRot = transform.rotation;
    SetNewTarget();
  }

  void Update()
  {
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * speed);
    if (Quaternion.Angle(transform.rotation, targetRot) < 0.1f)
      SetNewTarget();
  }

  void SetNewTarget()
  {
    float x = Random.Range(-5f, 5f);
    float z = Random.Range(-5f, 5f);
    targetRot = originalRot * Quaternion.Euler(x, 0, z);
    speed = Random.Range(0.5f, 1.5f);
  }
}
