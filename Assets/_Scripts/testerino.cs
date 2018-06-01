using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testerino : MonoBehaviour {

    [SerializeField]
    int forceX;

    [SerializeField]
    int forceY;

    [SerializeField]
    int positionX;

    [SerializeField]
    int positionY;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Shoot!");
            // GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY));
            GetComponent<Rigidbody2D>().MovePosition(new Vector2(forceX, forceY));
        }


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Reset");
            transform.position = new Vector3(positionX, positionY);
        }
    }
}
