using System;
using System.Collections.Generic;
using System.Text;

namespace CardTests.Models
{

    public class Deck
    {
        public bool success { get; set; }
        public Card[] cards { get; set; }
        public string deck_id { get; set; }
        public bool shuffled { get; set; }
        public int remaining { get; set; }
    }

    public class Card
    {
        public string image { get; set; }
        public string value { get; set; }
        public string suit { get; set; }
        public string code { get; set; }
    }

}
