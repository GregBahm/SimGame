using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public Material BaseMaterial;
    public TowerRowDefinition[] RowDefinitions;

    public int Rotation;
    public float CardSize;

    private ArtTower towerOfArt;

    private void Start()
    {
        towerOfArt = new ArtTower(BaseMaterial, RowDefinitions);
    }

    private void Update()
    {
        Vector3 localScale = new Vector3(CardSize, CardSize, CardSize);
        towerOfArt.TowerRoot.rotation = Quaternion.Euler(0, Rotation, 0);
        foreach (ArtTowerRow row in towerOfArt.Rows)
        {
            foreach (Transform item in row.Transforms)
            {
                item.rotation = Quaternion.identity;
                item.localScale = localScale;
            }
        }
    }
}

[Serializable]
public class TowerRowDefinition
{
    public Texture2D[] Textures;
}

public class ArtTower
{
    public Transform TowerRoot { get; }
    public IReadOnlyCollection<ArtTowerRow> Rows { get; }

    public ArtTower(Material baseMat, TowerRowDefinition[] rowDefinitions)
    {
        TowerRoot = new GameObject("TowerRoot").transform;
        Rows = GetRows(baseMat, rowDefinitions);
    }

    private ArtTowerRow[] GetRows(Material baseMat, TowerRowDefinition[] rowSlots)
    {
        ArtTowerRow[] ret = new ArtTowerRow[rowSlots.Length];
        for (int i = 0; i < rowSlots.Length; i++)
        {
            ret[i] = new ArtTowerRow(i, baseMat, rowSlots[i], TowerRoot);
        }
        return ret;
    }
}
public class ArtTowerRow
{
    public IReadOnlyCollection<Transform> Transforms { get; }

    public ArtTowerRow(int row, Material mat, TowerRowDefinition definition, Transform towerRoot)
    {
        Transforms = CreateTransforms(row, mat, definition, towerRoot);
    }

    private Transform[] CreateTransforms(int row, Material baseMat, TowerRowDefinition definition, Transform towerRoot)
    {
        GameObject centerPointObj = new GameObject("Row " + row + " CenterPoint");
        centerPointObj.transform.parent = towerRoot;
        int itemsCount = definition.Textures.Length;
        float rotation = 360f / itemsCount;

        Transform[] ret = new Transform[definition.Textures.Length];
        for (int i = 0; i < itemsCount; i++)
        {
            Texture2D texture = definition.Textures[i];
            ret[i] = CreateCard(centerPointObj.transform, baseMat, texture, itemsCount, -row);
            centerPointObj.transform.Rotate(0, rotation, 0);
        }
        return ret;
    }

    private Transform CreateCard(Transform centerPoint, Material baseMat, Texture2D texture, float z, float y)
    {
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        Material newMat = new Material(baseMat);
        newMat.SetTexture("_MainTex", texture);
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        obj.GetComponent<MeshRenderer>().sharedMaterial = newMat;
        obj.transform.localPosition = new Vector3(0, y, z);
        obj.transform.parent = centerPoint;
        return obj.transform;
    }
}