using System.Collections;
using UnityEngine;


public class BulettPowerUp : PowerUpBase
{
    public float sec = 10f;
    GameManager gunScript;
    public float timer;

    protected override void OnPickup()
    {
        //Call the default behaviour of the base class frist
        base.OnPickup();
        gunScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        gunScript.GunTurner(timer);
        int oldScore = PlayerPrefs.GetInt("score", 0);
        PlayerPrefs.SetInt("score", oldScore + 70);


        //turn on gun script for a limited time
    }
}
