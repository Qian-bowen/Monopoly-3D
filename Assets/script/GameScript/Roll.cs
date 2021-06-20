using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GlobalsNS;

namespace RollNS
{
    public class Roll : MonoBehaviour
    {
        private Button roll_btn;
        
        void Awake()
        {
            roll_btn=GameObject.Find("btn").GetComponent<Button>();
            roll_btn.onClick.AddListener(press_button);
        }


        public void press_button()
        {
            rolling();
            StartCoroutine(wait_value());
        }

        IEnumerator wait_value()
        {
            yield return new WaitForSeconds(3);
            int dice_val=Dice.Value("");
            GameGlobals.turn.num=dice_val;
            //GameGlobals.turn.num=6;
            SceneManager.LoadScene("Game");
        }

        public void rolling()
        {
            Dice.Clear();

            Dice.Roll("1d6", "d6-red", new Vector3(-1f,7f,0), new Vector3(0,0,0));
            Dice.Roll("1d6", "d6-red", new Vector3(1f,7f,0), new Vector3(0,0,0));
            
        }

    }

}
