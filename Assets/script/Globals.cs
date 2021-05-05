using CardNS;
using System.Collections.Generic;
using typeNS;

namespace GlobalsNS
{
    public class PlayerInfo
    {
        public int playernum;
        public int pos_x;
        public int pos_y;
        public int cash;
        public int coupon;
        public charType character;
        public List<int> cards;
    }

    public class TurnAction
    {
        public int turn_player_id=-3;//whose turn to play the game
        public bool turn_roll_dice=false;
        public bool choice=false;
        public int num=0;
    }

    public class BlockInfo
    {
        public int pos_x;
        public int pos_y;
        public BLOCK_TYPE type;
    }

    static class GameGlobals
    {
        public static int user_id=-1;
        public static int room_id=-2;
        //turn infomation
        public static TurnAction turn=new TurnAction();
        public static int player_money=-4;
        public static List<Card> card_pool=new List<Card>();
        public static List<PlayerInfo> player_group=new List<PlayerInfo>();
        public static List<BlockInfo> map=new List<BlockInfo>();
    }
}