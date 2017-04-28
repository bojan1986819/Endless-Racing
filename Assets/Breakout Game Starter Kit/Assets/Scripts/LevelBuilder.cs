using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {

    public Object brick;
    public Object wall;

    public int brickNumber = 30;
    public int wallPercent = 90;
    public float radius = 1;
    public float blocksBottomPosition = 0;
    private Vector3 spawnPos;

	// Use this for initialization
	void Awake () {
        radius = radius * 1.2f;
        for (int i = 0; i < brickNumber; i++)
        {
            setPosition(blocksBottomPosition);

            while(Physics.CheckSphere(spawnPos, radius))
            {
                setPosition(blocksBottomPosition);
            }
            if (Random.Range(0, 100) > wallPercent)
            {
                Instantiate(wall, spawnPos, Quaternion.identity);
            }
            else
            {
                Instantiate(brick, spawnPos, Quaternion.identity);
            }
        }
    }

    public void setPosition(float distanceFromBottom)
    {
        spawnPos = new Vector3(Mathf.Round((Random.Range(0, 6f)) * 3.9f) - 12f, 0, Mathf.Round((Random.Range(0, 14.5f-distanceFromBottom)) * 1.6f) + distanceFromBottom-4.8f);
    }
}
