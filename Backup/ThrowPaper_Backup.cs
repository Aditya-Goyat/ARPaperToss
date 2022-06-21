using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPaper : MonoBehaviour
{
    public float MaxObjectSpeed = 40f;
    public float flickSpeed = 0.4f;
    public string repsawnName = "";
    public float howClose = 9.5f;
    float startTime, endTime, swipeDistance, swipeTime;
    Vector2 startPos, endPos;
    float tempTime;
    float flickLength;
    float objectVelocity = 0, objectSpeed = 0;
    Vector3 angle;
    bool thrown, holding;
    Vector3 newPosition, velocity;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
    }

    void OnTouch()
    {
        Vector3 mousePos = Input.GetTouch(0).position;
        mousePos.z = Camera.main.nearClipPlane * howClose;
        newPosition = Camera.main.ScreenToViewportPoint(mousePos);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, newPosition, 80f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (holding)
        {
            OnTouch();
        }
        else if(thrown)
        {
            return;
        }
        
        if(Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if(_touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(_touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f))
                {
                    if (hit.transform == this.transform)
                    {
                        startTime = Time.time;
                        startPos = _touch.position;
                        holding = true;
                        transform.SetParent(null);
                    }
                }
            }
            else if(_touch.phase == TouchPhase.Ended && holding)
            {
                endTime = Time.time;
                endPos = _touch.position;
                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if(swipeTime < flickSpeed && swipeDistance > 100f)
                {
                    CalcSpeed();
                    MoveAngle();
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(angle.x * objectSpeed, angle.y * objectSpeed, angle.z * objectSpeed));
                    this.GetComponent<Rigidbody>().useGravity = true;
                    holding = false;
                    thrown = true;
                    Invoke("_Reset", 5f);
                }
                else
                {
                    _Reset();
                }
            }

            if (swipeTime > 0)
                tempTime = Time.time - startTime;
            if(tempTime > flickSpeed)
            {
                startTime = Time.time;
                startPos = _touch.position;
            }
        }
    }

    void _Reset()
    {
        Transform respawnPoint = GameObject.Find(repsawnName).transform;
        this.gameObject.transform.position = respawnPoint.position;
        this.gameObject.transform.rotation = respawnPoint.rotation;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().useGravity = false;
        thrown = holding = false;
    }

    void CalcSpeed()
    {
        flickLength = swipeDistance;
        if(swipeTime > 0)
        {
            objectVelocity = flickLength / (flickLength - swipeTime);
        }
        objectSpeed = objectVelocity * 50;
        objectSpeed = objectSpeed * -(objectSpeed - 1.7f);
        if(objectSpeed <= -MaxObjectSpeed)
        {
            objectSpeed = -MaxObjectSpeed;
        }
        swipeTime = 0;
    }

    void MoveAngle()
    {
        angle = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(endPos.y + 50f, (Camera.main.GetComponent<Camera>().nearClipPlane - howClose)));
    }
}
