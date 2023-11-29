using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractPrompt : MonoBehaviour
{
    public GameObject textPanel;
    public Text dialogueText;
    BoxCollider2D zone;
    SpriteRenderer sprite;
    public TextAsset textAsset;
    public Sprite talker;
    public Color background;
    string[] dialogue;
    private bool started;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        //make the text file a list of strings
        dialogue = textAsset.text.Split('\n');
        index = 0;

        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(started && Input.GetKeyDown(KeyCode.Space))
        {
            if (index < dialogue.Length - 1)
            {
                index++;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
            else {
                textPanel.SetActive(false);
                started = false;
            }
        }
    }

    public void PrintDialogue()
    {
        textPanel.SetActive(true);
        dialogueText.text = "";
        StartCoroutine(Typing());

        started = true;
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
    }

    public void NextLine()
    {
        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
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
