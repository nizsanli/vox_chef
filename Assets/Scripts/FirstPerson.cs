using UnityEngine;
using System.Collections;

public class FirstPerson : MonoBehaviour {

    Rigidbody rigid;
    public Transform body;
    public Transform cam;

    public Vector3 frameRots;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector3 move = body.right * Input.GetAxisRaw("Horizontal") + body.forward * Input.GetAxisRaw("Vertical");
        float moveSpeed = 2f * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = moveSpeed * 0.5f;
        }
        rigid.MovePosition(transform.position + move * moveSpeed);

        float mouseX = Input.GetAxisRaw("Mouse X");
        float xSensitivity = 180f;
        float mouseY = Input.GetAxisRaw("Mouse Y");
        float ySensitivity = 180f;
        frameRots = new Vector3(-mouseY * ySensitivity * Time.deltaTime, mouseX * xSensitivity * Time.deltaTime, 0f);
        if (!Input.GetMouseButton(1))
        {
            // no cam rotation on right mouse
            body.Rotate(new Vector3(0f, frameRots.y, 0f));
            cam.Rotate(new Vector3(frameRots.x, 0f, 0f));
        }

        float jumpForce = 1f;
        if (Physics.Raycast(new Ray(new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z), Vector3.down),
            0.05f) && Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break();
        }
        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
