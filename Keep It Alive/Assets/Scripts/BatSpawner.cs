using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject batPrefab;
    [SerializeField] private int shots = 1;

    public void SpawnBat()
    {
        if (shots > 0)
        {
            Instantiate(batPrefab, transform.position, Quaternion.Euler(Vector3.zero));
            shots--;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
