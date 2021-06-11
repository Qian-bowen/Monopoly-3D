using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GlobalsNS;

namespace CardScriptNS{
    public class CardScript: MonoBehaviour
    {
        private Button card1,card2,card3;
        public void Awake() {
            card1 = GameObject.Find("cardWrapper0").GetComponent<Button>();
            card1.onClick.AddListener(card1_handler);
            card2 = GameObject.Find("cardWrapper1").GetComponent<Button>();
            card2.onClick.AddListener(card2_handler);
            card3 = GameObject.Find("cardWrapper2").GetComponent<Button>();
            card3.onClick.AddListener(card3_handler);
        }

        public void card1_handler()
        {
            GameGlobals.turn.num=0;
            GameGlobals.turn.action_idx=2;
            Debug.Log("card choose:"+GameGlobals.turn.num);
        }
        public void card2_handler()
        {
            GameGlobals.turn.num=1;
            GameGlobals.turn.action_idx=2;
            Debug.Log("card choose:"+GameGlobals.turn.num);
        }
        public void card3_handler()
        {
            GameGlobals.turn.num=2;
            GameGlobals.turn.action_idx=2;
            Debug.Log("card choose:"+GameGlobals.turn.num);
        }
    }

}
