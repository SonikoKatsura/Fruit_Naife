using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectsUI : MonoBehaviour {
    #region primitiveTypes
    //[SerializeField] private PrimitiveType[] primitiveTypes = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Cylinder };
    #endregion
    [SerializeField] List<GameObject> ObjectsList;

    [SerializeField] private Vector3 spawnPosition = new Vector3(1, 1, 2);
    [SerializeField] private Vector3 spawnRotation = new Vector3(45, 45, 0);
    [SerializeField] private Vector3 spawnScale = new Vector3(0.5f, 0.5f, 0.5f);

    private int currentIndex = -1;
    private bool hasBeenGenerated;
    private List<GameObject> objs = new List<GameObject>();

    private void Start() {
        GenerateObjects();
    }

    public void GenerateObjects() {
        if (currentIndex >= 0) {
            return;
        }

        #region primitiveTypes
        /*GameObject container = new GameObject("ObjectsContainer");
        for (int i = 0; i < primitiveTypes.Length; i++) {
            GameObject obj = GameObject.CreatePrimitive(primitiveTypes[i]);
            obj.transform.parent = container.transform;
            obj.transform.position = spawnPosition;
            obj.transform.rotation = Quaternion.Euler(spawnRotation);
            obj.transform.localScale = spawnScale;
            obj.SetActive(false);
            objs.Add(obj);
        }
        currentIndex = 0;
        objs[currentIndex].SetActive(true);
        hasBeenGenerated = true;*/
        #endregion

        GameObject container = new GameObject("ObjectsContainer");
        for (int i = 0; i < ObjectsList.Count; i++) {
            GameObject obj = Instantiate(ObjectsList[i]);
            obj.transform.parent = container.transform;
            obj.transform.position = spawnPosition;
            obj.transform.rotation = Quaternion.Euler(spawnRotation);
            obj.transform.localScale = spawnScale;
            obj.SetActive(false);
            objs.Add(obj);
        }
        currentIndex = 0;
        objs[currentIndex].SetActive(true);
        hasBeenGenerated = true;
    }

    public void RotateObj(Single sliderValue) {
        if (!hasBeenGenerated) {
            return;
        }
        objs[currentIndex].transform.eulerAngles = new Vector3(spawnRotation.x, 360 * sliderValue, spawnRotation.z);
    }

    public void ChangeObj(Int32 index) {
        if (!hasBeenGenerated) {
            return;
        }
        objs[currentIndex].SetActive(false);
        currentIndex = index;
        objs[currentIndex].SetActive(true);
    }

    public void ToggleObj(bool state) {
        if (!hasBeenGenerated) {
            return;
        }
        objs[currentIndex].SetActive(state);
    }
}