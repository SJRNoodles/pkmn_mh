using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    public Text missionText;
    private string parsedText;
    public List<string> missionList = new List<string> {"Catch a pokemon"};

    void Update(){
        parseText();
        missionText.text = parsedText;
    }

    void parseText(int i = 0){
        parsedText = "";
        while (i < missionList.Count){
            parsedText = parsedText + missionList[i] + "\n";
            i+=1;
        }
    }

    public void addMis(string thing){
        missionList.Add(thing);
    }

    public void completeMission(int missionId){
        missionList.RemoveAt(missionId);
    }

    public bool scanMisList(string item, int i = 0, bool retrievedItem = false, bool delFromInv = false){
        while (i < missionList.Count){
            if(missionList[i] == item){
                
                retrievedItem = true;
                if(delFromInv == true){
                    missionList.Remove(missionList[i]);
                }
            }
            i+=1;
        }
        return (retrievedItem);
    }
}
