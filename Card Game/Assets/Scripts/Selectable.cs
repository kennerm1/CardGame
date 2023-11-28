//Morgan and Brandon contributed to this file
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

    [SerializeField] AudioClip[] _clips;

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

    void OnMouseDown()
    {
        int index = UnityEngine.Random.Range(0, _clips.Length);
        AudioClip clip = _clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

}
