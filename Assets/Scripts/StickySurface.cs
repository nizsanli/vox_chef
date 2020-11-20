using UnityEngine;
using System.Collections;

public class StickySurface : MonoBehaviour {

    Hashtable table;
    public float friction;

	// Use this for initialization
	void Start () {
        table = new Hashtable();
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter(Collider collider)
    {
        int key = collider.transform.GetHashCode();
        if (!table.Contains(key))
        {
            table.Add(key, collider.transform);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        int key = collider.transform.GetHashCode();
        if (table.Contains(key))
        {
            table.Remove(key);
        }
    }

    public void MoveSurface(Vector3 move, Quaternion deltRot)
    {
        foreach(Transform obj in table.Values)
        {
            Rigidbody rigid = obj.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                Vector3 vec = obj.position - transform.position;
                Vector3 newP = (deltRot * vec) + transform.position + move * friction;

                Quaternion newRot = deltRot * obj.rotation;


                rigid.MovePosition(newP);
                rigid.MoveRotation(newRot);
            } 
        }
    }
}
