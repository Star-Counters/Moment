using UnityEngine;
using System.Collections;

public class ChatPanel : MonoBehaviour
{
    public static ChatPanel Instance { get { return instance; } }
    private static ChatPanel instance;
    public UILabel chatLabel;
    // Use this for initialization
    void Awake () {
       // Debug.Log("YIYI");
        instance = this;
        if (chatLabel == null)
            chatLabel = transform.FindChild("Back/ChatLabel").GetComponent<UILabel>();
        gameObject.SetActive(false);
	}
    public void ShowChatBox(string chatText) {
        chatLabel.text = chatText;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
