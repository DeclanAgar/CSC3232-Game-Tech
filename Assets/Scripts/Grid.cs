using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public float nodeRadius;
    Node[,] grid;

    [SerializeField]
    Tilemap traversableTiles;
    [SerializeField]
    Tilemap untraversableTiles;
    [SerializeField]
    Tilemap obstacleTiles;
    [SerializeField]
    SpikeController spikeController;
    [SerializeField]
    private float gridOffset;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject player;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    private BoundsInt gridBounds;
    private bool mapChanged;

    private void Start()
    {
        // Calculate the cellbounds of the untraversableTiles tilemap
        untraversableTiles.CompressBounds();
        gridBounds = untraversableTiles.cellBounds;

        nodeDiameter = nodeRadius + nodeRadius;
        gridSizeX = gridBounds.size.x;
        gridSizeY = gridBounds.size.y;
        // Set A* object to be at the centre of the gridbounds
        gameObject.transform.position = gridBounds.center + new Vector3(gridOffset, 0);
        CreateGrid();
    }

    private void Update()
    {
        // If map has been changed, recalculate grid
        if (mapChanged)
        {
            CreateGrid();
            mapChanged = false;
        }
    }

    // Create grid creates a grid of nodes from the tilemap to be used for the navigation of the pathfinding algorithm
    void CreateGrid()
    {
        // Grid of nodes
        grid = new Node[gridSizeX, gridSizeY];
        // Start from bottom left of grid
        Vector3 bottomLeft = new Vector3(gridBounds.xMin + nodeRadius, gridBounds.yMin + nodeRadius, 0);

        // For every tile in tilemap
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // Get worldPoint of tile
                Vector3 worldPoint = bottomLeft + (Vector3.right * x) + Vector3.up * y;
                // If tile is untraversable, or doesn't contain a tile, or has an obstacle, or has a destructable object on it
                if (untraversableTiles.HasTile(new Vector3Int(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y), 0)) || !traversableTiles.HasTile(new Vector3Int(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y), 0)) || (spikeController.GetSpikesActive() && obstacleTiles.HasTile(new Vector3Int(Mathf.RoundToInt(worldPoint.x), Mathf.RoundToInt(worldPoint.y), 0))) || Physics2D.OverlapBox(new Vector2(worldPoint.x, worldPoint.y), new Vector2(nodeRadius, nodeRadius), 0f, layerMask))
                {
                    // Create new node on grid where node is untraversable
                    grid[x, y] = new Node(false, worldPoint, x, y);
                } else
                {
                    // Create new node ong grid where node is traversable
                    grid[x, y] = new Node(true, worldPoint, x, y);
                }
            }
        }
    }

    // GetNodeNeighbours returns a list of all neighbouring nodes of a given node
    // Finding the neighbours of a given node from 'https://www.youtube.com/watch?v=mZfyt03LDH4'
    // A* Pathfinding (E03: algoirthm implementation) by Sebastian Lague
    public List<Node> GetNodeNeighbours(Node node)
    {
        List<Node> nodeNeighbours = new List<Node>();
        // For all adjacent nodes
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // If current node is the node passed through, continue loop
                if ((x == 0 && y == 0))
                {
                    continue;
                }

                // To make sure node is within bounds, if node is in bounds, add to list of node neighbours
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    nodeNeighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return nodeNeighbours;
    }

    // Node from world point converts a given world position into a position on the grid
    // The calculations for converting this is from 'https://www.youtube.com/watch?v=nhiFx28e7JY'
    // A* Pathfinding (E02: node grid) from Sebastian Lague 
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Calculate percentages across the grid
        float percentX = (worldPosition.x + gridSizeX / 2) / gridSizeX;
        float percentY = (worldPosition.y + gridSizeY / 2) / gridSizeY;

        // Get x and y coordinate relative to grid
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public bool IsOnUntraversableTile()
    {
        // If player is on spikes, or is on an untraversable tile return true
        if (player.GetComponent<PlayerCollision>().GetOnSpikes() || untraversableTiles.HasTile(new Vector3Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y), 0)))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public bool GetMapChanged()
    {
        return mapChanged;
    }

    public void SetMapChanged(bool tempMapChanged)
    {
        mapChanged = tempMapChanged;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(gridSizeX, gridSizeY, 0));

    //    if (grid != null)
    //    {
    //        foreach (Node n in grid)
    //        {
    //            Gizmos.color = (n.traversable) ? Color.green : Color.red;
    //            Gizmos.DrawCube(n.worldPos, Vector3.one * .1f);
    //        }
    //    }
    //}
}
