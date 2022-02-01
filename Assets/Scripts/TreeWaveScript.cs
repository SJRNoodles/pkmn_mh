using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWaveScript : MonoBehaviour
{
    private float treeAnimation;
    void Update()
    {
        //Mathf.Sin(treeAnimation) * 5;
        treeAnimation+=1 * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(treeAnimation) * 2); // makes tree rock back and forth
    }
}
