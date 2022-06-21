using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PaperManager : MonoBehaviour
{
    public static PaperManager Instance;
    [SerializeField] GameObject paperBall;
    public Vector2 sensivity = new Vector2(5f, 65f);
    public float resetBallAfterSeconds = 2f;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StopInvoke()
    {
        CancelInvoke();
    }

    public void ResetBallInvoke()
    {
        Invoke("ResetBall", resetBallAfterSeconds);
    }

    public void ResetBall()
    {
        Instantiate(paperBall);
    }
}
