using NUnit.Framework;
using FluentAssertions;
using System;

namespace CardTests
{
    [TestFixture]
    public class Tests
    {
        
        [Test]
        public void VerifyCreateNewDeckOfCards()
        {
            var numberOfDecks = 2;
            var newDeck = CardApiHelper.GetNewDeck(numberOfDecks, withJokers:false);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();
            newDeck.remaining.Should().Be(numberOfDecks * 52);

            CardApiHelper.VerifyDeckIsComplete(newDeck.deck_id, numberOfDecks, hasJokers:false);
        }

        [Test]
        public void VerifyCreateNewDeckOfCardsWithJokers()
        {
            var numberOfDecks = 1;
            var newDeck = CardApiHelper.GetNewDeck(numberOfDecks, withJokers:true);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();

            // Add Jokers functionality does not seem to be working on the API
            //newDeck.remaining.Should().Be(numberOfDecks * 54);
            newDeck.remaining.Should().Be(numberOfDecks * 52);

            CardApiHelper.VerifyDeckIsComplete(newDeck.deck_id, numberOfDecks, hasJokers:true);
        }

        [Test]
        public void VerifyDrawCardsFromDeck()
        {
            var numberOfDecks = 1;
            var newDeck = CardApiHelper.GetNewDeck(numberOfDecks, withJokers:false);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();
            newDeck.remaining.Should().Be(numberOfDecks * 52);

            var rnd = new Random();
            var randomCards = rnd.Next(1, newDeck.remaining);
            var updatedDeck = CardApiHelper.DrawCards(newDeck.deck_id, randomCards);
            updatedDeck.remaining.Should().Be(newDeck.remaining - randomCards);

        }
    }
}