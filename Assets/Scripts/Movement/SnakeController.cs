using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private List<Transform> bones;
    private Transform head;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bones = new List<Transform>();
        Transform[] temp = GetComponentsInChildren<Transform>(true);
        foreach(Transform t in temp)
        {
            if (t.gameObject.name.Length == 8 && t.gameObject.name.Substring(0, 5) == "Bone.") bones.Add(t);
        }
        head = bones[bones.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<bones.Count; i++)
        {
            bones[i].localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(i+1*Time.time)*45);
        }
    }
}
