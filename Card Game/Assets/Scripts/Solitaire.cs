using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject drawPilePos;
    public GameObject drawnPos;
    public GameObject drawnCard;

    public static string[] suits = new string[] {"C", "D", "H", "S"};
    public static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public List<string>[] bottoms;
    public List<string>[] tops;

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    public List<string> deck;

    private int randRange;

    void Start()
    {
        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        PlayCards();
    }

    public void PlayCards()// initializes the game by shuffling, sorting, and dealing the starting cards
    {
        deck = GenerateDeck();
        Shuffle(deck);

        //test cards in deck:
        foreach(string card in deck)
        {
            print(card);
        }
        SolitaireSort();
        SolitaireDeal();
    }

    public static List<string> GenerateDeck()// creates the deck based off of the template deck
    {
        List<string> newDeck = new List<string>();
        foreach(string s in suits)
        {
            foreach(string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    void Shuffle<T>(List<T> list)// shuffles the starting deck
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while(n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    void SolitaireDeal()// physically moves the cards into their starting piles, and flips the top card in each pile
    {
        for (int i = 0; i < 7; i++)
        {

            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (string card in bottoms[i])// for every card in each pile
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;//names the card so it can be referenced later
                if (card == bottoms.ElementAt(i)[bottoms.ElementAt(i).Count - 1])
                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }

                yOffset = yOffset + 0.1f;//offsets each card so they aren't all stacked on top of each other
                zOffset = zOffset + 0.03f;
            }
        }
    }

    void SolitaireSort()//sorts all the starting cards into their individual piles
    {
        //int cardCount = 0;
        //assign all cards to the bottoms of each pile
        for (int i = 1; i < 8; i++) //for each pile
        {
            for (int j = 0; j < i; j++) //add that many cards to the pile and remove it from the starting deck
            {
                //bottoms.ElementAt(i - 1).Add(deck.ElementAt(cardCount));
                //deck.RemoveAt(cardCount);
                //cardCount++;

                bottoms[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    void DrawNextCard() // draws the next card from the draw pile
    {
        GameObject drawnCard = GameObject.Find(deck.ElementAt(0)); // find the revealed card gameobject
        drawnCard.transform.position = new Vector3(drawPilePos.transform.position.x, drawPilePos.transform.position.y, drawPilePos.transform.position.z); // move the current revealed card
        drawnCard.GetComponent<Selectable>().faceUp = false; // flip it back over
        deck.Insert(deck.Count - 1, deck.ElementAt(0)); // add a copy of the revealed card to the back
        deck.RemoveAt(0); // remove the revealed card
        drawnCard = GameObject.Find(deck.ElementAt(0)); // set the revealed card to the next card
        drawnCard.transform.position = new Vector3(drawnPos.transform.position.x, drawnPos.transform.position.y, drawnPos.transform.position.z); // move the new first card to the right pos
        drawnCard.GetComponent<Selectable>().faceUp = true; // flip the new first card
    }

}
