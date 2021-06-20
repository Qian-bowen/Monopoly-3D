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
    private Button close_msg_btn;
    private Button next_turn_btn;
    private Button choice_yes,choice_no;

    private int use_the_card=-1;

    private Map map;
    private bool thread_init=false;
    private GameMsgRecJson recJson;


    int dice_val=0;

    private void Start() {
        //new socket
        GameGlobals.socketWrapper.set_callback(this.get_socket_json);  
        init_all();
    }

    void init_all()
    {
        //init prefab according to initial status receive
        
        //reset name to remove"(clone)"
        player_card=Resources.Load("Prefab/Ui/card") as GameObject;
        GameObject new_player_card=Instantiate(player_card,GameObject.Find("game").transform);
        new_player_card.name=player_card.name;

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

        choice_yes = GameObject.Find("yes").GetComponent<Button>();
        choice_no = GameObject.Find("no").GetComponent<Button>();

        close_msg_btn=GameObject.Find("close_msg_btn").GetComponent<Button>();
        close_msg_btn.onClick.AddListener(close_msg);

        //disable card canvas
        GameObject.Find("card").GetComponent<Canvas>().enabled=false;
        GameObject.Find("choose_panel").GetComponent<Canvas>().enabled=false;

        // test_write_card();
        // load_player_card();
        //init_character();
        //test function
        //test_global_char_set();
        //character_test();
        //show_info("hello world");
        //GameGlobals.playernum=1;
        //game_status_handler(gameStatus.PLAYER1_WIN);
    }

    void test_write_card()
    {
        Card c1=new Card(CardType.ADD_MONEY,2);
        Card c2=new Card(CardType.UPGRADE,1);
        Card c3=new Card(CardType.DEGRADE,5);
        GameGlobals.card_pool.Add(c1);
        GameGlobals.card_pool.Add(c2);
        GameGlobals.card_pool.Add(c3);
    }

    void character_test()
    {
        int pos_x=0;
        int pos_y=0;
        GameObject ch=init_single_character((charType)0,0,0f,0);
        ch.AddComponent<Character>();
        ch.GetComponent<Character>().set_character(1,ch,0,0);//modified remove
    }

    void init_character()
    {
        //check whether character is already exist
        if(GameObject.Find("character1")!=null)
            return;
        foreach(PlayerInfo player in GameGlobals.playergroup)
        {
            int pos_x=player.pos_x;
            int pos_y=player.pos_y;
            GameObject ch=init_single_character((charType)player.playernum-1,player.pos_x,0f,player.pos_y);
            ch.AddComponent<Character>();
            ch.GetComponent<Character>().set_character(player.user_id,ch,pos_x,pos_y);//modified remove
        }
    }


    private GameObject init_single_character(charType ctype,float x,float y,float z)
    {
        Vector3 init_pos=new Vector3(0f,0f,0f);
        if (x == 0) init_pos = new Vector3(-2f, 0f, 2 * z);
        if (x == 14) init_pos = new Vector3(30f, 0f, 2f * z);
        if (z == 0) init_pos = new Vector3(2 * x, 0f, -2f); 
        if (z == 9) init_pos = new Vector3(2 * x, 0f, 20f); 
        
        string char_path="";
        switch(ctype)
        {
            case charType.CHAR1:
            {
                char_path="Prefab/Character/character1";
                break;
            }
            case charType.CHAR2:
            {
                char_path="Prefab/Character/character2";
                break;
            }
            case charType.CHAR3:
            {
                char_path="Prefab/Character/character3";
                break;
            }
            case charType.CHAR4:
            {
                char_path="Prefab/Character/character4";
                break;
            }
            default:
            {
                char_path="Prefab/Character/character4";
                break;
            }

        }
        GameObject me;
        GameObject character;
        character=Resources.Load(char_path, typeof(GameObject)) as GameObject;       
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        me=Instantiate(character,init_pos,rot);
        me.transform.localScale=new Vector3(1.5f,1.5f,1.5f);
        me.name="character"+((int)ctype+1).ToString();
        return me;
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
                use_the_card=(int)card.get_card_type();
                break;
            }
        }  
    }
    

    void close_msg()
    {
        GameObject.Find("info").GetComponent<Canvas>().enabled=false;
    }

    void close_card()
    {
        GameObject.Find("card").GetComponent<Canvas>().enabled=false;
    }

    void test_button()
    {
        make_choice_handler();
    }

    void next_turn()
    {
        next_turn_btn.enabled=false;
        if(GameGlobals.turn.turn_player_id!=GameGlobals.user_id)
        {
            show_info("NOT YOUR TURN");
            next_turn_btn.enabled=true;
            return;
        }
        else if(GameGlobals.turn.turn_player_id==GameGlobals.user_id&&
        GameGlobals.turn.turn_roll_dice==false)
        {
            show_info("PLEASE ROLL DICE FIRST");
            next_turn_btn.enabled=true;
            return;
        }

        //construct socket string
        //send the dice number rolled
        int cur_turn_idx=GameGlobals.turn.action_idx;
        string game_str="";
        if(cur_turn_idx%3==0)
        {
            GameMsgSendJson tmp_game_json=new GameMsgSendJson(inputType.GIVE_DICE_NUM,true,GameGlobals.turn.num);
            game_str=Json.SaveToString(tmp_game_json);
            GameGlobals.turn.action_idx++;
        }
        //make choice
        else if(cur_turn_idx%3==1)
        {
            GameMsgSendJson tmp_game_json=new GameMsgSendJson(inputType.GIVE_CHOICE,GameGlobals.turn.choice,GameGlobals.turn.num);
            game_str=Json.SaveToString(tmp_game_json);
            GameGlobals.turn.action_idx++;
        }
        //choose card
        else if(cur_turn_idx%3==2)
        {
            GameMsgSendJson tmp_game_json=new GameMsgSendJson(inputType.GIVE_CARD_NUM,true,use_the_card);
            use_the_card=-1;
            game_str=Json.SaveToString(tmp_game_json);
            //one turn end and reset
            GameGlobals.turn.action_idx=0;
            reset_game_turn_globals();
        }
 
        GameGlobals.socketWrapper.send_message(game_str);
    }

    //callback function, parse GameMsgRecJson
    private void get_socket_json()
    {
        string get_json=GameGlobals.socketWrapper.get_received_json();
        recJson=GameMsgRecJson.CreateFromJSON(get_json);
        
        thread_init=true;
    }

    private void Update() {
        if(thread_init==true)
        {
            main_thread_init();
            thread_init=false;
        }
    }

    private void main_thread_init()
    {
        next_turn_btn.enabled=true;
        game_reset_all_global();
        //show rec message
        string rec_info=recJson.information;
        load_player_card();
        if(rec_info!="")
        {
            show_info(rec_info);
        }
        //handle turn
        game_turn_handler(recJson.playernum); 
        //flush info on screen
        flush_global_info();   
        //handle game status such as win the game
        game_status_handler(recJson.game_status); 
    }

    void log_rec_msg()
    {
        Debug.Log("game_status:"+(int)recJson.game_status);
        foreach(PlayerMessage player in recJson.playergroup)
        {
            Debug.Log("user_type(num):"+player.playernum+" pos_x:"+player.pos_x+" pos_y:"+player.pos_y);
        }
    }

    void game_reset_all_global()
    {
        log_rec_msg();
        //set turn player id
        GameGlobals.turn.turn_player_id=recJson.playernum;
        //set player info
        game_reset_player(recJson.playergroup);

        //set palyer money and cards
        foreach(PlayerInfo player in GameGlobals.playergroup)
        {
            if(player.user_id==GameGlobals.user_id)
            {
                GameGlobals.playernum=player.playernum;
                GameGlobals.player_money=player.cash;
                List<Card> cards=new List<Card>();
                
                foreach(int card_type in player.cards)
                {
                    if(card_type==0) continue;
                    Card find=cards.Find(x=>((int)x.get_card_type()==card_type));
                    if(find==null)
                    {
                        Card card=new Card((CardType)card_type,1);
                        cards.Add(card);
                    }
                    else
                    {
                        find.add_card();
                    }
                }
                GameGlobals.card_pool=cards;
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

    IEnumerator wait_for_second()
    {
        yield return new WaitForSeconds(4);
        GameObject.Find("info").GetComponent<Canvas>().enabled=false;
        
    }

    IEnumerator wait_end_for_second()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("Editor");
        
    }

    void show_info(string info)
    {
        GameObject.Find("msg").GetComponent<Text>().text=info;
        GameObject.Find("info").GetComponent<Canvas>().enabled=true;
        StartCoroutine(wait_for_second());
        
    }

    void show_result_info(string info)
    {
        GameObject.Find("msg").GetComponent<Text>().text=info;
        GameObject.Find("info").GetComponent<Canvas>().enabled=true;
        StartCoroutine(wait_end_for_second());
        
    }

    void game_change_handler(int map_x,int map_y,changeType ct)
    {
        switch(ct)
        {
            case changeType.LEVEL_UP:
            { 
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_UP); 
                break;
            }
            case changeType.LEVEL_DOWN:
            {
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_DOWN); 
                break;
            }
            case changeType.PLAYER_ONE_GET:
            {
                GameGlobals.map[map_x][map_y].playernum=1;
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_UP); 
                break;
            }
            case changeType.PLAYER_TWO_GET:
            {
                GameGlobals.map[map_x][map_y].playernum=2;
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_UP); 
                break;
            }
            case changeType.PLAYER_THREE_GET:
            {
                GameGlobals.map[map_x][map_y].playernum=3;
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_UP); 
                break;
            }
            case changeType.PLAYER_FOUR_GET:
            {
                GameGlobals.map[map_x][map_y].playernum=4;
                BlockInfo block=GameGlobals.map[map_x][map_y];
                map.blockchanged(block.pos_x,block.pos_y,changeType.LEVEL_UP); 
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
    void game_reset_player(List<PlayerMessage> playergroup)
    {
        //if playergroup is empty
        if(!GameGlobals.playergroup.Any())
        {
            foreach(PlayerMessage playerMsg in playergroup)
            {
                PlayerInfo cur_player=new PlayerInfo();
                cur_player.playernum=playerMsg.playernum;
                cur_player.user_id=playerMsg.user_id;
                cur_player.pos_x=playerMsg.pos_x;
                cur_player.pos_y=playerMsg.pos_y;
                cur_player.cash=playerMsg.cash;
                cur_player.coupon=playerMsg.coupon;
                cur_player.cards=playerMsg.cards;

                GameGlobals.playergroup.Add(cur_player);
            }
            //init player if not exist
            init_character();
            return;
        }

        //init player to original position
        init_character();
     
        //if not empty just modified
        foreach(PlayerMessage playerMsg in playergroup)
        {
            foreach(PlayerInfo player in GameGlobals.playergroup)
            {
                if(playerMsg.user_id==player.user_id)
                {
                    player.playernum=playerMsg.playernum;
                    player.pos_x=playerMsg.pos_x;
                    player.pos_y=playerMsg.pos_y;
                    player.cash=playerMsg.cash;
                    player.coupon=playerMsg.coupon;
                    player.cards=playerMsg.cards;
                }
            }
        }

        //move player
        foreach(PlayerInfo player in GameGlobals.playergroup)
        {
            string char_name="character"+((int)player.playernum);
            Character ch=GameObject.Find(char_name).GetComponent<Character>();
            ch.move_character(player.pos_x,player.pos_y);
        }   
    }

    void game_turn_handler(int cur_turn_id)
    {
        if(cur_turn_id!=GameGlobals.user_id)
            return;
        //roll dice 
        else if(GameGlobals.turn.action_idx%3==0)
        {
            //show message window
            show_info("IT IS YOUR TURN. ROLL DICE FIRST");
            roll_btn.enabled=true;
            card_btn.enabled=false;
            test_btn.enabled=false;
        }
        //make choice
        else if(GameGlobals.turn.action_idx%3==1)
        {
            show_info("WOULD YOU LIKE TO TAKE THE BLOCK?");
            roll_btn.enabled=false;
            card_btn.enabled=false;
            test_btn.enabled=true;
        }
        //choose card
        else if(GameGlobals.turn.action_idx%3==2)
        {
            //show message window
            show_info("YOU CAN CHOOSE CARD NOW");
            roll_btn.enabled=false;
            card_btn.enabled=true;
            test_btn.enabled=false;
        }
    }

    void make_choice_handler()
    {
        if(GameGlobals.turn.turn_player_id!=GameGlobals.user_id)
            return;
        GameObject.Find("choose_panel").GetComponent<Canvas>().enabled=true;
        choice_yes.onClick.AddListener(choice_yes_handler);
        choice_no.onClick.AddListener(choice_no_handler);
    }

    void choice_yes_handler()
    {
        GameGlobals.turn.choice=true;
        GameObject.Find("choose_panel").GetComponent<Canvas>().enabled=false;
        choice_yes.onClick.RemoveListener(choice_yes_handler);
        choice_no.onClick.RemoveListener(choice_no_handler);
    }

    void choice_no_handler()
    {
        GameGlobals.turn.choice=false;
        GameObject.Find("choose_panel").GetComponent<Canvas>().enabled=false;
        choice_yes.onClick.RemoveListener(choice_yes_handler);
        choice_no.onClick.RemoveListener(choice_no_handler);
    }

    void game_status_handler(gameStatus gs)
    {
        switch(gs)
        {
            case gameStatus.GAME_CONTINUE:
            {
                break;
            }
            case gameStatus.PLAYER1_OUT:
            {
                if(GameGlobals.playernum==1)
                {
                    show_info("YOU ARE OUT");
                    GameGlobals.is_out=true;
                }
                else 
                {
                    show_info("PLAYER 1 OUT");
                }
                GameObject.Find("character1").GetComponent<Character>().enabled=false;
                break;
            }
            case gameStatus.PLAYER2_OUT:
            {
                if(GameGlobals.playernum==2)
                {
                    show_info("YOU ARE OUT");
                    GameGlobals.is_out=true;
                }
                else 
                {
                    show_info("PLAYER 2 OUT");
                }
                GameObject.Find("character2").GetComponent<Character>().enabled=false;
                break;
            }
            case gameStatus.PLAYER3_OUT:
            {
                if(GameGlobals.playernum==3)
                {
                    show_info("YOU ARE OUT");
                    GameGlobals.is_out=true;
                }
                else 
                {
                    show_info("PLAYER 3 OUT");
                }
                GameObject.Find("character3").GetComponent<Character>().enabled=false;
                break;
            }
            case gameStatus.PLAYER4_OUT:
            {
                if(GameGlobals.playernum==4)
                {
                    show_info("YOU ARE OUT");
                    GameGlobals.is_out=true;
                }
                else 
                {
                    show_info("PLAYER 4 OUT");
                }
                GameObject.Find("character4").GetComponent<Character>().enabled=false;
                break;
            }
            case gameStatus.PLAYER1_WIN:
            {
                if(GameGlobals.playernum==1)
                {
                    show_result_info("CONGRATULATIONS! YOU WIN!");
                }
                else 
                {
                    show_result_info("PLAYER 1 WIN");
                }
                send_end();
                break;
            }
            case gameStatus.PLAYER2_WIN:
            {
                if(GameGlobals.playernum==2)
                {
                    show_result_info("CONGRATULATIONS! YOU WIN!");
                }
                else 
                {
                    show_result_info("PLAYER 2 WIN");
                }
                send_end();
                break;
            }
            case gameStatus.PLAYER3_WIN:
            {
                if(GameGlobals.playernum==3)
                {
                    show_result_info("CONGRATULATIONS! YOU WIN!");
                }
                else 
                {
                    show_result_info("PLAYER 3 WIN");
                }
                send_end();
                break;
            }
            case gameStatus.PLAYER4_WIN:
            {
                if(GameGlobals.playernum==4)
                {
                    show_result_info("CONGRATULATIONS! YOU WIN!");
                }
                else 
                {
                    show_result_info("PLAYER 4 WIN");
                }
                send_end();
                break;
            }
        }
    }

    void send_end()
    {
        string game_str="{\"jsontype\":\"gameover\"}";
        GameGlobals.socketWrapper.send_message(game_str);
    }

    void reset_game_turn_globals()
    {
        GameGlobals.turn.turn_roll_dice=false;
        GameGlobals.turn.choice=false;
        GameGlobals.turn.num=-1;
    }

    void flush_global_info()
    {
        if(GameGlobals.is_out)
        {
            GameObject.Find("Money").GetComponent<Text>().text="当前玩家:"+GameGlobals.turn.turn_player_id+" 我的ID:"+GameGlobals.user_id+" 您已出局";
        }
        else
        {
            GameObject.Find("Money").GetComponent<Text>().text="当前玩家:"+GameGlobals.turn.turn_player_id+" 我的ID:"+GameGlobals.user_id+" 金钱:" + GameGlobals.player_money;
        }
    }
}
