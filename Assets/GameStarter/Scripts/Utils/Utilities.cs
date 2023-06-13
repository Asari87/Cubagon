using System.Collections; 
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

public class Utilities : MonoBehaviour
{
    public static TMP_Text CreateWorldText(Transform parent, Vector3 worldPosition, string text, Color color , float fontSize = 5,  TextAlignmentOptions textAlignment = TextAlignmentOptions.Center, bool wrapping = false)
    {
        GameObject textObject = new GameObject();
        Transform textTransform = textObject.transform;
        textTransform.position = worldPosition;
        textTransform.SetParent(parent, false);

        TextMeshPro textComponent = textObject.AddComponent<TextMeshPro>();
        textComponent.text = text; 
        textComponent.color = color;
        textComponent.fontSize = fontSize;
        textComponent.alignment = textAlignment;
        textComponent.enableWordWrapping = wrapping;

        return textComponent;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mPos.z = 0;
        return mPos;
    }
    public static Vector3 GetMouseWorldPosition(Vector2 screenPoint)
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(screenPoint);
        mPos.z = 0;
        return mPos;
    }
}
