using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    MapGeneratorKruskal mapGen;
    List<Node> path = new List<Node>();


    float Heuristic(Node from, Node target)
    {
        float d = Vector3.Distance(from.transform.position, target.transform.position);   // Pythagorean
        // float d = abs(from.i - target.i) + abs(from.j - target.j);    // Manhattan
        // float d = sq(target.i - from.i) + sq(target.j - from.j);  // No sqrt
        return d;
    }

    public void FindPath(Node startNode, Node targetNode, bool visualization = true)
    {
        Node current = null;
        List<Node> openSet = new List<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            int winnerIndex = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].F < openSet[winnerIndex].F)
                {
                    winnerIndex = i;
                }
            }

            current = openSet[winnerIndex];

            if (current == targetNode)
            {
                ReconstructPath(current, visualization);
                return;
            }

            openSet.Remove(current);
            current.IsOpened = false;
            current.IsClosed = true;

            for (int i = 0; i < current.neighbors.Count; i++)
            {
                Node neighbor = current.neighbors[i].GetComponent<Node>();
                if (neighbor.IsClosed == false)
                {
                    float tempG = current.G + Heuristic(neighbor, current);

                    // Is this a better path than before?
                    bool newPath = false;
                    if (neighbor.IsOpened)
                    {
                        if (tempG < neighbor.G)
                        {
                            neighbor.G = tempG;
                            newPath = true;
                        }
                    }
                    else
                    {
                        neighbor.G = tempG;
                        newPath = true;
                        openSet.Add(neighbor);
                        neighbor.IsOpened = true;
                    }

                    // Yes, it's a better path
                    if (newPath)
                    {
                        neighbor.H = Heuristic(neighbor, targetNode);
                        neighbor.F = neighbor.G + neighbor.H;
                        neighbor.previous = current;
                    }
                }
            }
        }
        ReconstructPath(current, visualization);
    }

    void ReconstructPath(Node from, bool visualization = true)
    {
        Node temp = from;
        path.Add(temp);
        while (temp.previous != null)
        {
            path.Add(temp.previous);
            temp = temp.previous;
        }

        if (visualization)
            VisualizePath();
    }

    IEnumerator ReconstructPathCoroutine(Node from)
    {
        Node temp = from;
        path.Add(temp);
        while (temp.previous != null)
        {
            path.Add(temp.previous);
            temp = temp.previous;
        }

        foreach (var spot in path)
        {
            spot.IsVisited = true;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void VisualizePath()
    {
        foreach (var spot in path)
        {
            spot.IsVisited = true;
        }
    }

    public Node GetHint(Node startNode, Node targetNode)
    {
        FindPath(startNode, targetNode, false);
        Node hintNode = path[path.Count - 2];
        return hintNode;
    }
}
