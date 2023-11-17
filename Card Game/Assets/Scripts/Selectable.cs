using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public string suit;
    public int value;
    public int row;
    public bool faceUp = false;
    public bool top = false;
    public bool inDeckPile = false;
    public bool inTopPiles = false;
    public bool inTableau = false;
    public int pile;

    private string valueString;

    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("Card"))
        {
            suit = transform.name[0].ToString();

            for (int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
