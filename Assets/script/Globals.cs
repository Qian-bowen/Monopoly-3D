using CardNS;
using System.Collections.Generic;
using typeNS;
using SocketMsgNS;

namespace GlobalsNS
{
    public class PlayerInfo
    {
        public int playernum;
        public int user_id;
        public int pos_x;
        public int pos_y;
        public int cash;
        public int coupon;
        public List<int> cards;
        public PlayerInfo(){}
        public PlayerInfo(int pn,int ui,int px,int py,int ch,int cp)
        {
            playernum=pn;
            user_id=ui;
            pos_x=px;
            pos_y=py;
            cash=ch;
            coupon=cp;
        }
    }

    public class TurnAction
    {
        public int action_idx=0;//mode3==0,roll;mode3==1 make choice;mode3==2 use card
        public int turn_player_id=-3;//whose turn to play the game
        public bool turn_roll_dice=false;
        public bool choice=false;
        public int num=-1;
        //public int num=6;
    }

    public class BlockInfo
    {
        public int pos_x;
        public int pos_y;
        public BLOCK_TYPE type;
        public int playernum;
    }

    public static class GameGlobals
    {
        public static SocketMsg socketWrapper=new SocketMsg();
        public static int receive_time=0;
        public static int user_id=-1;
        public static int playernum=-2;
        public static int room_id=-3;
        public static bool is_out=false;
        //turn infomatio
        public static TurnAction turn=new TurnAction();
        public static int player_money=-4;
        public static List<Card> card_pool=new List<Card>();
        public static List<PlayerInfo> playergroup=new List<PlayerInfo>();
        public static List<List<BlockInfo>> map=new List<List<BlockInfo>>();
    }
}