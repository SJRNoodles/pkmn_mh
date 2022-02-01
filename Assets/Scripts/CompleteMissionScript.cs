using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteMissionScript : MonoBehaviour
{
    public GameObject misList;
    public string requiredQuest;

    
    void OnCollisionEnter2D(Collision2D collision){
        if (misList.GetComponent<Missions>().scanMisList(requiredQuest)) {
            misList.GetComponent<Missions>().scanMisList(requiredQuest,0,false,true);
            Destroy(transform.gameObject);
        }
    }
}
