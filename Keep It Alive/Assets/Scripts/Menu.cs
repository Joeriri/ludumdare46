using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private float flickerTimer;
    [SerializeField] private int flickerAmn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        //startButton.gameObject.SetActive(false);
        StartCoroutine(StartFlicker());
        GameManager.Instance.StartIntroPan();
    }

    IEnumerator StartFlicker()
    {
        for (int i = 0; i < flickerAmn; i++)
        {
            startButton.gameObject.SetActive(false);
            yield return new WaitForSeconds(flickerTimer);
            startButton.gameObject.SetActive(true);
            yield return new WaitForSeconds(flickerTimer);
        }

        startButton.gameObject.SetActive(false);
        yield return null;
    }
}
