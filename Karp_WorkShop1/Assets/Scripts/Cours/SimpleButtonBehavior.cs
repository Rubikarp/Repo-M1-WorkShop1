using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SimpleButtonBehavior : MonoBehaviour
{
    public Image selfImage;
    public RectTransform self;
    public TextMeshProUGUI myText;

    public Color hoverColor, unhoverdeColor;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void OnHover()
    {
        selfImage.color = hoverColor;
    }

    public void OnUnhover()
    {
        selfImage.color = unhoverdeColor;
    }

    public void OnDrag(BaseEventData bed)
    {
        PointerEventData ped = bed as PointerEventData;
        self.anchoredPosition += ped.delta;
    }
}
