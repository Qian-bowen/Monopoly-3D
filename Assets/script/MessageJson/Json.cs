using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using CardNS;
using typeNS;
using GlobalsNS;

namespace JsonNS
{
    public class Json
    {
        public static string SaveToString(Json js_type)
        {
            return JsonUtility.ToJson(js_type);
        }

        public static Json CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Json>(jsonString);
        }

        public Json(){}
    }

    public class LoginJson:Json
    {
        public string jsontype;
        public string username;
        public string password;
        public LoginJson(string usrname,string pwd)
        {
            jsontype="login";
            username=usrname;
            password=pwd;
        }

        public static LoginJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<LoginJson>(jsonString);
        }


    }

    public class AuthenticJson:Json
    {
        public string jsontype;
        public bool legal;
        public int user_id;
        public AuthenticJson(string jt,bool auth,int id)
        {
            jsontype=jt;
            legal=auth;
            user_id=id;
        }

        public static AuthenticJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<AuthenticJson>(jsonString);
        }
    }

    public class RoomJson:Json
    {
        public string jsontype;
        public int user_id;
        public int room_id;
        public RoomJson(string jt,int uid,int rid)
        {
            jsontype=jt;
            user_id=uid;
            room_id=rid;
        }

        public static RoomJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<RoomJson>(jsonString);
        }

    }

    public class RoomAuthJson:Json
    {
        public bool authentic;
        public int user_id;
        public int room_id;

        public static RoomAuthJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<RoomAuthJson>(jsonString);
        }

    }

    public class GameMsgSendJson:Json
    {
        public string jsontype;
        public inputType type;
        public int user_id;
        public bool choice;
        public int num;

        public static GameMsgSendJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<GameMsgSendJson>(jsonString);
        }
        public GameMsgSendJson(inputType t,bool ch,int n)
        {
            jsontype="inputmessage";
            type=t;
            user_id=GameGlobals.user_id;
            choice=ch;
            num=n;
        }
    }

    [Serializable]
    public class PlayerMessage:Json
    {
        public int playernum;//added
        public int user_id;
        public int pos_x;
        public int pos_y;
        public int cash;
        public int coupon;
        public List<int> cards;
        public static PlayerMessage CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerMessage>(jsonString);
        }
        public PlayerMessage(int pn,int id,int px,int py,int csh,int cp)
        {
            user_id=id;
            playernum=pn;
            pos_x=px;
            pos_y=py;
            cash=csh;
            cp=coupon;
        }
    }

    [Serializable]
    public class GameMsgRecJson:Json
    {
        public gameStatus game_status;
        public List<PlayerMessage> playergroup;
        public string information;
        public int map_x;
        public int map_y;
        public int change;  

        public int playernum;

        public static GameMsgRecJson CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<GameMsgRecJson>(jsonString);
        }

        public GameMsgRecJson(gameStatus gs,List<PlayerMessage> ps ,string info,int mx,int my,int cg,int tp)
        {
            game_status=gs;
            playergroup=ps;
            information=info;
            map_x=mx;
            map_y=my;
            change=cg;
            playernum=tp;
        }   
    }
}