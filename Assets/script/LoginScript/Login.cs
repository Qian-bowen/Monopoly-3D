using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SocketMsgNS;
using JsonNS;
using GlobalsNS;

public class login : MonoBehaviour
{
    Button send;
    InputField username,password;
    SocketMsg socket;

    private bool valid_user;

    // Start is called before the first frame update
    void Start()
    {
        //init valid_user
        valid_user=false;

        send=GameObject.Find("Send").GetComponent<Button>();
        username=GameObject.Find("username").GetComponent<InputField>();
        password=GameObject.Find("password").GetComponent<InputField>();
        send.onClick.AddListener(send_login_json);
        //init socket message
        socket=new SocketMsg();
        socket.set_callback(this.get_socket_json);
        socket.start_server();
        //
        
    }

    private void send_login_json() {
        string name=username.text;
        string pwd=password.text;

        Json lg=new LoginJson(name,pwd);
        string lg_str=Json.SaveToString(lg);
        Debug.Log("lg_str:"+lg_str);
        socket.send_message(lg_str+"$_");//$_ should be added
    }

    //callback function
    private void get_socket_json()
    {
        Debug.Log("call back successfully");
        string get_json=socket.get_received_json();
        AuthenticJson au=AuthenticJson.CreateFromJSON(get_json);

        if(au.legal==true)
        {
            //set user_id
            GameGlobals.user_id=au.user_id;
            Debug.Log("final receive str:"+GameGlobals.user_id);
            valid_user=true;
            //close socket
            socket.close_socket();
        }     
    }

    void Update() {
        //if authorize , change scene
        if(valid_user)
        {
            change_scene();
        }

    }

    public void change_scene()
    {
        SceneManager.LoadScene("Editor");
    }

    void OnDestroy() {
        socket.close_socket();
    }
}
