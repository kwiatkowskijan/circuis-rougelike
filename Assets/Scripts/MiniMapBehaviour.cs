using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapBehaviour : MonoBehaviour
{
    private RectTransform rectTransform;
    public GameObject mapCameraObject; // Ustaw obiekt z komponentem Camera z Unity Editor

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Sprawdü, czy obiekt MapCamera zosta≥ przypisany
        if (mapCameraObject != null)
        {
            Camera mapCamera = mapCameraObject.GetComponent<Camera>();

            if (mapCamera != null)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    rectTransform.sizeDelta = new Vector2(1920f, 1080f);
                    mapCamera.backgroundColor = new Color32(77, 77, 77, 125);
                }

                if (Input.GetKeyUp(KeyCode.M))
                {
                    rectTransform.sizeDelta = new Vector2(200f, 112.5f);
                    mapCamera.backgroundColor = new Color32(77, 77, 77, 255);
                }
            }
        }
    }
}
