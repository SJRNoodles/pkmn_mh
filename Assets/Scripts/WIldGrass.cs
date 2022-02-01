using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIldGrass : MonoBehaviour
{
    private static float x;
    private static float y;
    private static float z;

    void Start(){
        x = transform.localScale.x;
        y = transform.localScale.y;
        z = transform.localScale.z;
    }
}
