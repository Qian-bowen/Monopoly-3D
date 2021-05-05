namespace CardNS
{
    /*
    BUYBLOCK:buy the block
    UPGRADE:upgrade house
    DEGRADE:degrade house
    STAY:stay in here for one turn
    */
    public enum CardType{BUYBLOCK,UPGRADE,DEGRADE,STAY};
    public class Card
    {
        CardType cardType;
        int cardNum;
        public Card(CardType ct,int cn)
        {
            cardType=ct;
            cardNum=cn;
        }
        public Card(){}

        public int get_card_num(){return cardNum;}
        public CardType get_card_type(){return cardType;}

        public void add_card(){cardNum++;}
        public bool sub_card()
        {
            if(cardNum>0)
            {
                cardNum--;
            }
            else
            {
                return false;
            }
            return true;
        }

        public string card_convert_resc()
        {
            switch(cardType)
            {
                case CardType.BUYBLOCK:
                return "buyblock";
                case CardType.UPGRADE:
                return "upgrade";
                case CardType.DEGRADE:
                return "degrade";
                case CardType.STAY:
                return "stay";
                default:
                return "";
            }
        }

        

    }
}