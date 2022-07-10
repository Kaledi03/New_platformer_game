using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    // Components
    public Light lightComponent;

    // Others
    public float changeRange, waitTime;
    private float startRange;
    private bool canChange = true;
    

    void Start()
    {
        lightComponent = GetComponent<Light>();
        startRange = lightComponent.range;
    }


    void Update()
    {
        if (canChange)
        {
            lightComponent.range = startRange + Random.Range(-changeRange, changeRange);
            canChange = false;
            StartCoroutine("setTrue");
        }
    }

    public IEnumerator setTrue() 
    {
        yield return new WaitForSeconds(waitTime);
        canChange = true;
    }
}
