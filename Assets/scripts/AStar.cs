using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    MapGeneratorKruskal mapGen;

    GameObject[,] nodes;
    List<Node> openSet = new List<Node>();
    List<Node> closedSet = new List<Node>();
    // List<Node> path = new List<Node>();
    Node current;

    float Heuristic(Node from, Node target)
    {
        float d = Vector3.Distance(from.transform.position, target.transform.position);   // Pythagorean
        // float d = abs(from.i - target.i) + abs(from.j - target.j);    // Manhattan
        // float d = sq(target.i - from.i) + sq(target.j - from.j);  // No sqrt
        return d;
    }

    public void FindPath(Node startNode, Node targetNode)
    {
        openSet.Add(startNode);


        while (openSet.Count > 0)
        {
            int winnerIndex = 0;
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].f < openSet[winnerIndex].f)
                {
                    winnerIndex = i;
                }
            }

            current = openSet[winnerIndex];

            if (current == targetNode)
            {
                ReconstructPath(current);
                // int steps = path.Count - 1;
                return;
            }

            openSet.Remove(current);
            current.isOpened = false;
            current.isClosed = true;

            for (int i = 0; i < current.neighbors.Count; i++)
            {
                Node neighbor = current.neighbors[i].GetComponent<Node>();
                if (neighbor.isClosed == false)
                {
                    float tempG = current.g + Heuristic(neighbor, current);

                    // Is this a better path than before?
                    bool newPath = false;
                    if (neighbor.isOpened)
                    {
                        if (tempG < neighbor.g)
                        {
                            neighbor.g = tempG;
                            newPath = true;
                        }
                    }
                    else
                    {
                        neighbor.g = tempG;
                        newPath = true;
                        openSet.Add(neighbor);
                        neighbor.isOpened = true;
                    }

                    // Yes, it's a better path
                    if (newPath)
                    {
                        neighbor.h = Heuristic(neighbor, targetNode);
                        neighbor.f = neighbor.g + neighbor.h;
                        neighbor.previous = current;
                    }
                }
            }
        }
        ReconstructPath(current);
    }

    void ReconstructPath(Node from)
    {
        List<Node> path = new List<Node>();
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
        }
    }

    IEnumerator ReconstructPathCoroutine(Node from)
    {
        List<Node> path = new List<Node>();
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
}
