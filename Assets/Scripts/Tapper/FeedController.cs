using System.Collections.Generic;
using UnityEngine;

public class FeedController : MonoBehaviour
{
    [SerializeField] TapperController tapperController;
    //... there's a way to import this so I can just use "Blood", right? ... Probably... ehh, time is of the essence. Learning is for later.
    public List<TapperController.Blood> bloodFeed;
    public List<GameObject> bloodObject;
    [SerializeField] private GameObject[] LiquidPrefab;

    [SerializeField] private float liquidLayerOffset = 2.0f;
    
    public void BuildBloodFeed()
    {
        Vector2 pos = transform.position;
        for (int i = 0; i < bloodFeed.Count; i++)
        {
            GameObject nextBlood = Instantiate(LiquidPrefab[(int)bloodFeed[i]], transform);
            nextBlood.transform.position = new Vector3(nextBlood.transform.position.x + liquidLayerOffset * i, nextBlood.transform.position.y,  nextBlood.transform.position.z - i);
            bloodObject.Add(nextBlood);
        }
    }
    
    public TapperController.Blood RemoveBlood()
    {
        TapperController.Blood b = bloodFeed[0];
        bloodFeed.RemoveAt(0);
        Destroy(bloodObject[0]);
        bloodObject.RemoveAt(0);

        if (bloodObject.Count < 1)
            tapperController.GameWon();
        //shift it all down by 1
        //... could add an animation here...? something jiggly... hmmmm
        foreach (GameObject blood in bloodObject)
            blood.transform.position = new Vector2(blood.transform.position.x - liquidLayerOffset, blood.transform.position.y);
        return b;
    }
}
