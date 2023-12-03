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
    private bool typing;
    JoController curjo;
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
        if (started && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return)))
        {
            if (index < dialogue.Length - 1 && !typing)
            {
                index++;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
            else if (!typing)
            {
                EraseText();
            }
        }
        if (started && Input.GetKeyDown(KeyCode.Escape))
        {
            EraseText();
        }
    }
    public void EraseText()
    {
        dialogueText.text = "";
        index = 0;
        textPanel.SetActive(false);
        started = false;

        curjo.busy = false;
    }

    public void PrintDialogue(JoController jo)
    {
        curjo = jo;
        textPanel.SetActive(true);
        dialogueText.text = "";
        StartCoroutine(Typing());

        started = true;
    }

    IEnumerator Typing()
    {
        typing = true;
        foreach(char letter in dialogue[index])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.05f);
        }
        typing = false;
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
