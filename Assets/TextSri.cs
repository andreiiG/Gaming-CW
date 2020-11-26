using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSri : MonoBehaviour
{
    public Canvas canvas;
    int i = 0;
    public Text textObject;
    public GameController controller;
    void Update()
    {
        if (Input.GetKeyDown("space") && i == 0)
        {
            i = 1;
            textObject.text = "The combat will be turn based: you and your opponent will take turns to move, attack or defend.On your turn, you will have to choose between attacking, moving forward, moving backwards, or blocking." +
                "  Press Space to continue ";
        }
        else 
        if (Input.GetKeyDown("space") && i == 1)
        {
            i = 2;
            controller = GameObject.Find("GameAssets").GetComponent<GameController>();
            controller.Begin();
            textObject.text = "Let me explain movement to you.To move forward, press W.";
        }else
        if (Input.GetKeyDown("w") && i == 2)
        {
            i = 3;
            textObject.text = "To move backwards, press S.";
        }
        else
        if (Input.GetKeyDown("s") && i == 3)
        {
            i = 4;
            textObject.text = "Now, I will explain attacking and blocking.You can only attack if you are close enough to your opponent. To attack, press A. Press Space to continue  ";
        }
        else
        if (Input.GetKeyDown("a") && i == 4)
        {
            i = 5;
            textObject.text = "When you block, you decrease the incoming damage by 3.Press D to block.";

        }
        else
        if (Input.GetKeyDown("d") && i == 5)
        {
            i = 6;
            textObject.text = "Your journey begins. You now have to defeat 3 opponents to set yourself free. Good luck! .Press Space to continue  ";
        }
        else
        if (Input.GetKeyDown("space") && i == 6)
        {
            textObject.text = " ";
            canvas.GetComponent<Canvas>().enabled = false;
        }
       
    }
}
