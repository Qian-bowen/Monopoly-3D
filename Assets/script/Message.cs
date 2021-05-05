using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using PlayerInfo_ns;

namespace PlayerInfo_ns
{
    public class PlayerInfo
    {
        public string username;
        public string password;

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }

        public PlayerInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }
    }

    public class GameInfo
     {
        public int dice_val;
        public int x,y,z;
        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}


public class message : MonoBehaviour
{
    Button send;
    Socket client;
    InputField username,password;
    private Thread receive_thread;
    private TcpClient conn;

    string server_ip="121.5.140.31";
    int server_port=5000;
    // Start is called before the first frame update
    void Start()
    {
        send=GameObject.Find("Send").GetComponent<Button>();
        username=GameObject.Find("username").GetComponent<InputField>();
        password=GameObject.Find("password").GetComponent<InputField>();

        send.onClick.AddListener(send_message);

        connect_server();
    }

    public void connect_server()
    {
        receive_thread=new Thread(new ThreadStart(listen_message));
        receive_thread.IsBackground=true;
        receive_thread.Start();
    }

    public void send_message()
    {
        string name=username.text;
        string pwd=password.text;

        PlayerInfo p = new PlayerInfo();
        p.username=name;
        p.password=pwd;
        string jsonstring=p.SaveToString();
        
        // conn.Send(Encoding.UTF8.GetBytes(jsonstring));
        Debug.Log("send");

        if (conn == null) {             
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = conn.GetStream(); 			
			if (stream.CanWrite) {                 				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(jsonstring); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                            
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}   
    }

    public void send_info(GameInfo info)
    {
        string jsonstring=info.SaveToString();
        client.Send(Encoding.UTF8.GetBytes(jsonstring));
        Debug.Log("send game info");
    }

    public void listen_message()
    {
        Debug.Log("listening");
        conn=new TcpClient(server_ip,server_port);
        Byte[] bytes = new Byte[1024];
        while(true)
        {
            string rec_json="";
            using(NetworkStream stream=conn.GetStream())
            {
                int len;
                
                while((len=stream.Read(bytes,0,bytes.Length))!=0)
                {
                    var incommingData = new byte[len]; 						
                    Array.Copy(bytes, 0, incommingData, 0, len); 											
                    string serverMessage = Encoding.ASCII.GetString(incommingData);	
                    rec_json += serverMessage;				
                    Debug.Log("server message received as: " + serverMessage);
                }
                Debug.Log("json : " + rec_json);
                PlayerInfo tmp=new PlayerInfo();
                PlayerInfo gi=tmp.CreateFromJSON(rec_json);
                Debug.Log("json decode: " + gi.username+" "+gi.password);
               
            }
        }
        
    }

    void OnDestroy() {
        client.Close();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
