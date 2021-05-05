using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using MapNS;
using GlobalsNS;
using CardNS;
using SocketMsgNS;
using JsonNS;
using typeNS;
using BlockNS;
using CharacterNS;

//create 10*10 map
public class Game : MonoBehaviour
{
    private GameObject nextCameraPosition = null;
	private GameObject startCameraPosition = null;
    private float cameraMovementSpeed = 0.8F;
	private float cameraMovement = 0;

    private GameObject sidebar;
    private GameObject player_card;

    private Button roll_btn;
    private Button test_btn;
    private Button card_btn;
    private Button close_card_btn;
    private Button next_turn_btn;

    private Map map;
    SocketMsg socket;
    List<Character> char_group=new List<Character>();

    int dice_val=0;

    private void Awake() {
        //init prefab
        //reset name to remove"(clone)"
        player_card=Resources.Load("Prefab/Ui/card") as GameObject;
        GameObject new_player_card=Instantiate(player_card,GameObject.Find("game").transform);
        new_player_card.name=player_card.name;
        //init prefab
        sidebar=Resources.Load("Prefab/Ui/sidebarWrapper") as GameObject;
        GameObject new_sidebar=Instantiate(sidebar,GameObject.Find("game").transform);
        new_sidebar.name=sidebar.name;

        //init game map
        map= gameObject.AddComponent<Map>();
        map.instantiate_map();

        //init character
        init_character();

        //set message canvas invisible
        GameObject.Find("info").GetComponent<Canvas>().enabled=false;

        roll_btn = GameObject.Find("roll").GetComponent<Button>();
        roll_btn.onClick.AddListener(roll_dice);

        test_btn = GameObject.Find("test_button").GetComponent<Button>();
        test_btn.onClick.AddListener(test_button);

        next_turn_btn= GameObject.Find("next_turn_btn").GetComponent<Button>();
        next_turn_btn.onClick.AddListener(next_turn);

        card_btn = GameObject.Find("card_btn").GetComponent<Button>();
        card_btn.onClick.AddListener(show_player_card);

        close_card_btn = GameObject.Find("closeCard").GetComponent<Button>();
        close_card_btn.onClick.AddListener(close_card);

        //disable card canvas
        GameObject.Find("card").GetComponent<Canvas>().enabled=false;

        //disable some button
        if(GameGlobals.turn.turn_roll_dice==true)
        {
            GameObject.Find("roll").GetComponent<Button>().enabled=false;
        }

        //new socket
        socket=new SocketMsg();
        socket.set_callback(this.get_socket_json);

        test_write_card();

        load_player_card();

        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        //ch1.move_character(0,5);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void test_write_card()
    {
        Card c1=new Card(CardType.BUYBLOCK,2);
        Card c2=new Card(CardType.UPGRADE,1);
        Card c3=new Card(CardType.DEGRADE,5);
        GameGlobals.card_pool.Add(c1);
        GameGlobals.card_pool.Add(c2);
        GameGlobals.card_pool.Add(c3);
    }

    void init_character()
    {
        Character ch1=GameObject.Find("game").AddComponent<Character>();
        ch1.init_character(charType.CHAR1,-2f,0f,0f);
        char_group.Add(ch1);

        Character ch2=GameObject.Find("game").AddComponent<Character>();
        ch2.init_character(charType.CHAR2,-2f,0f,20f);
        char_group.Add(ch2);

        Character ch3=GameObject.Find("game").AddComponent<Character>();
        ch3.init_character(charType.CHAR3,30f,0f,0f);
        char_group.Add(ch3);

        Character ch4=GameObject.Find("game").AddComponent<Character>();
        ch4.init_character(charType.CHAR4,30f,0f,20f);
        char_group.Add(ch4);
    }

    void load_player_card()
    {
        int card_ptr=0;
        foreach(Card card in GameGlobals.card_pool)
        {
            string card_name=card.card_convert_resc();
            int card_num=card.get_card_num();

            Sprite card_test=Resources.Load<Sprite>("card/"+card_name);

            string card_wrapper_name="cardWrapper"+card_ptr.ToString();
            GameObject card_wrapper=GameObject.Find(card_wrapper_name);
            card_wrapper.GetComponent<Image>().sprite=card_test;

            //card add listener and set interactable
            card_wrapper.GetComponent<Button>().onClick.AddListener(delegate{use_card(card.get_card_type(),card_wrapper_name);});
            card_wrapper.GetComponent<Button>().interactable=true;

            GameObject number_tag=GameObject.Find(card_wrapper_name+"/cardNum");
            number_tag.GetComponent<Text>().text="剩余："+card.get_card_num().ToString();

            card_ptr++;
        }
    }

    void show_player_card()
    {
        GameObject.Find("card").GetComponent<Canvas>().enabled=true;     
    }

    void use_card(CardType ct,string wrapper_name)
    {
        //enable button after it click once
        GameObject used_card=GameObject.Find(wrapper_name);
        used_card.GetComponent<Button>().interactable=false;

        foreach(Card card in GameGlobals.card_pool)
        {
            if(card.get_card_type()==ct)
            {
                card.sub_card();
                //send message to server
                GameMsgSendJson game_json=new GameMsgSendJson(inputType.GIVE_CARD_NUM,true,(int)card.get_card_type());
                string game_str=Json.SaveToString(game_json);
                socket.send_message(game_str);
                break;
            }
        }  
    }

    void close_card()
    {
        GameObject.Find("card").GetComponent<Canvas>().enabled=false;
    }

    void test_button()
    {
        flush_map_block(0,1,BLOCK_TYPE.NONE);     
    }

    void next_turn()
    {
        if(GameGlobals.turn.turn_player_id!=GameGlobals.user_id)
        {
            show_info("NOT YOUR TURN",1.5f);
            return;
        }
        else if(GameGlobals.turn.turn_player_id==GameGlobals.user_id&&
        GameGlobals.turn.turn_roll_dice==false)
        {
            show_info("PLEASE ROLL DICE",1.5f);
            return;
        }

        //construct socket string
        //send the dice number rolled
        GameMsgSendJson game_json=new GameMsgSendJson(inputType.GIVE_DICE_NUM,true,GameGlobals.turn.num);
        string game_str=Json.SaveToString(game_json);
        socket.send_message(game_str);
        reset_game_globals();
    }

    //callback function, parse GameMsgRecJson
    private void get_socket_json()
    {
        Debug.Log("call back successfully");
        string get_json=socket.get_received_json();
        GameMsgRecJson recJson=GameMsgRecJson.CreateFromJSON(get_json);
        int update_map_x=recJson.map_x;
        int update_map_y=recJson.map_y;
        changeType change=(changeType)recJson.change;  
        game_change_handler(update_map_x,update_map_y,change);   
        game_reset_player(recJson.player_group);
    }

    void roll_dice()
    {
        GameGlobals.turn.turn_roll_dice=true;
        SceneManager.LoadScene("Dice");       
    }

    private void OnEnable() {
        dice_val  =  PlayerPrefs.GetInt("dice_val");
        //clear playerpref
        PlayerPrefs.DeleteAll();
        Debug.Log("back:"+dice_val);
        string info="go "+dice_val;
        show_info(info,1.5f);     
    }

    IEnumerator wait_value(float sec)
    {
        yield return new WaitForSeconds(sec);
        GameObject.Find("info").GetComponent<Canvas>().enabled=false;
    }

    void show_info(string info,float sec)
    {
        GameObject.Find("msg").GetComponent<Text>().text=info;
        GameObject.Find("info").GetComponent<Canvas>().enabled=true;
        StartCoroutine(wait_value(sec));
    }

    void game_change_handler(int map_x,int map_y,changeType ct)
    {
        switch(ct)
        {
            case changeType.LEVEL_UP:
            {
                foreach(BlockInfo block in GameGlobals.map)
                {
                    if(block.pos_x==map_x&&block.pos_y==map_y)
                    {
                        block.type=(BLOCK_TYPE)((int)block.type+1);
                        flush_map_block(block.pos_x,block.pos_y,block.type);
                    }
                }
                break;
            }
            case changeType.LEVEL_DOWN:
            {
                foreach(BlockInfo block in GameGlobals.map)
                {
                    if(block.pos_x==map_x&&block.pos_y==map_y)
                    {
                        block.type=(BLOCK_TYPE)((int)block.type-1);
                        flush_map_block(block.pos_x,block.pos_y,block.type);
                    }
                }
                break;
            }
            case changeType.NO_CHANGE:
            {
                break;
            }
        }
    }

    /*
    * move player position and map 
    */
    void game_reset_player(List<PlayerMsg> player_group)
    {
        foreach(PlayerMsg playerMsg in player_group)
        {
            foreach(PlayerInfo player in GameGlobals.player_group)
            {
                if(playerMsg.playernum==player.playernum)
                {
                    player.pos_x=playerMsg.pos_x;
                    player.pos_y=playerMsg.pos_y;
                    player.cash=playerMsg.cash;
                    player.coupon=playerMsg.coupon;
                    player.cards=playerMsg.cards;
                }
            }
        }
    }

    void flush_map_block(int pos_x,int pos_y,BLOCK_TYPE cur_type)
    {
        //destroy previous block
        string block_name="block"+pos_x.ToString()+","+pos_y.ToString();
        GameObject block2destroy=GameObject.Find(block_name);
        Destroy(block2destroy.gameObject);
        //init new block 
        Block block=gameObject.AddComponent<Block>();
        block.instantiate_block(cur_type,pos_x,pos_y);
        
    }

    void game_status_handler(gameStatus gs)
    {
        switch(gs)
        {
            case gameStatus.OUT:
            {
                show_info("GAME OVER",1.5f);
                break;
            }
        }
    }

    void reset_game_globals()
    {
        GameGlobals.turn.turn_roll_dice=false;
    }

    void flush_global_info()
    {
        //flush money
         GameObject.Find("msg").GetComponent<Text>().text="Money:"+GameGlobals.player_money;
    }

    void camera_reset()
    {
        float tmp=0;
        if (Input.GetKeyDown(KeyCode.L))
        {

        }

        if(Input.GetMouseButtonDown(0))
        {
            while(tmp<1)
            {
                Camera.main.transform.position = Vector3.Slerp(new Vector3(0,7,0),new Vector3(10,2,0) ,1);
                tmp+=0.1f;
            }
            
        }
    }
}
