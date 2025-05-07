using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }
  [SerializeField] private GameObject startMessage;

  public bool isAIMode = false;

  [Header("Dependencies")]
  [SerializeField] private UIManager uiManager;
  [SerializeField] private AudioSource deathAudio;

  [Header("Game State")]
  private int totalEnemies;
  private int defeatedEnemies;
  private int score;

  private bool gameEnded = false;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
  }

  private void Start()
  {
    StartCoroutine(DelayHideStartMessage());
  }

  IEnumerator DelayHideStartMessage()
  {
    yield return new WaitForSeconds(3);
    startMessage.SetActive(false);
  }

  public void RegisterEnemy()
  {
    totalEnemies++;
  }

  public void OnEnemyDefeated(int points = 1)
  {
    if (gameEnded) return;

    deathAudio.PlayOneShot(deathAudio.clip);

    defeatedEnemies++;
    score += points;

    uiManager.UpdateScore(score);

    if (defeatedEnemies >= totalEnemies)
    {
      WinGame();
    }
  }

  public void LoseGame()
  {
    if (gameEnded) return;

    gameEnded = true;
    uiManager.ShowLoseScreen();
  }

  private void WinGame()
  {
    gameEnded = true;
    uiManager.ShowWinScreen();
  }
}
