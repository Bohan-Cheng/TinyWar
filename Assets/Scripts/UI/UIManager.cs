using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  [Header("Refs")]
  [SerializeField] private GameObject menuPanel;
  [SerializeField] private Button menuButton;
  [SerializeField] private Button gridVisButton;
  [SerializeField] private Button toggleModeButton;
  [SerializeField] private TMP_Text modeButtonText;
  [SerializeField] private Button toggleMusicButton;
  [SerializeField] private TMP_Text musicButtonText;
  [SerializeField] private Button resumeButton;
  [SerializeField] private Button exitButton;
  [SerializeField] private TMP_Text scoreText;
  [SerializeField] private GameObject winScreen;
  [SerializeField] private GameObject loseScreen;
  [SerializeField] private Button playAgainButtonWin;
  [SerializeField] private Button playAgainButtonLose;
  [SerializeField] private GridVisualizer gridVisualizer;

  private bool isPaused = false;
  private bool isAIControlled = false;
  private bool isMusicOn = true;

  private void Awake()
  {
    menuButton.onClick.AddListener(OpenMenu);
    toggleModeButton.onClick.AddListener(ToggleControlMode);
    toggleMusicButton.onClick.AddListener(ToggleMusic);
    resumeButton.onClick.AddListener(ResumeGame);
    exitButton.onClick.AddListener(ExitGame);
    playAgainButtonWin.onClick.AddListener(ReloadScene);
    playAgainButtonLose.onClick.AddListener(ReloadScene);
    gridVisButton.onClick.AddListener(ToggleDebug);


    menuPanel.SetActive(false);
    menuButton.gameObject.SetActive(true);

    UpdateScore(0);
  }

  private void OnEnable()
  {
    var playerInput = GetComponent<PlayerInput>();
    if (playerInput != null)
    {
      playerInput.actions["Pause"].performed += OnPauseInput;
    }
  }

  private void OnDisable()
  {
    var playerInput = GetComponent<PlayerInput>();
    if (playerInput != null)
    {
      playerInput.actions["Pause"].performed -= OnPauseInput;
    }
  }

  public void ReloadScene()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  private void OnPauseInput(InputAction.CallbackContext context)
  {
    if (!isPaused)
      OpenMenu();
    else
      ResumeGame();
  }

  public void OpenMenu()
  {
    isPaused = true;
    Time.timeScale = 0f;
    menuPanel.SetActive(true);
    menuButton.gameObject.SetActive(false);
  }

  public void ResumeGame()
  {
    isPaused = false;
    Time.timeScale = 1f;
    menuPanel.SetActive(false);
    menuButton.gameObject.SetActive(true);
  }

  public void ShowWinScreen()
  {
    Time.timeScale = 0f;
    winScreen.SetActive(true);
    menuPanel.SetActive(false);
    menuButton.gameObject.SetActive(false);
  }

  public void ShowLoseScreen()
  {
    Time.timeScale = 0f;
    loseScreen.SetActive(true);
    menuPanel.SetActive(false);
    menuButton.gameObject.SetActive(false);
  }

  public void ToggleControlMode()
  {
    isAIControlled = !isAIControlled;

    GameManager.Instance.isAIMode = isAIControlled;

    modeButtonText.text = isAIControlled ? "Start Player Mode" : "Start AI Mode";
  }

  public void ToggleDebug()
  {
    gridVisualizer.ToggleVisualization(!gridVisualizer.IsVisible());
  }

  public void ToggleMusic()
  {
    isMusicOn = !isMusicOn;

    AudioListener.volume = isMusicOn ? 1f : 0f;

    musicButtonText.text = isMusicOn ? "Turn Music Off" : "Turn Music On";
  }

  public void ExitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
  }

  public void UpdateScore(int score)
  {
    scoreText.text = "Score: " + score;
  }
}
