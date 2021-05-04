using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Script.Handler;

public class GamePopup : MonoBehaviour
{
    private bool isPopup;
    private float time;
    private UserData userdata = new UserData().getUserData();
    public Image Popup;

    public void ReturnLoddy() {
        userdata.GameFlag = "Loddy";
        userdata.GameDataSend();
        new SyncPlayerLeaveHandler().OnSyncPlayerLeaveSend(userdata);
        SceneManager.LoadScene("Loddy");
    }

    void Start()
    {
        isPopup = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        OnPopup();
    }

	private void OnDestroy()
	{
        new SyncPlayerLeaveHandler().OnSyncPlayerLeaveSend(userdata);
    }

	private void OnPopup() {
        time += Time.deltaTime;
        if (time < 0.1f)
        {
            return;
        }
        else {
            time = 0f;
        }

        if (isPopup != true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                userdata.GameFlag = "GamePopup";
                Popup.gameObject.SetActive(true);
                isPopup = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                userdata.GameFlag = "Game";
                Popup.gameObject.SetActive(false);
                isPopup = false;
            }
        }
    }
}
