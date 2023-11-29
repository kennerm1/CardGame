//Morgan and Brandon contributed to this file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
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
                    Deck(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Card"))
                {
                    Card(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Top"))
                {
                    Top(hit.collider.gameObject);
                }

                if (hit.collider.CompareTag("Tableau"))
                {
                    Tableau(hit.collider.gameObject);
                }
            }
        }
    }

    void Deck(GameObject selected)
    {
        solitaire.DrawNextCard();
        slot1 = this.gameObject;
        slot2 = this.gameObject;
    }

    void Card(GameObject selected)
    {

        if (!selected.GetComponent<Selectable>().faceUp) //flips the card up if possible
        {
            if (!solitaire.Blocked(selected))
            {
                selected.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }
        }
        if (selected.GetComponent<Selectable>().inDeckPile)
        {
            slot1 = this.gameObject;
            slot2 = this.gameObject;
        }
        if (slot1 == this.gameObject) //selects card if nothing selected
        {
            slot1 = selected;
        }

        else if (slot1 != selected) //if card is already selected, attempt to move if possible, else select new card
        {
            slot2 = selected;
            if (slot2.GetComponent<Selectable>().inTableau)
            {
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
    void Top(GameObject selected)
    {
        if(slot1 != this.gameObject)
        {
            string[] name = selected.name.Split('p');
            int pileNum = Int32.Parse(name[1]);
            solitaire.CheckTopPile(slot1.name, pileNum);
            slot1 = this.gameObject;
            slot2 = this.gameObject;
        }
    }

    void Tableau(GameObject selected)
    {
        if (slot1 != this.gameObject)
        {
            string[] name = selected.name.Split('u');
            int pileNum = Int32.Parse(name[1]);
            solitaire.CheckBottomPile(slot1.name, pileNum);
            slot1 = this.gameObject;
            slot2 = this.gameObject;
        }
    }
}
