using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using JsonNS;

namespace SocketMsgNS
{
    public class SocketMsg
    {
        Socket client;
        Thread receive_thread;
        TcpClient conn;
        string server_ip="127.0.0.1";//121.5.140.31
        int server_port=5000;

        string get_json;

        public delegate void get_socket_json();
        public get_socket_json gsj_addr=null;
        public void set_callback(get_socket_json gsj)
        {
            this.gsj_addr=gsj;
        }

        public void start_server()
        {   
            receive_thread=new Thread(()=>listen_message());
            receive_thread.IsBackground=true;
            receive_thread.Start();
        }

        public string get_received_json()
        {
            Debug.Log("get rec json:"+get_json);
            return get_json;
        }

        public void close_socket() {
 
            client.Close();

        }

        public void send_message(string jsonstring)
        {
            
            Debug.Log("send:"+jsonstring);

            if (conn == null) {             
                return;         
            }  		
            try { 		
                Debug.Log("prepare send");	
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

        private void listen_message()
        {
            Debug.Log("listening");
            conn=new TcpClient(server_ip,server_port);
           
            while(true)
            {
                StringBuilder jsonstring=new StringBuilder();
                NetworkStream stream=conn.GetStream();
                if(stream.CanRead)
                {
                    Byte[] bytes = new Byte[1024];
                    int read_byte=0;
                    do{
                        read_byte=stream.Read(bytes,0,bytes.Length);
                        jsonstring.Append(Encoding.ASCII.GetString(bytes,0,read_byte));
                    }
                    while(stream.DataAvailable);
                    
                    get_json=jsonstring.ToString();
                    // get_json=Json.CreateFromJSON(jsonstring.ToString());
                    if(get_json!="")
                    {
                        Debug.Log("out:"+get_json);   
                        //use callback function
                        gsj_addr();
                    }
                    //clear get_json
                    get_json="";
                    
                }
            }
            
        }

    }
}
