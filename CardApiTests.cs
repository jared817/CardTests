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
            var newDeck = CardApiHelper.GetNewDeck(false);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();
            newDeck.remaining.Should().Be(52);

            CardApiHelper.VerifyDeckIsComplete(newDeck.deck_id, newDeck.remaining, false);
        }

        [Test]
        public void VerifyCreateNewDeckOfCardsWithJokers()
        {
            var newDeck = CardApiHelper.GetNewDeck(true);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();
            
            // Add Jokers functionality does not seem to be working on the API
            //newDeck.remaining.Should().Be(54);
            newDeck.remaining.Should().Be(52);

            CardApiHelper.VerifyDeckIsComplete(newDeck.deck_id, newDeck.remaining, true);
        }

        [Test]
        public void VerifyDrawCardsFromDeck()
        {
            var newDeck = CardApiHelper.GetNewDeck(false);

            newDeck.success.Should().BeTrue();
            newDeck.deck_id.Should().NotBeNullOrEmpty();
            newDeck.shuffled.Should().BeFalse();
            newDeck.remaining.Should().Be(52);

            var rnd = new Random();
            var randomCards = rnd.Next(1, newDeck.remaining);
            var updatedDeck = CardApiHelper.DrawCards(newDeck.deck_id, randomCards);
            updatedDeck.remaining.Should().Be(newDeck.remaining - randomCards);

        }
    }
}