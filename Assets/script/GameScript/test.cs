using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    private Rect rect;
    // Start is called before the first frame update
    private string die_type = "d6-red";
	private GameObject dice = null;
    
    void Start()
    {
        //dice = Dice.prefab("d6",new Vector3(0, 5F, 0),new Vector3(1.4f,1.4f,1.4f) , new Vector3(1.4f,1.4f,1.4f),die_type);
        //dice.SetActive(true);
    }

    private GameObject spawnPoint = null;

    // Update is called once per frame
    void Update()
    {
       
    }

    public void press_button()
    {
        rolling();
        StartCoroutine(wait_value());
    }

    IEnumerator wait_value()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        int dice_val=Dice.Value("");
        PlayerPrefs.SetInt("dice_val", dice_val);
        Debug.Log("num: "+dice_val);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        SceneManager.LoadScene("Game");
    }

    private void OnDisable() 
    {
        int dice_val=Dice.Value("");
        PlayerPrefs.SetInt("dice_val", dice_val);
    }

    public void rolling()
    {
        Dice.Clear();

        Dice.Roll("1d6", "d6-red", new Vector3(-1f,7f,0), new Vector3(0,0,0));
        Dice.Roll("1d6", "d6-red", new Vector3(1f,7f,0), new Vector3(0,0,0));
        
    }

    void OnGUI() {
        if (Dice.Count("")>0)
        {

            // we have rolling dice so display rolling status
            GUI.Box(new Rect( 10 , Screen.height - 75 , Screen.width - 20 , 30), "");
            GUI.Label(new Rect(20, Screen.height - 70, Screen.width, 20), Dice.AsString(""));
        }
    }
}
