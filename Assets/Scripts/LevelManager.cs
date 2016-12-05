using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static GameObject Canvas;    //Canvas to display text
    public GameObject back;     //Back Button
    public static List<BubbleData> Data = new List<BubbleData>();   //List to contain all the data from the hierarchical tree
    public static List<string> TopMost = new List<string>();    //To contain the elements of the Layer 1
    public static Stack<string> stack = new Stack<string>();    //Add bubbles that have been entered
    public static List<TextControl> textboxes = new List<TextControl>();    //Store bubbles and their respective textboxes
    
    /*****Flags*****/
    private static bool isFirstLevel = false;   //Triggered when Layer 1 is open
    public static bool isBackPressed = false;   //Triggered when back button pressed

    //Executed when the game is started
    void Start()
    {
        AddData();
        back.GetComponent<Button>().interactable = false;   //Disable back button
        CreateBubble(null);                                 //Create bubbles for Layer 1
    }

    //Called before rendering a frame
    void Update()
    {
        //Enable back button
        if (isFirstLevel == true)
            back.GetComponent<Button>().interactable = false;
        else
            back.GetComponent<Button>().interactable = true;

        //Display textbox on their respective bubble
        foreach(TextControl textbox in textboxes)
        {
            textbox.textbox.transform.position = Camera.main.WorldToScreenPoint(textbox.Bubble.transform.position);
        }
    }

    //Create a clone of the existing Prefab
    public static void createClone(string data)
    {
        //Create Bubble clone
        GameObject PrefabClone = Instantiate(Resources.Load("Bubble")) as GameObject;
        float rand = 2f;
        PrefabClone.transform.localScale = new Vector3(rand, rand, rand);
        BubbleContent bc = PrefabClone.GetComponent<BubbleContent>();
        bc.data = data;

        //Create textbox clone
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject PrefabClone1 = Instantiate(Resources.Load("Text")) as GameObject;
        PrefabClone1.transform.localScale = new Vector3(rand/3, rand/3, rand/3);
        PrefabClone1.transform.parent = Canvas.transform;
        Text text = PrefabClone1.GetComponent<Text>();
        text.text = data;

        //Add current bubble and respective textbox to list
        textboxes.Add(new TextControl { textbox = PrefabClone1, Bubble = PrefabClone});
    }

    //Destroy all game objects with tag = Bubble/Textbox
    public static void DestroyGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject target in gameObjects)
        {
            GameObject.Destroy(target);
        }
    }

    //Create childern Bubbles of 'clicked'
    public static void CreateBubble(string clicked)
    {
        string current;

        //when starting the game first time, create bubbles from TopMost for Layer 1
        if(clicked==null)
        {
            //Destroy existing bubbles and text
            DestroyGameObjectsWithTag("Bubble");
            DestroyGameObjectsWithTag("Text");
            //clear the list containing current elements
            textboxes.Clear();
            for (int i=0; i<TopMost.Count; i++)
            {
                current = TopMost[i];
                createClone(current);
            }

            //Reset Flags
            isFirstLevel = true;
            isBackPressed = false;
        }

        //When a bubble is clicked
        if(clicked!=null)
        {

            //create bubbles of children of 'clicked'
            for(int i=0; i<Data.Count; i++)
            {
                if(clicked == Data[i].data)
                {
                    if (isBackPressed == false)
                    {
                        //push to stack
                        stack.Push(clicked);
                    }
                    //Rsest Flag
                    isBackPressed = false;

                    //destroy current bubbles and text
                    DestroyGameObjectsWithTag("Bubble");
                    DestroyGameObjectsWithTag("Text");
                    textboxes.Clear();
                    //Create bubble for each child
                    foreach (string data in Data[i].subData)
                    {
                        createClone(data);
                    }

                    //Reset Flag
                    isFirstLevel = false;
                    break;
                }
            }

            //reset clicked to null
            clicked = null;
        }
    }

    private void AddData()
    {
        Data.Add(new BubbleData { data = "Music", subData = new List<string> { "Rock", "Classical", "Indie", "Pop", "Country", "Trance", "Hip-hop", "Rap", "Jazz", "Bollywood"} });

        Data.Add(new BubbleData { data = "Food", subData = new List<string> { "Gujarati", "Punjabi", "Italian", "Chinese"} });
        Data.Add(new BubbleData { data = "Gujarati", subData = new List<string> { "Dhokla", "Fafda", "Patra" } });
        Data.Add(new BubbleData { data = "Punjabi", subData = new List<string> { "Paneer", "Paratha", "Lassi" } });
        Data.Add(new BubbleData { data = "Italian", subData = new List<string> { "Pizza", "Gnocchi", "Bruschetta", "Panini", "Ravioli" } });
        Data.Add(new BubbleData { data = "Chinese", subData = new List<string> { "Noodles", "Manchurian" } });

        Data.Add(new BubbleData { data = "Countries", subData = new List<string> { "France", "England", "India", "Pakistan", "Australia", "Spain", "Russia" } });

        Data.Add(new BubbleData { data = "Movies", subData = new List<string> { "Sholay", "Vertigo", "Batman", "Seven", "Te3n", "Brazil", "Goodfellas", "Leon", "Godfather" } });

        Data.Add(new BubbleData { data = "Personality", subData = new List<string> { "Talkative", "Quiet", "Happy", "Sad", "Shy", "Outgoing", "Traditional", "Progressive", "Careful", "Careless", "Neat", "Messy", "Logical", "Creative" } });
    
        TopMost.Add("Music");
        TopMost.Add("Food");
        TopMost.Add("Countries");
        TopMost.Add("Movies");
        TopMost.Add("Personality");
    }
}
