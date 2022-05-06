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
    public GameObject pkmnRandomizer;
    public int dexNumb;
    private string specialMove = "Quick Attack";

    void Start(){
        player = GameObject.Find("player");
        fightingGui = GameObject.FindGameObjectWithTag("FightUI");
        pkmnRandomizer = GameObject.FindGameObjectWithTag("Pokedex");
        dexNumb = Random.Range(0, pkmnRandomizer.GetComponent<Poketexdex>().pokeDex.Length);
        idleSprite = pkmnRandomizer.GetComponent<Poketexdex>().pokeDex[dexNumb];
        pkRender.GetComponent<SpriteRenderer>().sprite = idleSprite;
    }

    void endBattle(){
        player.GetComponent<PlayerScript>().battling = false;
        fightingGui.transform.Find("FightUI").gameObject.SetActive(false);
        Destroy(gameObject);
    }
    void sendDatatoPlr(){
        // Not a  lot of moves programmed in so it's like this for now.
        if (pkmnRandomizer.GetComponent<Poketexdex>().pokeTypeDex[dexNumb] == "electric"){
            specialMove = "Spark";
        }
        if (pkmnRandomizer.GetComponent<Poketexdex>().pokeTypeDex[dexNumb] == "water"){
            specialMove = "Water Gun";
        }
        StartCoroutine(player.GetComponent<PlayerScript>().getDataFromCaughtPokemon(pokeTexNumber,pokemonHealth.ToString(),"basicMove","Quick Attack",specialMove,"5")); 
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
            gameObject.transform.Find("Render").GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(fightingGui.transform.Find("FightUI").GetComponent<FightScript>().winSaD("faint"));
            yield return new WaitForSecondsRealtime(5.0f);
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
