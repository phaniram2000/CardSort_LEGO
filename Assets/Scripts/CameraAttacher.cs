using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CameraAttacher : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

}
