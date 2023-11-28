//Morgan contributed to this file
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
    private UserInput userInput;

    

    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();

        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                Debug.Log(card);
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        sr = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
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
