using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    [SerializeField]
    Vector3 rightPos;
    [SerializeField]
    Vector3 leftPos;
    [SerializeField]
    float distance;
    [SerializeField]
    float speed;

    private void Awake()
    {
        leftPos = new Vector3(transform.localPosition.x - distance, 0, 0);
        rightPos = new Vector3(transform.localPosition.x + distance, 0, 0); 
    }

    // Use this for initialization
    void Start () {
        
        
        StartCoroutine(MoveCoin(rightPos));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator MoveCoin(Vector3 pos)
    {
        
       
        while (Mathf.Abs((pos.x - transform.localPosition.x))>0.2f) {
     
            Vector3 dir = pos.x == leftPos.x ? Vector3.left : Vector3.right;
            //moves coin
            transform.localPosition += dir * Time.deltaTime * speed;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Vector3 nextDir = pos.x == leftPos.x ? rightPos : leftPos;
        StartCoroutine(MoveCoin(nextDir));
        
    }
}
