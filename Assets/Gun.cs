using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public GameObject projectile1;
    public float bulletWait;
    public float nextFire;


	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + bulletWait;
            GameObject clone = Instantiate(projectile1,transform.position, transform.rotation) as GameObject;
        }
	
	}
}
