using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node { // Properties and variables of a node object

    public bool traversable;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;


    private int gCost;
    private int hCost;
    private int fCost;
    private Node parent;

    // Constructor of a node to initialize variables
    public Node(bool tempTraversable, Vector3 tempWorldPos, int tempGridX, int tempGridY)
    {
        traversable = tempTraversable;
        worldPos = tempWorldPos;
        gridX = tempGridX;
        gridY = tempGridY;

    }

    public int GetGCost()
    {
        return gCost;
    }

    public void SetGCost(int tempGCost)
    {
        gCost = tempGCost;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public void SetHCost(int tempHCost)
    {
        hCost = tempHCost;
    }

    public int GetFCost(int tempGCost, int tempHCost)
    {
        return tempGCost + tempHCost;
    }

    public Node GetParent()
    {
        return parent;
    }

    public void SetParent(Node tempParent)
    {
        parent = tempParent;
    }
}
