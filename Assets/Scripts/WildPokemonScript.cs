using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildPokemonScript : MonoBehaviour
{
    public int pokemonHealth = 20;
    public SpriteRenderer pkmnSR;
    public string pokeTexNumber = "0";
    public GameObject pkRender;
    public Sprite idleSprite;
    public GameObject player;
    public GameObject fightingGui;

    void Start(){
        player = GameObject.Find("player");
        fightingGui = GameObject.FindGameObjectWithTag("FightUI");
    }

    void endBattle(){
        player.GetComponent<PlayerScript>().battling = false;
        fightingGui.transform.Find("FightUI").gameObject.SetActive(false);
        Destroy(gameObject);
    }
    void sendDatatoPlr(){
        // TODO: Add customizable wild pokemon moves.
        StartCoroutine(player.GetComponent<PlayerScript>().getDataFromCaughtPokemon(pokeTexNumber,pokemonHealth.ToString(),"basicMove","Quick Attack","Spark","5")); 
    }
    public IEnumerator Caught(){
        //Happens when the Pokemon is currently being caught.
        // The code will be simplistic for now.

        sendDatatoPlr();
        endBattle();
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator Hurt()
    {
        Debug.Log("Owchie");
        
        yield return new WaitForSecondsRealtime(0.5f);

        if(pokemonHealth <= 0){
            player.GetComponent<PlayerScript>().battling = false;
            fightingGui.transform.Find("FightUI").gameObject.SetActive(false);
            Destroy(gameObject);
            StopCoroutine("Hurt");
        }
        
        pkmnSR.sprite = idleSprite;
        // Since it is pokemons turn to go it will do its move! It'll be a Simple attack that costs 5 health for now.
        // Array starting number is 0 ; Pokemon using variable is above that number
        Debug.Log(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing]);

        // This line below took me hours to write
        player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing] = (int.Parse(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing]) - 5).ToString();
        player.GetComponent<PlayerScript>().turn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
