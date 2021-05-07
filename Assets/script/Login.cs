using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SocketMsgNS;
using JsonNS;
using GlobalsNS;

namespace LoginNS
{
    public class Login : MonoBehaviour
    {
        Button send;
        Button server_btn;
        InputField username,password,ipaddress,ipport;

        private bool valid_user;

        // Start is called before the first frame update
        void Start()
        {
            //init valid_user
            valid_user=false;

            server_btn=GameObject.Find("ipset").GetComponent<Button>();
            ipaddress=GameObject.Find("ipaddress").GetComponent<InputField>();
            ipport=GameObject.Find("ipport").GetComponent<InputField>();
            server_btn.onClick.AddListener(set_server);

            send=GameObject.Find("Send").GetComponent<Button>();
            username=GameObject.Find("username").GetComponent<InputField>();
            password=GameObject.Find("password").GetComponent<InputField>();
            send.onClick.AddListener(send_login_json);
            
        }

        private void set_server()
        {
            string ip=ipaddress.text;
            int port=Int32.Parse(ipport.text);
            GameGlobals.socketWrapper.set_server(ip,port);

            //init socket message
            GameGlobals.socketWrapper.set_callback(this.get_socket_json);
            GameGlobals.socketWrapper.start_server();
        }

        private void send_login_json() {
            string name=username.text;
            string pwd=password.text;

            Json lg=new LoginJson(name,pwd);
            string lg_str=Json.SaveToString(lg);
            Debug.Log("lg_str:"+lg_str);
            GameGlobals.socketWrapper.send_message(lg_str);//$_ should be added
        }

        //callback function
        private void get_socket_json()
        {
            Debug.Log("call back successfully");
            string get_json=GameGlobals.socketWrapper.get_received_json();
            AuthenticJson au=AuthenticJson.CreateFromJSON(get_json);

            if(au.legal==true)
            {
                //set user_id
                GameGlobals.user_id=au.user_id;
                Debug.Log("final receive str:"+GameGlobals.user_id);
                valid_user=true;
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
            //socket.close_socket();
        }
    }

}
