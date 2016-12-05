using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BubbleControl : MonoBehaviour
{

    public Rigidbody2D rb2d;
    public BubbleData bd = new BubbleData();
    
    //Variables to detec single/double clicks
    private bool mouseClickStart = false;
    private int mouseClicks = 0;
    private float mouseTimerLimit = 0.25f;

    //Various size of bubble
    private Vector3 smallSize = new Vector3(2f, 2f, 2f);
    private Vector3 mediumSize = new Vector3(2.5f, 2.5f, 2.5f);
    private Vector3 largeSize = new Vector3(3f, 3f, 3f);

    // Use this for initialization
    void Start ()
    {
        //Apply a small force on the rigidbody of the bubble
        rb2d = GetComponent<Rigidbody2D>();
        Vector2 force = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        rb2d.AddForce(force, ForceMode2D.Impulse);
	}

    //When bubble is clicked, check if single or double
    void OnMouseDown()
    {
        mouseClicks++;
        if (mouseClickStart)
            return;
        mouseClickStart = true;
        Invoke("checkMouseDoubleClick", mouseTimerLimit);
    }

    private void checkMouseDoubleClick()
    {
        //If double click, load the layer with its children, else switch size
        if (mouseClicks == 2)
        {
            BubbleContent bc = GetComponent<BubbleContent>();
            print(bc.data);
            LevelManager.CreateBubble(bc.data);
        }
        else
        {
            if (gameObject.transform.localScale.x == smallSize.x)
                gameObject.transform.localScale = mediumSize;
            else if (gameObject.transform.localScale.x == mediumSize.x)
                gameObject.transform.localScale = largeSize;
            else if (gameObject.transform.localScale.x == largeSize.x)
                gameObject.transform.localScale = smallSize;
        }
            
        //Reset Flags
        mouseClickStart = false;
        mouseClicks = 0;
    }

    //Change posion of bubble with moise pointer when dragged
    void OnMouseDrag()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
