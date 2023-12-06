using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer))]
public class CardState : MonoBehaviour
{
    public bool isOpen;// { get; set; }
    public int Number;// { get; set; }
    public bool MyCard;
    GameObject thisgameobject;
    Renderer thisrenderer;
    TextMeshProUGUI thistextMeshPro;
    public CardState Initialize(GameObject _thisgameobject, bool _myCard)
    {
        MyCard = _myCard;
        thisrenderer = GetComponent<Renderer>();
        isOpen = false;
        thisgameobject = _thisgameobject;
        thistextMeshPro = thisgameobject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        thistextMeshPro.text = "13";
        return this;
    }

    public void Open()
    {
        isOpen = true;
        thistextMeshPro.text = Number.ToString();
        thisrenderer.material.color = Color.white;
    }
    public void Close()
    {
        isOpen = false;
        thistextMeshPro.text = "";
        thisrenderer.material.color = Color.gray;
    }

    public void Clicked()
    {
        thisrenderer.material.color = Color.yellow;
    }
    public void UnClicked()
    {
        thisrenderer.material.color = Color.white;
    }

}
