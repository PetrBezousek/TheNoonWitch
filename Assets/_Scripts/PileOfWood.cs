using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PileOfWood : MonoBehaviour {

    List<WoodID> woods = new List<WoodID>();

    [SerializeField]
    GameObject log;

    [SerializeField]
    private bool small;

    [SerializeField]
    private float spawnX;

    [SerializeField]
    private float spawnY;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>().OnWoodAddedEvent += PileOfWood_OnWoodAddedEvent; ;

        foreach (Transform child in transform)
        {
            woods.Add(child.GetComponent<WoodID>());
        }

        woods = woods.OrderBy(x => x.pickOrder).ToList();
       
    }

    private void PileOfWood_OnWoodAddedEvent(bool smallWood)
    {
        if(small == smallWood)
        {
            if(woods.Count > 0)
            {
                RemoveNextWood();

                GameObject newWood = Instantiate(log,null);
                newWood.transform.position = new Vector3(spawnX, spawnY, 0);
            }
        }
    }

    private void RemoveNextWood()
    {
        Destroy(woods[0].gameObject);

        woods.RemoveAt(0);
    }
}
