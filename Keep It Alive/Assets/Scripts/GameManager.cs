using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool inMenu = true;
    private bool levelIsDone = false;

    [Header("Intro Pan")]
    [SerializeField] private AnimationClip introPanAnim;
    [SerializeField] private float windPitchLowered = 0.7f;

    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    private FadeScreen fadeScreen;

    [HideInInspector] public GameObject bodyPartClicked = null;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        levelIsDone = false;
        fadeScreen = FindObjectOfType<FadeScreen>();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            StartCoroutine(FadeInLevelRoutine());
        }
    }

    void Start()
    {
        AudioManager.instance.Play("Wind");
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

    public void stopCamera()
    {
        Camera.main.GetComponent<CameraMovement>().lockAllAxes = true;
    }

    public void ResumeCamera()
    {
        Camera.main.GetComponent<CameraMovement>().lockAllAxes = false;
    }

    public void StartIntroPan()
    {
        inMenu = false;
        Camera.main.GetComponent<Animator>().Play("IntroPan");
        StartCoroutine(IntroSoundPitchFade(1.0f, windPitchLowered, introPanAnim.length));
    }

    public void EndIntroPan()
    {
        Debug.Log("game is go!");
        Camera.main.GetComponent<CameraMovement>().enabled = true;
        BatBehaviour[] bats = FindObjectsOfType<BatBehaviour>();
        foreach (BatBehaviour bat in bats)
        {
            bat.enabled = true;
        }
    }

    public void GoToNextLevel()
    {
        if (!levelIsDone)
        {
            levelIsDone = true;
            StartCoroutine(GoToNextLevelRoutine());
        }
    }

    private IEnumerator FadeInLevelRoutine()
    {
        fadeScreen.StartFade(Color.black, Color.clear, fadeInDuration);
        yield return new WaitForSeconds(fadeInDuration);
    }

    private IEnumerator GoToNextLevelRoutine()
    {
        fadeScreen.StartFade(Color.clear, Color.black, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator IntroSoundPitchFade(float oldPitch, float newPitch, float duration)
    {
        float step = 1f / duration;
        for (float i = 0f; i < 1f; i += step * Time.deltaTime)
        {
            AudioManager.instance.ChangePitch("Wind", Mathf.Lerp(oldPitch, newPitch, i));
            yield return null;
        }
    }
}
