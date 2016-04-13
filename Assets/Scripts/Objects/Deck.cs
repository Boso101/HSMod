﻿using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public Player Player;

    public GameObject DeckObject;
    public GameObject HandObject;

    // This needs to be redesigned
    private List<GameObject> _currentDeck = new List<GameObject>();

    public List<BaseCard> Cards = new List<BaseCard>();

    public int Fatigue = 0;

    private void Start()
    {
        // Looping on all cards of the Deck
        for (int i = 0; i < DeckObject.transform.childCount; i++)
        {
            // Adding the card to the current deck
            _currentDeck.Add(DeckObject.transform.GetChild(i).gameObject);
        }

        // Shuffling all the cards in the deck
        for (int t = 0; t < _currentDeck.Count; t++)
        {
            GameObject tmp = _currentDeck[t];
            int random = Random.Range(t, _currentDeck.Count);
            _currentDeck[t] = _currentDeck[random];
            _currentDeck[random] = tmp;
        }

        // Drawing the 3 starting cards
        Draw(3);
    }

    public List<BaseCard> Draw(int draws)
    {
        List<BaseCard> drawnCards = new List<BaseCard>();

        // Looping x number of times
        for (int i = 0; i < draws; i++)
        {
            // Drawing a card
            BaseCard drawnCard = Draw();

            if (drawnCard != null)
            {
                drawnCards.Add(drawnCard);
            }
        }

        return drawnCards;
    }

    public BaseCard Draw()
    {
        if (Cards.Count > 0)
        {
            // Instantiating a new card
            GameObject drawnCard = GameObject.Instantiate(_currentDeck[0]);
            drawnCard.transform.localScale = drawnCard.transform.localScale/2;
            drawnCard.transform.SetParent(HandObject.transform);

            // Removing the instantiated card from the deck
            _currentDeck.RemoveAt(0);

            // Getting the BaseCard component from the instantiated card
            BaseCard drawnBaseCard = drawnCard.GetComponent<BaseCard>();

            // Triggering OnDrawn events (card and global)
            drawnBaseCard.OnDrawn();
            EventManager.Instance.OnCardDrawn(this.Player, drawnBaseCard);
            
            return drawnBaseCard;
        }
        else
        {
            // Add 1 to fatigue
            Fatigue++;

            // Deal fatigue damage
            this.Player.Hero.Damage(Fatigue);

            return null;
        }
    }

    public BaseCard MulliganDraw()
    {
        // Instantiating a new card
        GameObject drawnCard = GameObject.Instantiate(_currentDeck[0]);
        drawnCard.transform.localScale = drawnCard.transform.localScale / 2;
        drawnCard.transform.SetParent(HandObject.transform);

        // Removing the instantiated card from the deck
        _currentDeck.RemoveAt(0);

        // Getting the BaseCard component from the instantiated card
        return drawnCard.GetComponent<BaseCard>();
    }
}