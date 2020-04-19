using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool inMenu = true;

    [Header("Fade")]
    private FadeScreen fadeScreen;
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;

    [HideInInspector] public GameObject bodyPartClicked = null;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(this);
            }
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        fadeScreen = FindObjectOfType<FadeScreen>();
    }
    
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(FadeInLevelRoutine());
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !inMenu)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restarting level");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inMenu)
            {
                Application.Quit();
                Debug.Log("Quitting application");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Returning to menu");
            }
        }
    }
    
    public void FrankDie()
    {
        Debug.Log("FRANK IS DOOD ;_;");
    }

    public void StartIntroPan()
    {
        inMenu = false;
        Camera.main.GetComponent<Animator>().Play("IntroPan");
    }

    public void EndIntroPan()
    {
        Debug.Log("game is go!");
    }

    public void GoToNextLevel()
    {
        StartCoroutine(GoToNextLevelRoutine());
    }

    private IEnumerator FadeInLevelRoutine()
    {
        fadeScreen.StartFade(Color.clear, Color.black, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration);
    }

    private IEnumerator GoToNextLevelRoutine()
    {
        fadeScreen.StartFade(Color.clear, Color.black, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
