using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    MapGeneratorKruskal mapGen;

    // let nodes = new Array(arrSize);
    // let savedNodes;
    // let openSet = [];
    // // let closedSet = [];
    // let path = [];
    // let startNode = null;
    // let targetNode = null;
    // let current = null;

    // void Start()
    // {
    //     mapGen = GameObject.Find("MapGenerator").GetComponent<MapGeneratorKruskal>();

    //     openSet.push(startNode);
    // }

    // float Heuristic(Node from, Node target)
    // {
    //     float d = 0f;
    //     if (currentHeur == 1)
    //     {
    //         d = abs(from.i - target.i) + abs(from.j - target.j);
    //         // d += 1;
    //     }
    //     else if (currentHeur == 2)
    //         d = dist(from.i, from.j, target.i, target.j);
    //     else
    //     {
    //         d = sq(target.i - from.i) + sq(target.j - from.j);
    //         // d += 10;
    //     }
    //     // console.log( currentHeur + ", " + from.i + " " + from.j + " -> " + target.i + " " + target.j + ", dist: " + d );
    //     return d;
    // }

    // void FindPath(Node startNode, Node targetNode)
    // {
    //     //while (openSet.length > 0)
    //     if (openSet.length > 0)
    //     {
    //         int winnerInd = 0;
    //         for (int i = 0; i < openSet.length; i++)
    //         {
    //             if (openSet[i].f < openSet[winnerInd].f)
    //             {
    //                 winnerInd = i;
    //                 //console.log(i);
    //             }
    //         }

    //         current = openSet[winnerInd];

    //         if (current == targetNode)
    //         {
    //             ReconstructPath(current);
    //             int steps = path.length - 1;
    //         }

    //         removeFromArray(openSet, current);
    //         // closedSet.push( current );
    //         current.isOpened = false;
    //         current.isClosed = true;


    //         for (int i = 0; i < current.neighbors.length; i++)
    //         {
    //             let neighbor = current.neighbors[i];
    //             // if ( !closedSet.includes( neighbor ) && neighbor.isWall == false ) {
    //             if (neighbor.isClosed == false && neighbor.isWall == false)
    //             {

    //                 let tempG = current.g + Heuristic(neighbor, current);

    //                 // Is this a better path than before?
    //                 let newPath = false;
    //                 // if ( openSet.includes( neighbor ) ) {
    //                 if (neighbor.isOpened)
    //                 {
    //                     if (tempG < neighbor.g)
    //                     {
    //                         neighbor.g = tempG;
    //                         newPath = true;
    //                     }
    //                 }
    //                 else
    //                 {
    //                     neighbor.g = tempG;
    //                     newPath = true;
    //                     openSet.push(neighbor);
    //                     neighbor.isOpened = true;
    //                 }

    //                 // Yes, it's a better path
    //                 if (newPath)
    //                 {
    //                     neighbor.h = Heuristic(neighbor, targetNode);
    //                     neighbor.f = neighbor.g + neighbor.h;
    //                     neighbor.previous = current;
    //                 }
    //             }
    //         }
    //     }
    //     else
    //     {
    //         console.log("NO SOLUTION");
    //         isNoLoop = true;
    //         noLoop();
    //         // return;
    //     }
    //     ReconstructPath(current);
    // }

    // void ReconstructPath(Node from)
    // {
    //     path = [];
    //     let temp = from;
    //     path.push(temp);
    //     while (temp.previous != null)
    //     {
    //         path.push(temp.previous);
    //         temp = temp.previous;
    //     }

    //     noFill();
    //     stroke(0, 0, 255);
    //     strokeWeight(4);
    //     beginShape();
    //     for (let i = 0; i < path.length; i++)
    //     {
    //         vertex(path[i].i * wid + wid / 2, path[i].j * wid + wid / 2);
    //     }
    //     endShape();
    //     noStroke();
    // }
}
