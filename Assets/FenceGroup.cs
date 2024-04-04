using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
class Fence
{
    public float length;
    public float angle;
}

[System.Serializable]
class FenceJSON
{
    public Fence[] fences;
}

public class FenceGroup : MonoBehaviour
{
    public GameObject fenceAsset;
    private ArrayList fenceObjects = new();

    public void SyncFromJSON(string json)
    {
        Clear();

        FenceJSON result = JsonUtility.FromJson<FenceJSON>(json);
        Fence[] fences = result.fences;

        float cumulativeAngle = 0; // ugly hack because of euler angles
        foreach (Fence fence in fences) {
            cumulativeAngle += fence.angle;
            createFence(fence.length, cumulativeAngle);
        }


    }

    public void Clear()
    {
       foreach (GameObject fence in fenceObjects)
        {
            Destroy(fence);
        }
        fenceObjects.Clear();
    }

    public GameObject createFence(float length, float angle)
    {
        var fence = Instantiate(fenceAsset);

        fence.transform.Rotate(0, angle, 0);
        fence.transform.localScale = new Vector3(0.1f, 1, length);

        if (fenceObjects.Count > 0)
        {
            Transform lastTransform = ((GameObject)fenceObjects[fenceObjects.Count - 1]).transform;
            fence.transform.position = lastTransform.position +
                (lastTransform.forward * lastTransform.localScale.z/2) +
                (fence.transform.forward * (length/2));
        }
        else
        {
            fence.transform.position = (fence.transform.forward * (length / 2));
        }

        fenceObjects.Add(fence);
        return fence;
    }

}
