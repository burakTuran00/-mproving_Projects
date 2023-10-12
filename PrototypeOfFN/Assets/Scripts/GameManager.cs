using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Image fadeImage;

    public TextMeshProUGUI scoreText;

    private Blade blade;

    private Spawner spawner;

    private ShowItemToCut showItemToCut;

    private Timer timer;

    private int score = 0;

    [Header("Start Button")]

    public Button startButton;

    public TextMeshProUGUI startButtonText;

    public GameObject startButtonObject;

    public void StartButton()
    {
        spawner.StartSpawner();
        showItemToCut.TaskPanel.SetActive(false);
        timer.StartTimer();
        startButtonObject.SetActive(false);
    }

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
        showItemToCut = FindAnyObjectByType<ShowItemToCut>();
        timer = GetComponent<Timer>();
    }

    private void Start()
    {
        NewGame();
    }

    public void IncreaseScor(int point)
    {
        score += point;
        scoreText.text = score.ToString("00000");
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString("00000");

        ClearScene();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void Explode()
    {
        blade.enabled = false;
        //spawner.enabled = false;
        spawner.StopSpawner();

        StartCoroutine(ExplodeSequence());
    }

    IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        spawner.StartSpawner();
    }
}
