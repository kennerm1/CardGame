using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;
    private Solitaire solitaire;

    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
        slot2 = this.gameObject;
    }

    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Deck();
                }

                if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Top"))
                {
                    Top();
                }

                if (hit.collider.CompareTag("Tableau"))
                {
                    Tableau();
                }
            }
        }
    }

    void Deck()
    {
        Debug.Log("Clicked on deck");
        //solitaire.DealFromDeck();
    }

    void Card(GameObject selected)
    {
        Debug.Log("Clicked on card");

        if (!selected.GetComponent<Selectable>().faceUp) //flips the card up if possible
        {
            if (!solitaire.Blocked(selected))
            {
                selected.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }
        }

        if (slot1 == this.gameObject) //selects card if nothing selected
        {
            slot1 = selected;
            Debug.Log("Selected first card");
        }

        else if (slot1 != selected) //if card is already selected, attempt to move if possible, else select new card
        {
            slot2 = selected;
            Debug.Log("Selected second card");
            if (slot2.GetComponent<Selectable>().inTableau)
            {
                Debug.Log("In tableau");
                if (!solitaire.CheckBottomPile(slot1.name, slot2.GetComponent<Selectable>().pile))
                {
                    slot1 = selected;
                    slot2 = this.gameObject;
                }
            }
            else if (slot2.GetComponent<Selectable>().inTopPiles)
            {
                if (!solitaire.CheckTopPile(slot1.name, slot2.GetComponent<Selectable>().pile))
                {
                    slot1 = selected;
                    slot2 = this.gameObject;
                }
            }
        }
    }

    void Top()
    {
        Debug.Log("Clicked on top");
    }

    void Tableau()
    {
        Debug.Log("Clicked on tableau");
    }

    bool Stackable(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();

        if (!s2.inDeckPile)
        {
            if (s2.top)
            {
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
                {
                    if (s1.value == s2.value + 1)
                    {
                        return true;
                    }
                }

                else
                {
                    return false;
                }
            }

            else
            {
                if (s1.value == s2.value - 1)
                {
                    bool card1Red = true;
                    bool card2Red = true;

                    if (s1.suit == "C" || s1.suit == "S")
                    {
                        card1Red = false;
                    }

                    if (s2.suit == "C" || s2.suit == "S")
                    {
                        card2Red = false;
                    }

                    if (card1Red == card2Red)
                    {
                        print("Not stackable");
                        return false;
                    }

                    else
                    {
                        print("Stackable");
                        return true;
                    }
                }
            }
        }

        return false;

    }

    /*void Stack(GameObject selected)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        float yOffset = 0.3f;

        if (s2.top || (!s2.top && s1.value == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z -0.01f);
        slot1.transform.parent = selected.transform;

        if (s1.inDeckPile)
        {
            solitaire.CheckPileValue.Remove(slot1.name);
        }

        else if (s1.top && s2.top && s1.value == 1)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = null;
        }

        else if (s1.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value - 1;
        }

        else
        {
            solitaire.bottoms[s1.row].Remove(slot1.name);
        }

        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top)
        {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        }

        else
        {
            s1.top = false;
        }

        slot1 = this.gameObject;

    }*/


}