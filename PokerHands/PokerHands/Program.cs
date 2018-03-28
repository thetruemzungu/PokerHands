using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHands
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String HandAsString = Console.ReadLine();
                Array HandAsArray = HandAsString.Split(',');
                Hand Hand = new Hand(HandAsArray);

                string BestHand = Hand.DetermineBestHand();
                Console.WriteLine(BestHand);
                Console.ReadLine();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid Input");
                Console.ReadLine();
            }


            
        }
    }

    class Hand
    {
        List<Card> CardsInHand = new List<Card>();
        private List<Card> OrganizedCardsInHand = new List<Card>();

        public Hand(Array HandAsArray)
        {
            foreach (string item in HandAsArray)
            {
                CardsInHand.Add(new Card(item));
            }

            while (CardsInHand.Count != 0)
            {
                Card card = CardsInHand[0];
                foreach (Card cardInner in CardsInHand)
                {
                    if (card.getValueOfCard() < cardInner.getValueOfCard())
                    {
                        card = cardInner;
                    }
                }

                OrganizedCardsInHand.Add(card);
                CardsInHand.Remove(card);
            }
        }

        public String DetermineBestHand()
        {
            String BestHandPossible = "invalid input";
            bool flush = ItIsAFlush();
            bool straight = ItIsAStraight();
            string kindOfPair = ItIsAPair();
            if (flush && straight)
            {
                BestHandPossible = "straight flush";
            }
            else if (flush && !straight)
            {
                BestHandPossible = "Flush";
            }
            else if(kindOfPair.Equals("full house"))
            {
                BestHandPossible = "full house";
            }
            else if (straight && !flush)
            {
                BestHandPossible = "Straight";
            }
            else {
                BestHandPossible = ItIsAPair();
            }


            return BestHandPossible;
        }

        private bool ItIsAStraight()
        {
            bool result = true;
            Card currentCard = OrganizedCardsInHand.First();
            for (int i = 1; i < OrganizedCardsInHand.Count; i++)
            {
                if (result == true && currentCard.getValueOfCard() - OrganizedCardsInHand[i].getValueOfCard() == 1)
                {
                    result = true;
                    currentCard = OrganizedCardsInHand[i];
                }
                else if (result == true && OrganizedCardsInHand[i].rank == "A")
                {
                    if (OrganizedCardsInHand.First().getValueOfCard() - OrganizedCardsInHand[i].getValueOfCard() == 12)
                    {
                        result = true;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private String ItIsAPair()
        {
            String result = "high card";
            Dictionary<String, int> DetermineTypeOfKind = new Dictionary<string, int>();
            foreach (Card card in OrganizedCardsInHand) {
                if (!DetermineTypeOfKind.ContainsKey(card.rank))
                {
                    DetermineTypeOfKind.Add(card.rank, 1);
                }
                else
                {
                    DetermineTypeOfKind[card.rank]++;
                }
            }


            KeyValuePair<String, int> cardCombo = DetermineTypeOfKind.First();
            foreach (KeyValuePair<String, int> currentCard in DetermineTypeOfKind)
            {
                if (currentCard.Value > cardCombo.Value)
                {
                    cardCombo = currentCard;
                }
            }

            switch (cardCombo.Value)
            {
                case 4:
                    result = "four of a kind";
                    break;
                case 3:
                    if (DetermineTypeOfKind.ContainsValue(2))
                    {
                        result = "full house";
                    }
                    else
                    {
                        result = "three of a kind";
                    }
                    break;
                case 2:
                    result = "One Pair";
                    foreach (KeyValuePair<String, int> currentCard in DetermineTypeOfKind)
                    {
                        if (currentCard.Value.Equals(2) && !currentCard.Key.Equals(cardCombo.Key))
                        {
                            result = "two pair";
                        }
                    }
                    break;
            }

            return result;
        }

        private bool ItIsAFlush()
        {
            bool results = true;
            Card FirstCard = OrganizedCardsInHand.First();
            foreach (Card card in OrganizedCardsInHand)
            {
                if (FirstCard.suit != card.suit)
                {
                    results = false;
                }
            }

            return results;
        }
    }
    
    class Card {
        public string rank { get; }
        public string suit { get; }

        public Card(string StringRepresentationOfCard)
        {
            string[] acceptableRanks = { "2", "3", "4", "5", "6", "7", "8", "9", "J", "K", "Q", "A" };
            string[] acceptableSuits = {"h", "d", "s", "c"};

            if (acceptableRanks.Contains(StringRepresentationOfCard[0].ToString().ToUpper()))
            {
                rank = StringRepresentationOfCard[0].ToString().ToLower();
            }
            else
            {
                throw new ArgumentException("That is not a valid rank");
            }

            if (acceptableSuits.Contains(StringRepresentationOfCard[1].ToString().ToLower()))
            {
                suit = StringRepresentationOfCard[1].ToString().ToLower();
            }
            else
            {
                throw new ArgumentException("That is not a valid suit");
            }
        }

        public int getValueOfCard()
        {
            int value = 1;
            switch (this.rank)
            {
                case "2":
                    value = 2;
                    break;
                case "3":
                    value = 3;
                    break;
                case "4":
                    value = 4;
                    break;
                case "5":
                    value = 5;
                    break;
                case "6":
                    value = 6;
                    break;
                case "7":
                    value = 7;
                    break;
                case "8":
                    value = 8;
                    break;
                case "9":
                    value = 9;
                    break;
                case "T":
                    value = 10;
                    break;
                case "J":
                    value = 11;
                    break;
                case "Q":
                    value = 12;
                    break;
                case "K":
                    value = 13;
                    break;
            }
            return value;
        }
    }
}
