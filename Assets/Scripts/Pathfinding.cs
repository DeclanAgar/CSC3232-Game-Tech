using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    Grid grid;

    private int openListNodeGCost;
    private int openListNodeHCost;

    private int currentNodeGCost;
    private int currentNodeHCost;
    private int currentNodeFCost;

    private int neighbourNodeG;
    private int neighbourNodeH;
    private int neighbourNodeF;
    private bool inopenList;

    private List<Node> path;

    // FindPath is an implementation of an A* algorithm to find the best path between a start and destination position.
    public void FindPath(Vector3 startPos, Vector3 destPos)
    {

        startPos = startPos - grid.transform.position;
        destPos = destPos - grid.transform.position + new Vector3(0, 1, 0);

        // Convert transform position into a node on the grid
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node destNode = grid.NodeFromWorldPoint(destPos);

        // If a node position has not been found, break early out of method
        if (startNode == null || destNode == null || grid.IsOnUntraversableTile())
        {
            path.Clear();
            return;
        }
        startNode.SetParent(null); // Root node does not have a parent
        startNode.SetGCost(0); 
        startNode.SetHCost(10);

        // Open and Closed lists of nodes
        List<Node> openList = new List<Node>(); // The list of nodes to be evaluated
        List<Node> closedList = new List<Node>(); // The list of nodes already evaluated

        // Add starting node to the open list
        openList.Add(startNode);

        // Loop as long as we have nodes to check
        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            // Find best node in the open list
            for (int i = 1; i < openList.Count; i++)
            {
                openListNodeGCost = openList[i].GetGCost();
                openListNodeHCost = openList[i].GetHCost();
                currentNodeGCost = currentNode.GetGCost();
                currentNodeHCost = currentNode.GetHCost();
                currentNodeFCost = currentNodeGCost + currentNodeHCost;

                // If f cost of node in open list better than currentNode f cost, set openlist node as current node
                if(openList[i].GetFCost(openListNodeGCost, openListNodeHCost) < currentNode.GetFCost(currentNodeGCost, currentNodeHCost)/* || openList[i].fCost == currentNode.fCost*/)
                {
                        currentNode = openList[i];
                }
            }
            // Remove currentNode from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == destNode) // Path has been found!
            {
                path = new List<Node>();
                while (currentNode != startNode) // Add parent nodes to the path list
                {
                    path.Add(currentNode);
                    currentNode = currentNode.GetParent();
                }
                path.Reverse(); // Reverse list originate from the 'seeker'
                return;
            }
            // Expand node to check its neighbours
            foreach (Node neighbourNode in grid.GetNodeNeighbours(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) // Node has already been evaluated
                    {
                        continue;
                    }
                    if (!neighbourNode.traversable) // Node doesn't need to be evaluated
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }
                    // Calculate G, H and F costs of the neighbours
                    neighbourNodeG = currentNode.GetFCost(currentNodeGCost,currentNodeHCost) + GetDistance(currentNode, neighbourNode);
                    neighbourNodeH = neighbourNode.GetHCost();
                    neighbourNodeF = neighbourNode.GetFCost(neighbourNodeG, neighbourNodeH);
                    inopenList = openList.Contains(neighbourNode);
                    
                    if (neighbourNodeF < currentNodeFCost || !inopenList) // Better path or newly seen node found
                    {
                        if (!inopenList) // Add newly seen node to open list
                        {
                            neighbourNode.SetHCost(neighbourNodeH);
                            openList.Add(neighbourNode);
                        }
                        neighbourNode.SetGCost(neighbourNodeG);
                        neighbourNode.SetHCost(neighbourNodeH);
                        neighbourNode.SetParent(currentNode);
                    }
                }
            // Done with node, remove from open list and add to closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);
        }
    }

    // Get Distance calculates the distance between two nodes on the grid
    // To calculate the distance between two neighbour nodes on the grid, the concept
    // to do this has been retrieved from 'https://youtu.be/mZfyt03LDH4' 'A* Pathfinding
    // (E03: algorithm implementation) by Sebastian Lague
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // 14 is a rough calculation of the distance between diagonal nodes (Squareroot of 10 + 10)
        if (distanceX > distanceY) {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        } else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }

    #region Getters and Setters
    public List<Node> GetPath()
    {
        return path;
    }

    public void SetPath(List<Node> tempPath)
    {
        path = tempPath;
    }
    #endregion

    //private void OnDrawGizmos()
    //{
    //    if (path != null)
    //    {
    //        for (int i = 0; i < path.Count - 1; i++)
    //        {
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawLine(new Vector2(path[i].worldPos.x, path[i].worldPos.y), (new Vector2(path[i+1].worldPos.x , path[i+1].worldPos.y)));
    //        }
    //    }
    //}
}
