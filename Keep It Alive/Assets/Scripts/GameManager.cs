using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BodyPart bodyPartToDestroy;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug cheat om bodyparts te destroyen
        if (Input.GetKeyDown(KeyCode.Z))
        {
            bodyPartToDestroy.DestroyBodyPart();
        }
    }
    
    public void FrankDie()
    {
        Debug.Log("FRANK IS DOOD ;_;");
    }

    public void StartIntroPan()
    {
        Camera.main.GetComponent<Animator>().Play("IntroPan");
    }

    public void EndIntroPan()
    {
        Debug.Log("game is go!");
    }
}
