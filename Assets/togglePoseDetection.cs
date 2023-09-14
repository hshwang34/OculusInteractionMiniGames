using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class togglePoseDetection : MonoBehaviour
{
    public GameObject[] targetObjects; // The GameObject you want to toggle. Assign this in the inspector or through script.

    // This public function toggles the active status of the target object.
    public void ToggleGameObject()
    {
        if (targetObjects.Length != 0)
        {
            foreach (GameObject obj in targetObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(!obj.activeSelf);
                }
                else
                {
                    Debug.LogWarning("One of the GameObjects in the array is null on " + gameObject.name);
                }

            }
        }
        else
        {
            Debug.LogWarning("No target GameObject assigned to ToggleActive script on " + gameObject.name);
        }
    }
}
