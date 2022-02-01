using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpin : MonoBehaviour
{
    public float spinner = 0;
    void Update()
    {
        spinner = spinner + 50 * Time.deltaTime;
        transform.rotation = Quaternion.Euler(spinner,spinner,0f);
    }
}
