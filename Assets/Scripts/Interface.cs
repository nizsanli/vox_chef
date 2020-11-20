using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

    public Transform cam;
    public Transform chefBody;
    float grabDist = 2f;

    Rigidbody lckedObj;
    float tetherDist;
    Vector3 tetherO;
    Vector3 origRot;
    Vector3 tether;

	// Use this for initialization
	void Start () {
	    
	}

    void FixedUpdate()
    {
        if (lckedObj != null)
        {

            lckedObj.velocity = Vector3.zero;
            lckedObj.angularVelocity = Vector3.zero;

            if  (Input.GetMouseButton(1))
            {
                float frameRotZ = -GetComponent<FirstPerson>().frameRots.y;
                float frameRotX = -GetComponent<FirstPerson>().frameRots.x;
                Vector3 oldRot = lckedObj.transform.rotation.eulerAngles;
                Quaternion newRot = Quaternion.Euler(oldRot.x + frameRotX, oldRot.y, oldRot.z + frameRotZ);

                lckedObj.MovePosition(cam.position + cam.forward*tetherDist + cam.TransformVector(tetherO));
                lckedObj.MoveRotation(newRot);

                origRot = new Vector3(origRot.x + frameRotX, origRot.y, origRot.z + frameRotZ);
            }
            else
            {
                float frameRotY = GetComponent<FirstPerson>().frameRots.y;
                Vector3 oldRot = lckedObj.transform.rotation.eulerAngles;
                Quaternion newRot = Quaternion.Euler(origRot.x, oldRot.y + frameRotY, origRot.z);

                Vector3 newTether = cam.forward * tetherDist;
                Vector3 oldP = lckedObj.transform.position;

                Vector3 t = cam.TransformVector(tetherO);
                Vector3 newP = cam.position + newTether + t;

                lckedObj.MovePosition(newP);
                lckedObj.MoveRotation(newRot);

                StickySurface surf = lckedObj.GetComponent<StickySurface>();
                if (surf != null)
                {
                    Vector3 move = newP - oldP;
                    surf.MoveSurface(move, Quaternion.Euler(0f, frameRotY, 0f));
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(new Ray(cam.position, cam.forward), out hitInfo, grabDist, 0xff, QueryTriggerInteraction.Ignore))
            {
                Transform obj = hitInfo.collider.transform;
                Rigidbody rigid = obj.GetComponent<Rigidbody>();

                if (rigid == null)
                {
                    obj = hitInfo.collider.transform.parent;
                    rigid = obj.GetComponent<Rigidbody>();
                }
                if (rigid != null)
                {
                    tether = hitInfo.point - cam.position;
                    tetherDist = tether.magnitude;
                    tetherO = cam.InverseTransformVector(obj.position - hitInfo.point);

                    origRot = obj.rotation.eulerAngles;

                    lckedObj = rigid;
                    lckedObj.freezeRotation = true;
                    lckedObj.useGravity = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && lckedObj != null)
        {
            lckedObj.freezeRotation = false;
            lckedObj.useGravity = true;
            lckedObj = null;
        }
	}
}
