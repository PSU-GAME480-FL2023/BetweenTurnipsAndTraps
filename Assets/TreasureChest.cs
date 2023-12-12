using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public int cash;
    public bool open;
    public Sprite opened;
    SpriteRenderer rend;
    private GameManager gameManager;
    public TextMeshProUGUI moneyElement;
    AudioSource source;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        if(cash == 0)
        {
            cash = 5;
        }
    }
    
    public void OpenChest()
    {
        if (!open)
        {
            source.Play();
            gameManager.money += cash;
            rend.sprite = opened;
            moneyElement.text = gameManager.money.ToString();
        }
    }
}
