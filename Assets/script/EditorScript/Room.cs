using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using JsonNS;
using SocketMsgNS;
using GlobalsNS;

struct RoomInfo
{
    public int room_id;
    public int cur_person;
    public string founder_name;
    public RoomInfo(int id,int psn,string fn){room_id=id;cur_person=psn;founder_name=fn;}

}
public class Room : MonoBehaviour
{

    public GameObject line;
    Button refresh_btn;
    Button join_room;
    List<RoomInfo> room;
    int info_line=10;

    InputField input_room;


    bool valid_room=false;


    void Awake() {
        room=new List<RoomInfo>();
        refresh_btn=GameObject.Find("refresh_btn").GetComponent<Button>();
        refresh_btn.onClick.AddListener(refresh_room_info);

        join_room=GameObject.Find("join_room").GetComponent<Button>();
        join_room.onClick.AddListener(submit_room_info);

        input_room=GameObject.Find("input_room").GetComponent<InputField>();

        GameGlobals.socketWrapper.set_callback(this.get_socket_json);
        //GameGlobals.socketWrapper.start_server();
    }
    // Start is called before the first frame update
    void Start()
    {
        generate_room_info();
        init_info_table();
        //refresh_room_info();
    }

    // Update is called once per frame
    void Update()
    {
        if(valid_room)
        {
            change_scene();
        }
    }

    void generate_room_info()
    {
        room.Clear();
        for(int i=0;i<info_line;++i)
        {
            RoomInfo tmp=new RoomInfo(i,i,"founder"+i);
            room.Add(tmp);
        }
    }

    void init_info_table()
    {
        GameObject table = GameObject.Find("all_panel/room_panel/room_table");
        for (int i = 0; i < info_line; i++)
        {
            GameObject row = Instantiate(line, table.transform.position-i*line.transform.position, table.transform.rotation) as GameObject;
            RoomInfo info=room[i];
            row.name = "row" + i;
            row.transform.SetParent(table.transform);
            row.transform.localScale = Vector3.one;
        }
    }

    void refresh_room_info()
    {
        for(int i=0;i<info_line;++i)
        {
            string row_name="row"+i;
            GameObject row=GameObject.Find(row_name);
            RoomInfo info=room[i];
            row.transform.Find("col1").GetComponent<Text>().text = "id"+info.room_id;
            row.transform.Find("col2").GetComponent<Text>().text = "person"+info.cur_person;
            row.transform.Find("col3").GetComponent<Text>().text = info.founder_name;
        }
    }

    void submit_room_info()
    {
        string rid_str=input_room.text;
        Debug.Log("submit room:"+rid_str);
        int uid=GameGlobals.user_id;
        int rid;
        if(rid_str=="")
        {
            rid=0;
        }
        rid=Int32.Parse(rid_str);
        RoomJson roomJson=new RoomJson("match",uid,rid);
        string rj=Json.SaveToString(roomJson);
        GameGlobals.socketWrapper.send_message(rj);
    }

    private void get_socket_json()
    {
        Debug.Log("call back successfully");
        string get_json=GameGlobals.socketWrapper.get_received_json();
        RoomAuthJson ra=RoomAuthJson.CreateFromJSON(get_json);

        //if(ra.authentic==true&&ra.user_id==GameGlobals.user_id)
        if(ra.user_id==GameGlobals.user_id)
        {
            //set user_id
            GameGlobals.room_id=ra.room_id;
            Debug.Log("final receive str:"+ GameGlobals.room_id);
            valid_room=true;
        }
        else
        {
            //not handle successfully
        }   
    }

    private void change_scene()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnDestroy() {
        //socket.close_socket();
    }

}
