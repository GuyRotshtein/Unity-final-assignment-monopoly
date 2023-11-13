using System;
using UnityEngine;
using System.Collections;
 
public class SC_CameraZoom : MonoBehaviour
{
    public Camera MainCamera;
    public float maxZoom;
    public float minZoom;
    public float panSpeed;
    private float currentZoom;
    private Vector3 CameraRotation;
    private Vector3 ScreenbottomLeft;
    private Vector3 ScreentopRight;
   
    float cameraMaxY;
    float cameraMinY;
    float cameraMaxX;
    float cameraMinX;

    private void Start()
    {
        if (!MainCamera)
        {
            MainCamera = this.GetComponent<Camera>();
        }
        currentZoom = MainCamera.orthographicSize;
        
        //set max camera bounds (assumes camera is max zoom and centered on Start)
        ScreentopRight = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, MainCamera.pixelHeight, -transform.position.z));
        ScreenbottomLeft = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0,0,-transform.position.z));
        cameraMaxX = ScreentopRight.x;
        cameraMaxY = ScreentopRight.y;
        cameraMinX = ScreenbottomLeft.x;
        cameraMinY = ScreenbottomLeft.y;
        maxZoom = 15f;
        minZoom = 4f;
        panSpeed = -1;
    }
   
    private void Update ()
    {
        if (Input.GetMouseButton(0)){MoveCamera();}
        if (Input.GetAxis("Mouse ScrollWheel") != 0){Zoom();}
        if (Input.GetMouseButton(1)){RotateCamera();}
        CheckBounds();
        
    }
    
    //drag the camera if possible across the game board
    private void MoveCamera()
    {
            float x = Input.GetAxis("Mouse X") * panSpeed;
            float y = Input.GetAxis("Mouse Y") * panSpeed;
            transform.Translate(x,y,0);
    }

    // zoom in on the user's mouse cursor
    private void Zoom()
    {
            currentZoom = MainCamera.orthographicSize;
        
            if((Input.GetAxis("Mouse ScrollWheel") > 0) &&  currentZoom > minZoom ) // forward
            {
                currentZoom -= 0.5f;
            }
            else if ((Input.GetAxis("Mouse ScrollWheel") < 0) && currentZoom < maxZoom) // back            
            {
                currentZoom += 0.5f;
            }
            Vector3 mousePos = Input.mousePosition;
        
            MainCamera.orthographicSize = currentZoom;
    }

    private void RotateCamera()
    {
        //get the mouse's movement direction and rotate accordingly
        if(Input.GetAxis("Mouse X") < 0)
        {
            // mouse is moving left!
        }
        else if (Input.GetAxis("Mouse X") > 0)
        {
            // mouse is moving right!
        }
        CameraRotation = new Vector3(0, 0, Input.GetAxis("Mouse X"));
        
        // change the camera's rotation values
        transform.eulerAngles = transform.eulerAngles - CameraRotation;
        
    }

    //checking that the camera isn't out-of-bounds, and if it is, moving back in-bounds
    private void CheckBounds()
    {
        ScreentopRight = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth, MainCamera.pixelHeight, -transform.position.z));
        ScreenbottomLeft = MainCamera.ScreenToWorldPoint(new Vector3(0,0,-transform.position.z));
        
        // calculating extra space for movement to account for the rotation of the board:
        float extraScreenSize = 283f * MathF.Sin(transform.eulerAngles.z * Mathf.Deg2Rad * 2);
        //Debug.Log(extraScreenSize);
        //Debug.Log("BEHOLD MY CREATION: " + (MathF.Cos(transform.eulerAngles.z * Mathf.Deg2Rad * 2) * 840 +extraScreenSize));
        
        if (ScreentopRight.x > cameraMaxX)
        {
            transform.position = new Vector3(transform.position.x - (ScreentopRight.x - cameraMaxX), transform.position.y, transform.position.z);
        }
        
        if(ScreentopRight.y > cameraMaxY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (ScreentopRight.y - cameraMaxY), transform.position.z);
        }
        
        if(ScreenbottomLeft.x < cameraMinX)
        {
            transform.position = new Vector3(transform.position.x + (cameraMinX - ScreenbottomLeft.x), transform.position.y, transform.position.z);
        }
       
        if(ScreenbottomLeft.y < cameraMinY)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (cameraMinY - ScreenbottomLeft.y), transform.position.z);
        }
    }
}