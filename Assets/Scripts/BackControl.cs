using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackControl : MonoBehaviour {
    
    public GameObject back;     //BackButton

    //Invoked when back button is clicked                            
    public void Click()
    {
        //Turn on flag
        LevelManager.isBackPressed = true;
        LevelManager.stack.Pop();
        //If stack is empty, load Layer 1, else load the naex layer in stack
        if (LevelManager.stack.Count == 0)
        {
            LevelManager.CreateBubble(null);
        }
        else
            LevelManager.CreateBubble(LevelManager.stack.Peek());
    }
}
