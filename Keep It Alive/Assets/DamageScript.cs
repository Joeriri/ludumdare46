using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{

    [SerializeField] private BoxCollider2D NormalCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon" && collision.gameObject.tag == "DamageCollider")
        {
            NormalCollider.offset = new Vector2(-5.960464e-08f, 0.007507861f);
            NormalCollider.size = new Vector2(1.340746f, 2.936532f);
            GetComponent<DestroyGO>().StartDestroyThisSpritedGO();
        }
    }
}
