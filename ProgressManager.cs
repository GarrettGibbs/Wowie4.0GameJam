using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public bool firstTimeAtMenu = true;

    public bool[] levelsDone = new bool[5] {false, false, false, false, false};

    public bool alreadyBegunLevel = false;
    public bool hasMetTommy = false;

    public bool secondConversation = false;
    public bool thirdConversation = false;
    public bool fourthConversation = false;
    public bool fifthConversation = false;
    public bool sixthConversation = false;

    public bool pushedTheButton = false;
    
    public bool gotSecretPickup = false;

    public static ProgressManager instance;
    

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
