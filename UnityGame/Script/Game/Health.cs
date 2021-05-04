using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Handler;

public class Health : MonoBehaviour
{
    private int MaxHP = 100;
    private UserData userdata= new UserData().getUserData();
    public GameObject Arms_assault_rifle;
    public Slider slider;
    public int CurrentHP;
    public Text List;

    void Start()
    {
        CurrentHP = MaxHP;
    }

    public void OnHealth(int damage,string playername) {
        CurrentHP -= damage;
        slider.value = CurrentHP / (float)MaxHP;
        if (CurrentHP <= 0) {
            new SyncPlayerHealthRequest().OnSyncPlayerDeathRequest(playername,userdata.userattribute.name);
            userdata.userattribute.Deaths++;
            userdata.userattribute.Experience--;
            KillList(playername,userdata.userattribute.name);
            OnDeath();
        }
    }

    public void KillList(string killer,string player) {
        List.text = killer + " 击杀 " + player;
        if (killer == userdata.userattribute.name) {
            userdata.userattribute.Kills++;
            userdata.userattribute.Experience += 5;
        }
    }

    public void OnDeath() {
        transform.position = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().Resurrections[(int)Random.Range(0, 5)];
        CurrentHP = MaxHP;
        slider.value = CurrentHP / (float)MaxHP;
        Arms_assault_rifle.GetComponent<AutomaticGunScriptLPFP>().GunInit();
    }

 
}
