using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;

    private SpriteRenderer sr;
    private Selectable selectable;
    private Solitaire solitaire;

    

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();

        list<string> deck = solitaire.GenerateDeck();
        solitaire.FindObjectOfType<Solitaire>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
    }

    void Update()
    {
        if (selectable.faceUp == true)
        {
            sr.sprite = cardFace;
        }
        else
        {
            sr.sprite = cardBack;
        }
    }
}
