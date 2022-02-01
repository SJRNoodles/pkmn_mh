using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScript : MonoBehaviour
{
    public GameObject player;
    public GameObject pokemon;
    public GameObject misList;
    public Canvas ui;
    public GameObject movesTab;
    public AudioSource selectionAudio;
    public AudioSource totemAudio;
    public ParticleSystem totemPart;

    public ParticleSystem waterType;
    public ParticleSystem normalType;
    
    public Camera cam;
    private int targetI;
    private string publicMoveUsing;

    public int pkmnChosen = 1;

    public Text Move1;
    public Text Move2;
    public Text Move3;

    public Text moveText;
    // Start is called before the first frame update
    public IEnumerator wait1s(){
        yield return new WaitForSecondsRealtime(1.0f); // Waits 1 seconds.
    }

    public IEnumerator screenShake(int i = 0){
        while (i<10) {
            cam.transform.position = new Vector3(cam.transform.position.x + Random.Range(-0.3f, 0.3f),cam.transform.position.y + Random.Range(-0.3f, 0.3f),cam.transform.position.z);
            yield return new WaitForSecondsRealtime(0.02f);
            i+=1;
        }
    }

    public IEnumerator diaTotem(){
        StartCoroutine(screenShake());
        moveText.enabled = true;
        moveText.text = "Totem ignited!!";
        yield return new WaitForSecondsRealtime(1.0f);
        moveText.enabled = false;

    }
    public IEnumerator diaThenAtk()
    {
        selectionAudio.Play();
        moveText.enabled = true;
        moveText.text = "Pokemon" + " has used " + publicMoveUsing;
        yield return new WaitForSecondsRealtime(1.0f);
        player.GetComponent<PlayerScript>().turn = 1;
        StartCoroutine(pokemon.GetComponent<WildPokemonScript>().Hurt());
        StartCoroutine(screenShake());
        moveText.enabled = false;
    }

    public void Catch(){
        pokemon = player.GetComponent<PlayerScript>().pokemonFighting;
        if (scanInventory("pokeball",0,false,true) == true) {
            // We can catch the Wild Pokemon, since we have Pokeballs.
            // Lets start a coruntine on the Wild Pokemon script so we can let it know it is currently being caught.
            if (misList.GetComponent<Missions>().scanMisList("Catch a pokemon")) {
                // Get the mission and delete it if its on the list.
                misList.GetComponent<Missions>().scanMisList("Catch a pokemon",0,false,true);
            }

            StartCoroutine(pokemon.GetComponent<WildPokemonScript>().Caught());
        }
    }

    // Update is called once per frame
    bool scanInventory(string item, int i = 0, bool retrievedItem = false, bool delFromInv = false){
        while (i < player.GetComponent<PlayerScript>().itemData.Count){
            Debug.Log(player.GetComponent<PlayerScript>().itemData[i]);
            if(player.GetComponent<PlayerScript>().itemData[i] == item){
                
                retrievedItem = true;
                targetI = i;
                if(delFromInv == true){
                    player.GetComponent<PlayerScript>().itemData.Remove(player.GetComponent<PlayerScript>().itemData[targetI]);
                }
                return (retrievedItem);
            }
            i+=1;
        }
        return (retrievedItem);
    }
    void Update()
    {
        if (int.Parse(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing]) <= 0) {
            
            if (scanInventory("totem",0,false,true)==true) {
                totemPart.Play();
                StartCoroutine("diaTotem");
                totemAudio.Play();
                player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing] = (20).ToString();
            }
            
        }
    }

    public void switchPokemon() {
        pkmnChosen += 1;
        if(pkmnChosen > player.GetComponent<PlayerScript>().pokemonData.Count / 6){
            pkmnChosen=1;
        }
        player.GetComponent<PlayerScript>().pokemonUsing = 6 * pkmnChosen - 5;
        player.GetComponent<PlayerScript>().turn = 1;
        pokemon = player.GetComponent<PlayerScript>().pokemonFighting;
        StartCoroutine(player.GetComponent<PlayerScript>().SummonPM());
        StartCoroutine(pokemon.GetComponent<WildPokemonScript>().Hurt());
    }

    public void Attack (string move) {
        // Now there would be a thingy for pokemon moves that would just check the array on a certain axis but nothing now
        if (player.GetComponent<PlayerScript>().turn == 0) {
            pokemon = player.GetComponent<PlayerScript>().pokemonFighting;
            StartCoroutine(screenShake());
            if (move == "basicMove") {
                normalType.Play();
                pokemon.GetComponent<WildPokemonScript>().pokemonHealth -= 3 * int.Parse(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 4]);
            }
            if (move == "Quick Attack") {
                normalType.Play();
                pokemon.GetComponent<WildPokemonScript>().pokemonHealth -= 5 * int.Parse(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 4]);
            }
            if (move == "Water Gun") {
                waterType.Play();
                pokemon.GetComponent<WildPokemonScript>().pokemonHealth -= 7 * int.Parse(player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 4]);
            }
            StartCoroutine("diaThenAtk");
        }
    }

    public void Move(int item){
        string pkmnMove = player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + item];
        publicMoveUsing = pkmnMove;
        Attack(pkmnMove);
        CloseTabs();
    }

    public void OpenTab(string tab) {
        if (tab == "moves"){
            movesTab.SetActive(true);
            Move1.text = player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 1];
            Move2.text = player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 2];
            Move3.text = player.GetComponent<PlayerScript>().pokemonData[player.GetComponent<PlayerScript>().pokemonUsing + 3];
        }
    }

    public void CloseTabs(){
        movesTab.SetActive(false);
    }

    public void Run () {
        player.GetComponent<PlayerScript>().battling = false;
        Debug.Log(player.GetComponent<PlayerScript>().battling);
        ui.gameObject.SetActive(false);
    }
}
