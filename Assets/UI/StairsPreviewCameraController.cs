using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class StairsPreviewCameraController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FreeCamera freeCamera;

    Vector3 position;
    Quaternion rotation;
    
    private void Awake()
    {
        freeCamera.enabled = false;
        
        //save position and rotation
        position = freeCamera.transform.position;
        rotation = freeCamera.transform.rotation;
    }
    
    private void Update()
    {
        if (freeCamera.enabled && Input.GetKey(KeyCode.Space))
        {
            freeCamera.transform.position = position;
            freeCamera.transform.rotation = rotation;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            freeCamera.enabled = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        freeCamera.enabled = false;
    }
}