using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // Components
    private Vector3 startPos;

    // Others
    public float shakeTime = 0;
    public float shakePower;
    public float movmentRange; // Maximum excursion
    float remainingTime;
    bool canShake = false; 


    void Start()
    {
        remainingTime = shakeTime;
    }


    private void LateUpdate()
    {
        if ((remainingTime > 0) && (canShake))
        {
            remainingTime -= Time.deltaTime;
            float xAmount = Random.Range(-movmentRange, movmentRange) * shakePower * remainingTime;
            float yAmount = Random.Range(-movmentRange, movmentRange) * shakePower * remainingTime;

            transform.position += new Vector3(xAmount, yAmount, startPos.z);
        }
        else if (canShake)
        {
            transform.position = startPos;
            canShake = false;
        }
    }


    public void callShake()
    {
        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        canShake = true;
    }
}
