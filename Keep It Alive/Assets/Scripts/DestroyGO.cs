using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGO : MonoBehaviour
{
    [SerializeField] private AnimationCurve spriteFadeCurve;
    [SerializeField] private float spriteFadeTiming;
    [SerializeField] private float idleTime;

    public SpriteRenderer[] sR;

    private Coroutine DestroyRoutine;

    private void Start()
    {
        sR = GetComponentsInChildren<SpriteRenderer>();
    }

    public void StartDestroyThisSpritedGO()
    {
        if (DestroyRoutine != null) { StopCoroutine(DestroyRoutine); }
        DestroyRoutine = StartCoroutine(DestroyThisSpritedGO());
    }

    private IEnumerator DestroyThisSpritedGO()
    {
        yield return new WaitForSeconds(idleTime);

        float lerpTime = 0;

        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime / spriteFadeTiming;
            float evaluatedLerpTime = spriteFadeCurve.Evaluate(lerpTime);

            for (int i = 0; i < sR.Length; i++)
            {
                sR[i].color = new Color(sR[i].color.r, sR[i].color.g, sR[i].color.b, evaluatedLerpTime);
            }

            yield return null;
        }

        Destroy(gameObject);
        yield return null;
    }
}
