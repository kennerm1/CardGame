using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public GameObject drawPilePos;
    public GameObject drawnPos;
    public GameObject drawnCard;

    [SerializeField] AudioClip[] _clips;

    public static string[] suits = new string[] { "C", "D", "H", "S" };
    public static string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string>[] bottoms;
    public List<string>[] tops;

    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();

    private List<string> top0 = new List<string>();
    private List<string> top1 = new List<string>();
    private List<string> top2 = new List<string>();
    private List<string> top3 = new List<string>();

    public List<string> deck;

    private int randRange;

    void Start()
    {
        int index = UnityEngine.Random.Range(0, _clips.Length);
        AudioClip clip = _clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);

        bottoms = new List<string>[] { bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
        tops = new List<string>[] { top0, top1, top2, top3 };
        PlayCards();
    }

    public void PlayCards()// initializes the game by shuffling, sorting, and dealing the starting cards
    {
        deck = GenerateDeck();
        Shuffle(deck);

        //test cards in deck:
        /*foreach(string card in deck)
        {
            print(card);
        }*/
        SolitaireSort();
        SolitaireDeal();
    }

    public static List<string> GenerateDeck()// creates the deck based off of the template deck
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
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
        while (n > 1)
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
                newCard.GetComponent<Selectable>().row = i;
                newCard.GetComponent<Selectable>().inTableau = true;
                newCard.GetComponent<Selectable>().pile = i;
                if (card == bottoms[i].ElementAt(bottoms[i].Count - 1))

                {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }

                yOffset = yOffset + 0.3f;//offsets each card so they aren't all stacked on top of each other
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

    public void DrawNextCard() // draws the next card from the draw pile
    {
        GameObject drawnCard = GameObject.Find(deck.ElementAt(0)); // find the revealed card gameobject
        if (drawnCard == null)
        {
            drawnCard = Instantiate(cardPrefab, new Vector3(drawnPos.transform.position.x, drawnPos.transform.position.y, drawnPos.transform.position.z + 0.03f), Quaternion.identity, drawnPos.transform);
            drawnCard.name = deck.ElementAt(0);
            drawnCard.GetComponent<Selectable>().inDeckPile = true;
        }
        Debug.Log(deck.ElementAt(0));
        drawnCard.transform.position = new Vector3(drawPilePos.transform.position.x, drawPilePos.transform.position.y, drawPilePos.transform.position.z + 0.03f); // move the current revealed card
        drawnCard.GetComponent<Selectable>().faceUp = false; // flip it back over
        deck.Insert(deck.Count - 1, deck.ElementAt(0)); // add a copy of the revealed card to the back
        deck.RemoveAt(0); // remove the revealed card
        drawnCard = GameObject.Find(deck.ElementAt(0)); // set the revealed card to the next card
        if (drawnCard == null)
        {
            drawnCard = Instantiate(cardPrefab, new Vector3(drawnPos.transform.position.x, drawnPos.transform.position.y, drawnPos.transform.position.z + 0.03f), Quaternion.identity, drawnPos.transform);
            drawnCard.name = deck.ElementAt(0);
            drawnCard.GetComponent<Selectable>().inDeckPile = true;
        }
        drawnCard.transform.position = new Vector3(drawnPos.transform.position.x, drawnPos.transform.position.y, drawnPos.transform.position.z + 0.03f); // move the new first card to the right pos
        drawnCard.GetComponent<Selectable>().faceUp = true; // flip the new first card
    }

    // Checks the values of the cards being stacked, only call after determining suit elsewhere
    private bool CheckPileValue(string cardName, int mode, int pileNum)
    {
        string s = cardName[0].ToString();
        string v = cardName[1].ToString();
        if (v == "1")
            v = v + "0";
        List<string> stack = new List<string>();
        GameObject card = GameObject.Find(cardName);
        Selectable cardSelectable = card.GetComponent<Selectable>();
        if (mode == 1)
        {
            string topCardName = null;
            GameObject topCard = null;
            string topValue = null;
            if (bottoms[pileNum].Count != 0)
            {
                topCardName = bottoms[pileNum].Last();
                topCard = GameObject.Find(topCardName);
                topValue = topCardName[1].ToString();
            }
            if (topValue == "1")
                topValue = topValue + "0";
            Debug.Log(Array.IndexOf(values, v));
            Debug.Log(Array.IndexOf(values, topValue));
            if (Array.IndexOf(values, v) == Array.IndexOf(values, topValue) - 1 || (bottoms[pileNum].Count == 0 && Array.IndexOf(values, v) == 12)) //must ensure that a blank pile has a value of 14 so that only king can be placed
            {
                Debug.Log("Correct value");

                if (cardSelectable.inDeckPile)
                    deck.RemoveAt(0);

                for (int i = 0; i < 7; i++) //for every bottom pile
                {
                    if (bottoms[i].Contains(cardName)) //check if that pile contains the old card
                    {
                        for (int j = bottoms[i].IndexOf(cardName); j < bottoms[i].Count; j++) //create a stack of the cards being moved and remove that stack from the old pile
                        {
                            Debug.Log("Adding: " + bottoms[i].ElementAt(j));
                            stack.Add(bottoms[i].ElementAt(j));
                        }
                        for (int k = 0; k < stack.Count; k++)
                        {
                            bottoms[i].RemoveAt(bottoms[i].Count - 1);
                        }
                        break;
                    }
                }
                if (stack.Count == 0)
                {
                    stack.Add(cardName);
                    Debug.Log("success");
                }
                for (int i = 0; i < stack.Count; i++) //add the stack to the new pile
                {
                    Debug.Log(stack.Count);
                    Debug.Log("Moving card: " + stack.ElementAt(i));

                    card = GameObject.Find(stack.ElementAt(i));
                    cardSelectable = card.GetComponent<Selectable>();
                    if (bottoms[pileNum].Count != 0)
                        card.transform.position = new Vector3(topCard.transform.position.x, topCard.transform.position.y - 0.3f, topCard.transform.position.z - 0.03f);
                    else
                        card.transform.position = new Vector3(bottomPos[pileNum].transform.position.x, bottomPos[pileNum].transform.position.y, bottomPos[pileNum].transform.position.z - 0.03f);
                    bottoms[pileNum].Add(stack.ElementAt(i));
                    cardSelectable.pile = pileNum;
                    cardSelectable.inTableau = true;
                    cardSelectable.inTopPiles = false;
                    cardSelectable.inDeckPile = false;
                    topCardName = bottoms[pileNum].Last();
                    topCard = GameObject.Find(topCardName);
                }
                return true;
            }
        }
        else
        {
            Debug.Log("pile num: " + pileNum);
            Debug.Log("tops count: " + tops[pileNum].Count);
            string topCardName = null;
            GameObject topCard = null;
            string topValue = null;
            if (cardSelectable.inDeckPile)
                deck.RemoveAt(0);
            if (tops[pileNum].Count != 0)
            {
                topCardName = tops[pileNum].Last();
                topValue = topCardName[1].ToString();
                topCard = GameObject.Find(topCardName);
                Debug.Log(topCardName);
            }
            if (topValue == "1")
                topValue = topValue + "0";
            Debug.Log("" + Array.IndexOf(values, v) + " " + Array.IndexOf(values, topValue));
            if (Array.IndexOf(values, v) == Array.IndexOf(values, topValue) + 1 || (tops[pileNum].Count == 0 && Array.IndexOf(values, v) == 0)) //empty pile needs to have a value of 0 so that only ace can be placed
            {
                Debug.Log("wtf");
                if (tops[pileNum].Count != 0)
                    card.transform.position = new Vector3(topCard.transform.position.x, topCard.transform.position.y - 0.3f, topCard.transform.position.z - 0.03f);
                else
                    card.transform.position = new Vector3(topPos[pileNum].transform.position.x, topPos[pileNum].transform.position.y, topPos[pileNum].transform.position.z - 0.03f);
                tops[pileNum].Add(cardName);
                cardSelectable.pile = pileNum;
                cardSelectable.inTopPiles = true;
                cardSelectable.inTableau = false;
                cardSelectable.inDeckPile = false;
                for (int i = 0; i < 7; i++) //for every pile
                {
                    if (bottoms[i].Contains(cardName)) //check if that pile contains the old card
                    {
                        bottoms[i].RemoveAt(bottoms[i].IndexOf(cardName));
                        break;
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool CheckBottomPile(string cardName, int pileNum) // checks to make sure that the card being added is the correct suit for the bottom piles
    {
        string s = cardName[0].ToString();
        string topCardName = null;
        string topSuit = null;
        if (bottoms[pileNum].Count != 0)
        {
            topCardName = bottoms[pileNum].Last();
            topSuit = topCardName[0].ToString();
        }
        Debug.Log(s);
        Debug.Log(topSuit);
        if (s == "C" || s == "S")
        {
            if (topSuit == "D" || topSuit == "H" || topSuit == null)
            {
                Debug.Log("Correct suit");
                return CheckPileValue(cardName, 1, pileNum);
            }
        }
        else
        {
            if (topSuit == "C" || topSuit == "S" || topSuit == null)
            {
                Debug.Log("Correct suit");
                return CheckPileValue(cardName, 1, pileNum);
            }
        }
        return false;
    }

    public bool CheckTopPile(string cardName, int pileNum) // checks to make sure that the card being added is the correct suit for the top piles
    {
        Debug.Log(pileNum);
        string topCardName = null;
        string topSuit = null;
        if (tops[pileNum].Count != 0)
        {
            topCardName = tops[pileNum].Last();
            topSuit = topCardName[0].ToString();
        }
        string s = cardName[0].ToString();
        if (s == topSuit || topSuit == null)
        {
            Debug.Log("Correct Suit");
            return CheckPileValue(cardName, 2, pileNum);
        }
        return false;
    }

    public bool Blocked(GameObject selected)
    {
        Selectable selectable = selected.GetComponent<Selectable>();
        if (selectable.inTableau)
        {
            if (selected.name == bottoms[selectable.pile].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (selectable.inTopPiles)
        {
            if (selected.name == tops[selectable.pile].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            print("In neither tableau nor tops");
            return true;
        }
    }

}
