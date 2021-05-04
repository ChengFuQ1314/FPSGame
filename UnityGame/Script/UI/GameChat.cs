using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Handler;

public class GameChat : MonoBehaviour
{
    private UserData userdata = new UserData().getUserData();
	private float time = 0;
	private bool isControl = false;

	public InputField MessageInput;
	public Button SendMessageButton;
	public Text Message;
	public ScrollRect scrollRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		OnReleaseControl();
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
			SendMessage();
		}
	}

	public void SendButton() {
		SendMessage();
	}

	public void OnDisplay(string name,string msg) {
		if (msg != "" || name != "") {
			string newMessage = DateTime.Now.Hour.ToString() + ":"
							   + DateTime.Now.Minute.ToString() + ":"
							   + DateTime.Now.Second.ToString() + " "
							   + name + ":" + msg + "\n";
			Message.text += newMessage;
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = 0f;
			Canvas.ForceUpdateCanvases();
		}
	}

	private void SendMessage() {
		if (userdata.GameFlag == "ReleaseControl") {
			if (MessageInput.text != "") {
				new SyncPlayerChatHandler().OnSyncPlayerChatMessageSend(userdata,MessageInput.text);
				string newMessage = DateTime.Now.Hour.ToString() + ":"
									+ DateTime.Now.Minute.ToString() + ":"
									+ DateTime.Now.Second.ToString() + " "
									+ userdata.userattribute.name + ":" + MessageInput.text + "\n";
				Message.text += newMessage;
				Canvas.ForceUpdateCanvases();
				scrollRect.verticalNormalizedPosition = 0f;
				Canvas.ForceUpdateCanvases();
				MessageInput.text = "";
			}
		}
	}

	private void OnReleaseControl() {

		time += Time.deltaTime;
		if (time < 0.1f)
		{
			return;
		}
		else
		{
			time = 0f;
		}

		if (Input.GetKey(KeyCode.LeftAlt))
		{
			if (isControl)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				userdata.GameFlag = "Game";
				isControl = false;
			}
			else {
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				userdata.GameFlag = "ReleaseControl";
				isControl = true;
			}
		}
	}
}
