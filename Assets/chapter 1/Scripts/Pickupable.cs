using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Pickupable : Interactable
{
    public bool holdingE;
    public bool hovering;
    public bool charging = false;
    [Range(0, 1f)]public float itemCharge, sliderCharge;
    public float fillTime = 0f;
    public float evilFillTime = 0f;

    public UI uI;

    public GameObject key;
    //public Slider itemSlider;
    //public TextMeshProUGUI ammunitionDisplay;

    public PlayerController pc;
    private OpenDoor od;

    public void Awake()
    {
        uI = FindAnyObjectByType<UI>();
        pc = FindAnyObjectByType<PlayerController>();
        od = FindAnyObjectByType<OpenDoor>();

        uI.text1.SetText("");
        uI.text2.SetText("");
        uI.text3.SetText("");
    }

    public void Update()
    {
        itemCharge = Math.Clamp (itemCharge + (holdingE? Time.deltaTime: -Time.deltaTime), 0, 1f);
        sliderCharge = Math.Clamp (sliderCharge + (holdingE? Time.deltaTime : -Time.deltaTime), 0, 1f);

        uI.text1.SetText("Jacob is here!!! I must find the key!!!");

        if(holdingE && charging)
        {
            Debug.Log(gameObject.name + " p");
            //itemSlider.value = itemCharge;

        }
            

        if (itemCharge >= 0.05f) 
        {
            gameObject.SetActive(false);
            uI.text1.SetText("");
            uI.keyCollected.SetText("Key has been collected... Escape!!!");
            uI.keyHover.SetText("");
            od.keyCollected = true;
            Debug.Log("key is collected");
            //key.SetActive(false);
            //ammunitionDisplay.SetText( uI.itemsCollected + " / " + "5");
            //itemSlider.gameObject.SetActive(false);
        }
    }

    public override void MouseOver()
    {
        // Runs when the mouse Hovers Over this
        hovering = true;
        uI.keyHover.SetText("Grab Key");
    }

    public override void MouseExit()
    {
        // Runs when the mouse Exits this
        hovering = false;
        holdingE = false;
        charging = false;
        uI.keyHover.SetText("");
        //itemSlider.gameObject.SetActive(false);

    }

    public override void InteractStart()
    {
        // Runs when E is Pressed on the Object
        hovering = false;
        holdingE = true;
        charging = true;
        //itemSlider.gameObject.SetActive(true);
        //itemSlider.value = 0;

        //itemSlider.value += Time.deltaTime;
    }

    public override void InteractEnd()
    {
        // Runs when E is Released on the Object
        holdingE = false;
        charging = false;
        //itemSlider.gameObject.SetActive(false);
    }

    public void PerformAction()
    {
        Debug.Log("Action performed!"); // Replace with your desired action
    }
}
