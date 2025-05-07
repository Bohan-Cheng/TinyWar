using UnityEngine;

public class SelectionManager : MonoBehaviour
{
  private BaseUnit selectedUnit;

  [SerializeField] private LayerMask unitLayerMask;
  [SerializeField] private LayerMask groundLayerMask;
  [SerializeField] private AudioSource selectionAudio;
  [SerializeField] private AudioClip selectionClip;
  [SerializeField] private AudioClip onActionClip;
  [SerializeField] private float audioCooldown = 0.3f;

  private float nextAudioTime = 0f;

  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      HandleLeftClick();
    }
  }

  void HandleLeftClick()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out RaycastHit hit, 100f, unitLayerMask))
    {
      BaseUnit clickedUnit = hit.collider.GetComponent<BaseUnit>();

      if (clickedUnit != null)
      {
        if (clickedUnit.stats.isFriendly)
        {
          PlaySound(selectionClip, 0.35f);
          selectedUnit = clickedUnit;
          selectedUnit.Stop();
          AntUnit ant = (AntUnit)selectedUnit;
          if (ant)
          {
            ant.OnSelect();
          }
        }
        else if (selectedUnit != null && !clickedUnit.stats.isFriendly)
        {
          PlaySound(onActionClip, 1.0f);
          selectedUnit.Init(clickedUnit.transform);
          selectedUnit.SetToMove();
        }

        return;
      }
    }

    if (selectedUnit != null && Physics.Raycast(ray, out RaycastHit groundHit, 100f, groundLayerMask))
    {
      selectedUnit.ClearTarget();
      selectedUnit.MoveToPosition(groundHit.point);
    }

    PlaySound(onActionClip, 1.0f);
  }

  void PlaySound(AudioClip clip, float volume)
  {
    if (Time.time >= nextAudioTime)
    {
      selectionAudio.volume = volume;
      selectionAudio.PlayOneShot(clip);
      nextAudioTime = Time.time + audioCooldown;
    }
  }
}
