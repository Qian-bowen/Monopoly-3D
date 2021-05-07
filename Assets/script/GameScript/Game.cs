using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using MapNS;
using GlobalsNS;
using CardNS;
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
    List<Character> char_group=new List<Character>();

    int dice_val=0;

    private void Awake() {
        //init prefab according to initial status receive
        
        //reset name to remove"(clone)"
        player_card=Resources.Load("Prefab/Ui/card") as GameObject;
        GameObject new_player_card=Instantiate(player_card,GameObject.Find("game").transform);
        new_player_card.name=player_card.name;

        //init prefab
        // sidebar=Resources.Load("Prefab/Ui/sidebarWrapper") as GameObject;
        // GameObject new_sidebar=Instantiate(sidebar,GameObject.Find("game").transform);
        // new_sidebar.name=sidebar.name;

        //init game map
        map= gameObject.AddComponent<Map>();
        map.instantiate_map();

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
        GameGlobals.socketWrapper.set_callback(this.get_socket_json);

        //test_write_card();

        load_player_card();
        //test function
        test_global_char_set();

        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //ch1.move_character(0,5);

    }



    void test_global_char_set()
    {
        GameGlobals.user_id=1;
        GameGlobals.room_id=1;
        GameGlobals.turn.turn_player_id=1;
        // PlayerInfo p1=new PlayerInfo(charType.CHAR1,0,0,0,100,100);
        // PlayerInfo p2=new PlayerInfo(charType.CHAR2,0,10,1,100,100);
        // PlayerInfo p3=new PlayerInfo(charType.CHAR3,15,0,3,100,100);
        // PlayerInfo p4=new PlayerInfo(charType.CHAR4,15,10,5,100,100);
        // List<PlayerInfo> pg=new List<PlayerInfo>{p1,p2,p3,p4};
        // GameGlobals.player_group=pg;

        PlayerMsg s1=new PlayerMsg(1,(int)charType.CHAR1,0,0,100,100);
        PlayerMsg s2=new PlayerMsg(2,(int)charType.CHAR2,0,10,100,100);
        PlayerMsg s3=new PlayerMsg(3,(int)charType.CHAR3,15,0,100,100);
        PlayerMsg s4=new PlayerMsg(4,(int)charType.CHAR4,15,10,100,100);
        List<PlayerMsg> playerMsgs=new List<PlayerMsg>{s1,s2,s3,s4};
        GameMsgRecJson recJson=new GameMsgRecJson(gameStatus.START,playerMsgs,"hello",0,0,0,1);

        game_reset_all_global(recJson);
        //handle game status such as win the game
        game_status_handler(recJson.game_status);
        //handle turn
        game_turn_handler(recJson.turn_player);  
        if(!char_group.Any())   
        {
            init_character();
        }
        
        //flush info on screen
        flush_global_info();
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
        foreach(PlayerInfo player in GameGlobals.player_group)
        {
            Character ch=GameObject.Find("game").AddComponent<Character>();
            int pos_x=player.pos_x;
            int pos_y=player.pos_y;
            ch.init_character(player.user_id,player.player_type,2f*(pos_x-1),0f,2f*(pos_y-1));
            char_group.Add(ch);
        }
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
                GameGlobals.socketWrapper.send_message(game_str);
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
        flush_map_block(0,1,0);     
        flush_map_block(0,1,1);  
        flush_map_block(0,1,2);  
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
        GameGlobals.socketWrapper.send_message(game_str);
        reset_game_globals();
    }

    //callback function, parse GameMsgRecJson
    private void get_socket_json()
    {
        Debug.Log("call back successfully");
        string get_json=GameGlobals.socketWrapper.get_received_json();
        GameMsgRecJson recJson=GameMsgRecJson.CreateFromJSON(get_json);
        //reset game globals
        game_reset_all_global(recJson);
        //handle game status such as win the game
        game_status_handler(recJson.game_status);
        //handle turn
        game_turn_handler(recJson.turn_player);  
        if(!char_group.Any())   
        {
            init_character();
        }
        
        //flush info on screen
        flush_global_info();
    }

    void game_reset_all_global(GameMsgRecJson recJson)
    {
        //set turn player id
        GameGlobals.turn.turn_player_id=recJson.turn_player;
        //set player info
        game_reset_player(recJson.player_group);
        //set palyer money
        foreach(PlayerInfo player in GameGlobals.player_group)
        {
            if(player.user_id==GameGlobals.user_id)
            {
                GameGlobals.player_money=player.cash;
                break;
            }
        }
        
        int update_map_x=recJson.map_x;
        int update_map_y=recJson.map_y;
        changeType change=(changeType)recJson.change;  
        game_change_handler(update_map_x,update_map_y,change);   
        
    }

    void roll_dice()
    {
        GameGlobals.turn.turn_roll_dice=true;
        SceneManager.LoadScene("Dice");       
    }

    private void OnEnable() {
        if(GameGlobals.turn.turn_roll_dice==true)
        {
            string info="roll: "+GameGlobals.turn.num;
            Debug.Log(info);
            show_info(info,1.5f);    
        }        
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
                        flush_map_block(block.pos_x,block.pos_y,(int)block.type+1);
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
                        flush_map_block(block.pos_x,block.pos_y,(int)block.type-1);
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
        //if player_group is empty
        if(!GameGlobals.player_group.Any())
        {
            foreach(PlayerMsg playerMsg in player_group)
            {
                PlayerInfo cur_player=new PlayerInfo();
                cur_player.player_type=(charType)playerMsg.player_type;
                cur_player.user_id=playerMsg.user_id;
                cur_player.pos_x=playerMsg.pos_x;
                cur_player.pos_y=playerMsg.pos_y;
                cur_player.cash=playerMsg.cash;
                cur_player.coupon=playerMsg.coupon;

                GameGlobals.player_group.Add(cur_player);
            }
            return;
        }
        //if not empty just modified
        foreach(PlayerMsg playerMsg in player_group)
        {
            foreach(PlayerInfo player in GameGlobals.player_group)
            {
                if(playerMsg.user_id==player.user_id)
                {
                    player.player_type=(charType)playerMsg.player_type;
                    player.pos_x=playerMsg.pos_x;
                    player.pos_y=playerMsg.pos_y;
                    player.cash=playerMsg.cash;
                    player.coupon=playerMsg.coupon;
                    player.cards=playerMsg.cards;
                }
            }
        }

        foreach(Character character in char_group)
        {
            foreach(PlayerInfo player in GameGlobals.player_group)
            {
                if(character.get_player_id()==player.user_id)
                {
                    character.move_character(player.pos_x,player.pos_y);
                }
            }
        }
        
    }

    void game_turn_handler(int cur_turn_id)
    {
        //if its my turn
        if(cur_turn_id==GameGlobals.user_id)
        {
            //show message window
            show_info("IT IS YOUR TURN",1.5f);
        }
    }

    void flush_map_block(int pos_x,int pos_y,int change_level)
    {
        //destroy previous block
        map.blockchanged(pos_x,pos_y,change_level);     
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
            case gameStatus.WIN:
            {
                show_info("YOU WIN",1.5f);
                break;
            }
            case gameStatus.START:
            {
                show_info("GAME START",1.5f);
                break;
            }
        }
    }

    void reset_game_globals()
    {
        GameGlobals.turn.turn_roll_dice=false;
        GameGlobals.turn.choice=false;
        GameGlobals.turn.num=-1;
    }

    void flush_global_info()
    {
        //flush money
        Debug.Log("Money:"+GameGlobals.player_money);
        GameObject.Find("Money").GetComponent<Text>().text="Money:"+GameGlobals.player_money;
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
