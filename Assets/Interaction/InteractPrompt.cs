using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPrompt : MonoBehaviour
{

    BoxCollider2D zone;
    SpriteRenderer sprite;
    public string dialogue;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void PrintDialogue()
    {
        Debug.Log(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Jo")
        {
            sprite.color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Jo")
        {
            sprite.color = Color.clear;
        }
    }
}
