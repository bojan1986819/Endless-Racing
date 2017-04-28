using UnityEngine;

//Make sure there is always a BoxCollider component on the GameObject where this script is added.
[RequireComponent(typeof(BoxCollider))]
public class DropPowerUpOnHit : MonoBehaviour
{
    public int percentDrop1;
    public int percentDrop2;
    public int percentDrop3;
    public int percentDrop4;
    //Every powerup needs to be derived/inherit from PowerUpBase to ensure consistent behaviour
    public PowerUpBase PowerUpPrefab;
    public PowerUpBase PowerUpPrefab2;
    public PowerUpBase PowerUpPrefab3;
    public PowerUpBase PowerUpPrefab4;

    //OnCollision create the powerup
    void OnCollisionEnter(Collision c)
    {
        float rand = Random.Range(0, 100);

        if (rand < percentDrop1 && rand > 0)       
        {
            GameObject.Instantiate(PowerUpPrefab, this.transform.position, Quaternion.identity);
        }
        else if (rand < percentDrop1+percentDrop2 && rand > percentDrop1)        
        {
            GameObject.Instantiate(PowerUpPrefab2, this.transform.position, Quaternion.identity);
        }
        else if (rand < percentDrop1 + percentDrop2+percentDrop3 && rand > percentDrop1 + percentDrop2)        
        {
            GameObject.Instantiate(PowerUpPrefab3, this.transform.position, Quaternion.identity);
        }
        else if (rand < percentDrop1 + percentDrop2 + percentDrop3+percentDrop4 && rand > percentDrop1 + percentDrop2+percentDrop3)        
        {
            GameObject.Instantiate(PowerUpPrefab4, this.transform.position, Quaternion.identity);
        }
    }
}