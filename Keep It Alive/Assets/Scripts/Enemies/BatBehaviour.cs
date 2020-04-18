using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBehaviour: MonoBehaviour
{
    [SerializeField] float batSpeed;
    [SerializeField] bool noiseOn;
    [SerializeField] Vector2 noiseResetRange;
    [SerializeField] Vector2 noiseTimeRange;
    private Transform player;
    private Vector3 direction = Vector3.zero;
    private Vector3 directionOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (noiseOn) { RandomOffset(); }
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.Normalize(Vector3.Normalize(player.position - transform.position) + directionOffset);
        transform.position += direction * batSpeed * Time.deltaTime;
    }

    void RandomOffset()
    {
        float randomTime = Random.Range(noiseResetRange.x, noiseResetRange.y);

        directionOffset = new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0);

        Invoke("CorrectAgain", randomTime);
    }

    void CorrectAgain()
    {
        float randomTime = Random.Range(noiseTimeRange.x, noiseTimeRange.y);

        directionOffset = Vector3.zero;

        Invoke("RandomOffset", randomTime);
    }

    public void WingSnapped()
    {
        gameObject.AddComponent<Rigidbody2D>();
        GetComponent<DestroyGO>().StartDestroyThisSpritedGO();
        BatWings[] batwings = GetComponentsInChildren<BatWings>();

        for (int i = 0; i < batwings.Length; i++)
        {
            batwings[i].animCancel = true;
        }

        Destroy(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //trigger damage ofzo (dit moet eigk gwn op de player trouwens)
        }
    }
}
