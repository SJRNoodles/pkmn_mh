using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private bool rolling = false;
    public float speed;
    public Camera camera;
    public Animator animator;
    public Animator effectAnimator;
    public bool battling;
    public AudioSource pokeNoise;
    public GameObject pokemonFighting;
    public GameObject fightingGui;
    public Canvas fightingGui2;
    public int pokemonUsing = 1;
    public GameObject misList;
    public GameObject myPokemon;
    public GameObject pokemonTextureDex;
    public SpriteRenderer mypkmnSR;
    public AudioSource musicPlaying;
    private bool musicPlayed = false;
    private Transform camTarget;
    public GameObject wildPokemonPrefab;
    public Text diaText;
    public Canvas dialogueBox;
    public GameObject spcToTalkText;
    public SpriteRenderer spriteRender;
    private bool reload = false;
    public bool talking = false;
    public int turn = 0; // 0 = You, 1 = Pokemon
    // Each pokemon in your party take up 6 array slots
    // View pokemon texture number by going to Poketexturedex in the scene
    // Structure: Pokemon Texture Number, Health, Move1 , Move2, Move3, Level
    public List<string> pokemonData = new List<string> {"0","20","basicMove","quickattack","spark","5"};
    // User items such as pokeballs and so and so.
    // TODO: show whole array as a gui in the battle menu.
    public new List<string> itemData = new List<string> {"pokeball","totem"};

    public IEnumerator getDataFromCaughtPokemon(string ptnm,string h,string m1,string m2,string m3,string l){
        // Since pokemon are 6 array slots, we will proceed to add the required 6 array slots to the Pokemon Data list. ez pz
        pokemonData.Add(ptnm);
        pokemonData.Add(h);
        pokemonData.Add(m1);
        pokemonData.Add(m2);
        pokemonData.Add(m3);
        pokemonData.Add(l);

        yield return new WaitForSeconds(0.1f);
    }

    void Start(){
        
        fightingGui = GameObject.FindGameObjectWithTag("FightUI");
    }

    public IEnumerator reloading(){
        yield return new WaitForSeconds(0.8f);
        reload = false;
        //battling=false;
    }

    void Update()
    {
        if (reload == true) {
            StartCoroutine(reloading());
        }
        spcToTalkText.transform.position = new Vector3(transform.position.x,transform.position.y + 2,transform.position.z);
        if (battling == false) {
            camTarget = transform;
            if (musicPlayed == false) {
                musicPlaying.Play();
                musicPlayed = true;
            }
            if (Input.GetKey(KeyCode.E)){
                if (reload == false) {
                    rolling=true;
                    animator.Play("roll");
                }
            }
            myPokemon.SetActive(false);
            if(rolling == false){
                if(Input.GetAxis("Horizontal")>0){
                    animator.Play("player_right");
                    transform.localScale = new Vector3(2f,2f,0f);
                }else{
                    if(Input.GetAxis("Horizontal")<0){
                        animator.Play("player_left");
                        transform.localScale = new Vector3(-2f,2f,0f);
                    }
                }
                if(Input.GetAxis("Horizontal") == 0){
                    if(Mathf.Abs(Input.GetAxis("Vertical"))>0){
                        animator.Play("player_right");
                    }
                }
                if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){
                    animator.Play("idle");
                }
            }
        }
       camera.transform.position -= new Vector3( ((camera.transform.position.x - camTarget.position.x)/.2f) * Time.deltaTime , ((camera.transform.position.y - camTarget.position.y)/.2f) * Time.deltaTime , 0); 
       camera.transform.position = new Vector3(camera.transform.position.x,camera.transform.position.y,transform.position.z-10); 
       if (battling == false) {
            pokeNoise.gameObject.SetActive(false);
            mypkmnSR.enabled = false;
            if(rolling==false){
                transform.position = new Vector3(transform.position.x+Input.GetAxis("Horizontal")*speed*Time.deltaTime,transform.position.y + Input.GetAxis("Vertical")*speed*Time.deltaTime,-1);
            }else{
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                    rolling=false;
                    reload = true;
                }
                transform.position = new Vector3(transform.position.x+Input.GetAxis("Horizontal")*speed*2*Time.deltaTime,transform.position.y + Input.GetAxis("Vertical")*speed*3*Time.deltaTime,-1);
            }
       }
    }

    public IEnumerator hardFixDiaStuck(int i = 0){
        while(i<50){
            battling = false;
            i+=1;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator DialogueEnter(string text, string message = "", int i = 0){
        Debug.Log("display dialogue");
        Debug.Log(text);
        dialogueBox.gameObject.SetActive(true);
        diaText.text = message;
        battling = true;
        while(i<text.Length)
        {
            message = message + text[i];
            diaText.text = message;
            i+=1;
            yield return new WaitForSecondsRealtime(0.03f);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        spcToTalkText.SetActive(false);
        battling=false;
        talking = false;
        reload=true;
        StartCoroutine(hardFixDiaStuck());
        dialogueBox.gameObject.SetActive(false);
    }

    public IEnumerator QuestEnter(string text, string quest, string message = "", int i = 0){
        Debug.Log("display dialogue");
        Debug.Log(text);
        dialogueBox.gameObject.SetActive(true);
        diaText.text = message;
        battling = true;
        while(i<text.Length)
        {
            message = message + text[i];
            diaText.text = message;
            i+=1;
            yield return new WaitForSecondsRealtime(0.03f);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        spcToTalkText.SetActive(false);
        battling=false;
        misList.GetComponent<Missions>().addMis(quest);
        StartCoroutine(hardFixDiaStuck());
        reload=true;
        talking = false;
        dialogueBox.gameObject.SetActive(false);
    }

    public IEnumerator SummonPM(){
        pokeNoise.gameObject.SetActive(false);
        mypkmnSR.enabled = false;
        effectAnimator.Play("poof");
        pokeNoise.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.7f);
        mypkmnSR.enabled = true;
        mypkmnSR.sprite = pokemonTextureDex.GetComponent<Poketexdex>().pokeDex[int.Parse(pokemonData[pokemonUsing-1])];
    }

    void StartWPBattle (GameObject pkmn) {
        battling = true;
        animator.Play("idle");
        pokemonFighting = pkmn;
        turn = 0;
        

        pkmn.transform.position = new Vector2(pkmn.transform.position.x + transform.localScale.x * 2 ,pkmn.transform.position.y);
        pkmn.transform.localScale = new Vector3(transform.localScale.x * -1,2,2);
        fightingGui.transform.Find("FightUI").gameObject.SetActive(true);

        Debug.Log(pokemonData[pokemonUsing-1]);

        StartCoroutine(SummonPM());
        myPokemon.SetActive(true);
        musicPlaying.Stop();
        musicPlayed = false;


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WildGrass")
        {
            collision.transform.position = new Vector2(collision.transform.position.x,collision.transform.position.y);
            collision.transform.localScale = new Vector3(3.2f,3.2f,0f);

            // Make a chance for a wild pokemon to show up
            if (Random.Range(-500.0f, 20.0f) > 1) {
                // Instantiate a pokemon on the player
                Debug.Log("Create pokemon");
                GameObject newPkmn = Instantiate(wildPokemonPrefab, transform.position, Quaternion.identity);

            }
        }
        if (collision.tag == "Pokemon"){
            // Battling code for wild pokemon
            StartWPBattle(collision.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D collision){
        if (collision.tag == "Dialogue"){
            spcToTalkText.SetActive(true);
            if(Input.GetKey(KeyCode.Space)){
                if (reload == false) {
                    if (talking == false) {
                        spcToTalkText.SetActive(false);
                        StartCoroutine(DialogueEnter(collision.gameObject.GetComponent<UnityEngine.UI.Text>().text)); 
                        reload = true;
                        talking = true;
                    }
                }
            }
        }
        if (collision.tag == "Quest"){
            spcToTalkText.SetActive(true);
            if(Input.GetKey(KeyCode.Space)){
                if (reload == false) {
                    if (talking == false) {
                        spcToTalkText.SetActive(false);
                        StartCoroutine(QuestEnter(collision.gameObject.GetComponent<UnityEngine.UI.Text>().text,collision.gameObject.GetComponent<NPCMissionScript>().quest)); 
                        reload = true;
                        talking = true;
                    }
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Dialogue"){
            spcToTalkText.SetActive(false);
            reload=true;
        }
        if (collision.tag == "Quest"){
            spcToTalkText.SetActive(false);
            reload=true;
        }
        if (collision.tag == "WildGrass")
        {
            collision.transform.position = new Vector2(collision.transform.position.x,collision.transform.position.y);
            collision.transform.localScale = new Vector3(3f,3f,0f);
        }
    }
}
