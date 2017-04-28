using UnityEngine;
using System.Collections;

public class Bulett : MonoBehaviour {
    public float bulletSpeed;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(0, bulletSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision c)
    {
      
            Destroy(this.gameObject);
       

    }
}
