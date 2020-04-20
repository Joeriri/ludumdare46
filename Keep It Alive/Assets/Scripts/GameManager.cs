using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool inMenu = true;
    private bool levelIsDone = false;
    public bool hasAttachedFirstLimb = false;

    [Header("Intro Pan")]
    [SerializeField] private AnimationClip introPanAnim;
    [SerializeField] private float windPitchLowered = 0.7f;

    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    private FadeScreen fadeScreen;
    [SerializeField] private float showTextDuration = 3f;

    [HideInInspector] public GameObject bodyPartClicked = null;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;

        //if (Instance != null)
        //{
        //    if (Instance != this)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        //else
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(this);
        //}
    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log("OnSceneLoaded: " + scene.name);
    //    Debug.Log(mode);

    //    levelIsDone = false;
    //    fadeScreen = FindObjectOfType<FadeScreen>();

    //    if (SceneManager.GetActiveScene().buildIndex != 0)
    //    {
    //        StartCoroutine(FadeInLevelRoutine());
    //    }
    //}

    void Start()
    {
        levelIsDone = false;
        fadeScreen = FindObjectOfType<FadeScreen>();

        StartCoroutine(FadeInLevelRoutine());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inMenu)
            {
                Application.Quit();
                Debug.Log("Quitting application");
            }
            else
            {
                StartCoroutine(RestartLevelRoutine(false, false));
                Debug.Log("Returning to menu");
            }
        }
    }
    
    public void FrankDie()
    {
        StartCoroutine(RestartLevelRoutine(false, true));
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
        StartCoroutine(FadeSoundPitch("Wind", 1.0f, windPitchLowered, introPanAnim.length));
    }

    public void EndIntroPan()
    {
        Debug.Log("game is go!");
        Camera.main.GetComponent<CameraMovement>().enabled = true;
    }

    public void AttachedFirstLimb()
    {
        Debug.LogWarning("test 2");
        hasAttachedFirstLimb = true;
        AudioManager.instance.Play("Music");
    }

    public void EndGame()
    {
        StartCoroutine(RestartLevelRoutine(true, false));
        Debug.Log("It is finished. It is done.");
    }

    private IEnumerator FadeInLevelRoutine()
    {
        // screen
        fadeScreen.StartFade(Color.black, Color.clear, fadeInDuration);
        // wind
        AudioManager.instance.Play("Wind");
        AudioManager.instance.ChangePitch("Wind", 1.0f);
        StartCoroutine(FadeSoundVolume("Wind", 0.0f, 1.0f, fadeInDuration));

        yield return new WaitForSeconds(fadeInDuration);
    }

    private IEnumerator RestartLevelRoutine(bool win, bool die)
    {
        // screen
        fadeScreen.StartFade(Color.clear, Color.black, fadeOutDuration);
        // audio
        StartCoroutine(FadeSoundVolume("Music", 0.1f, 0.0f, fadeOutDuration));
        StartCoroutine(FadeSoundVolume("Wind", 1.0f, 0.0f, fadeOutDuration));

        yield return new WaitForSeconds(fadeOutDuration);

        if (win)
        {
            fadeScreen.ShowEndText(showTextDuration);
            yield return new WaitForSeconds(showTextDuration);
        }
        else if (die)
        {
            fadeScreen.ShowDieText(showTextDuration);
            yield return new WaitForSeconds(showTextDuration);
        }

        // reload and stop sounds
        AudioManager.instance.Stop("Music");
        AudioManager.instance.Stop("Wind");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeSoundPitch(string name, float oldPitch, float newPitch, float duration)
    {
        float step = 1f / duration;
        for (float i = 0f; i < 1f; i += step * Time.deltaTime)
        {
            AudioManager.instance.ChangePitch(name, Mathf.Lerp(oldPitch, newPitch, i));
            yield return null;
        }
    }

    private IEnumerator FadeSoundVolume(string name, float oldVolume, float newVolume, float duration)
    {
        float step = 1f / duration;
        for (float i = 0f; i < 1f; i += step * Time.deltaTime)
        {
            AudioManager.instance.ChangeVolume(name, Mathf.Lerp(oldVolume, newVolume, i));
            yield return null;
        }
    }
}
