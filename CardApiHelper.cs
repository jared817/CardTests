using CardTests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace CardTests
{
    class CardApiHelper
    {

        public static string url = "https://deckofcardsapi.com/api/deck/";

        public static Deck GetNewDeck(int numberOfDecks, bool withJokers)
        {
            var client = new RestClient(url);
            var request = new RestRequest("new", DataFormat.Json);
            var response = new RestResponse();

            if (numberOfDecks > 1)
            {
                request.AddParameter("deck_count", numberOfDecks);
            }

            if (withJokers)
            {

                request.AddParameter("jokers_enabled", true);
                response = (RestResponse)client.Post(request);
            }

            else
            {
                response = (RestResponse)client.Get(request);
            }

            return JsonConvert.DeserializeObject<Deck>(response.Content);
        }

        public static Deck DrawCards(string deckId, int count)
        {
            var client = new RestClient(url);
            var request = new RestRequest($"{deckId}/draw/?count={count}", DataFormat.Json);
            var response = client.Get(request);

            return JsonConvert.DeserializeObject<Deck>(response.Content);
        }

        public static bool VerifyDeckIsComplete(string deckId, int numberOfDecks, bool hasJokers)
        {
            var cardSuite = new List<string>() { "C", "D", "H", "S" };
            var cardValue = new List<string>() { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };
            var fullCardSet = new List<string>();
            var hashCards = new HashSet<string>();
            var deckSize = 52;

            // Create Full Card Set for verification
            foreach (var suite in cardSuite)
            {
                foreach (var value in cardValue)
                {
                    fullCardSet.Add($"{value}{suite}");
                }
                
            }


            // Add jokers if set
            if (hasJokers)
            {
                fullCardSet.Add("X1");
                fullCardSet.Add("X2");
                deckSize += 2;
            }

            // Extract all cards from the deck
            var cardDeck = DrawCards(deckId, numberOfDecks*deckSize);
            
            cardDeck.success.Should().BeTrue();
            cardDeck.deck_id.Should().Be(deckId);
            cardDeck.remaining.Should().Be(0);

            // Check that the new deck has expected cards
            foreach (var card in cardDeck.cards)
            {
                if (!fullCardSet.Contains(card.code))
                {
                   return false;
                }

                // Get Unique Cards
                hashCards.Add(card.code);
                
            }

            // Verify base deck does not have duplicates
            hashCards.Count.Should().Be(deckSize);

            return true;
        }
    }
}
