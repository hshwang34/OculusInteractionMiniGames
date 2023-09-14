using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBottles : MonoBehaviour
{
    public GameObject bottlePrefab;
    public int bottomRowBottles = 3;
    public GameObject stackMarker; // This is the empty GameObject you mentioned.

    private float bottleWidth;
    private float bottleHeight;



    private void Start()
    {
        if (!bottlePrefab) return;

        // Getting the dimensions of the bottle from its Box Collider.
        BoxCollider bottleCollider = bottlePrefab.GetComponent<BoxCollider>();
        if (bottleCollider)
        {
            bottleWidth = bottleCollider.size.x * bottlePrefab.transform.localScale.x;
            bottleHeight = bottleCollider.size.y * bottlePrefab.transform.localScale.y;
        }
        else
        {
            Debug.LogError("Ensure your bottle prefab has a Box Collider component!");
            return;
        }

        Vector3 startPosition = stackMarker.transform.position;
        startPosition.x -= bottleWidth * (bottomRowBottles - 1) / 2; // Centering the bottles on the stackMarker.

        int currentLayerCount = bottomRowBottles;

        // This variable keeps track of the current height at which bottles are being placed.
        float currentHeight = stackMarker.transform.position.y;

        // Keep stacking as long as there are more than 0 bottles in the current layer.
        while (currentLayerCount > 0)
        {
            for (int i = 0; i < currentLayerCount; i++)
            {
                Instantiate(bottlePrefab, new Vector3(startPosition.x + i * bottleWidth, currentHeight, startPosition.z), Quaternion.identity);
            }

            // Move to the next layer.
            currentHeight += bottleHeight;
            startPosition.x += bottleWidth / 2;
            currentLayerCount--;
        }
    }
}

