/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyPaper : MonoBehaviour
{
    bool isInCamera;
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(DestroyObj(other));
*//*        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if(Camera.main.transform.childCount == 1)
                isInCamera = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Camera.main.transform.childCount == 2)
                isInCamera = true;
        }

        if (!isInCamera)
        {
            PaperManager.Instance.StopInvoke();
            PaperManager.Instance.ResetBall();
        }*//*
    }

    IEnumerator DestroyObj(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Camera.main.transform.childCount == 1)
                isInCamera = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (Camera.main.transform.childCount == 2)
                isInCamera = true;
        }

        if (!isInCamera)
        {
            PaperManager.Instance.StopInvoke();
            PaperManager.Instance.ResetBall();
        }

        yield return new WaitForSecondsRealtime(1.0f);

        Destroy(other.gameObject);
    }
}
*/