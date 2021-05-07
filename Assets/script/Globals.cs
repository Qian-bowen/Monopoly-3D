using CardNS;
using System.Collections.Generic;
using typeNS;
using SocketMsgNS;

namespace GlobalsNS
{
    public class PlayerInfo
    {
        public charType player_type;
        public int user_id;
        public int pos_x;
        public int pos_y;
        public int cash;
        public int coupon;
        public List<int> cards;
        public PlayerInfo(){}
        public PlayerInfo(charType type,int ui,int px,int py,int ch,int cp)
        {
            player_type=type;
            user_id=ui;
            pos_x=px;
            pos_y=py;
            cash=ch;
            coupon=cp;
        }
    }

    public class TurnAction
    {
        public int turn_player_id=-3;//whose turn to play the game
        public bool turn_roll_dice=false;
        public bool choice=false;
        public int num=-1;
    }

    public class BlockInfo
    {
        public int pos_x;
        public int pos_y;
        public BLOCK_TYPE type;
    }

    static class GameGlobals
    {
        public static SocketMsg socketWrapper=new SocketMsg();
        public static int user_id=-1;
        public static int room_id=-2;
        //turn infomatio
        public static TurnAction turn=new TurnAction();
        public static int player_money=-4;
        public static List<Card> card_pool=new List<Card>();
        public static List<PlayerInfo> player_group=new List<PlayerInfo>();
        public static List<BlockInfo> map=new List<BlockInfo>();
    }
}