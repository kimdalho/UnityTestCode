using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UniTaskTest : MonoBehaviour
{
    private CancellationTokenSource disableCancellation;
    private CancellationTokenSource AAACancellation;

    private void OnEnable()
    {
        if(disableCancellation != null)
        {
            disableCancellation.Dispose();
        }

        disableCancellation= new CancellationTokenSource();
    }


    private IEnumerator CoWait1Seconde()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("CoTest");
    }

    private async UniTaskVoid TaskWait1Seconde()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        Debug.Log("Task Test");
    }

    private void Start()
    {
        StartCoroutine(CoWait1Seconde());
        TaskWait1Seconde().Forget();
    }

}
