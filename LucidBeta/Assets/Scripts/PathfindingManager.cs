using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager instance;

    public List<Node> navNodes = new List<Node>();
    void Start()
    {
        instance = this;

        float range = 6;
        float step = 0.2f;
        float _x = 0;
        float _y = 0;

        LayerMask mask = 1 << 2;

        //Spawn Nodes
        for (_x = -range; _x < range; _x += step)
        {
            for (_y = -range; _y < range; _y += step)
            {
                Vector3 pos = new Vector3(_x, _y, 0);
                //Make sure node is not in wall
                Collider2D[] hit = Physics2D.OverlapPointAll(pos, mask);
                bool inWall = false;

                foreach (Collider2D h in hit)
                {
                    if (!h.isTrigger)
                    {
                        inWall = true;
                        break;
                    }
                }

                if (!inWall)
                {
                    Node n = new Node(pos);
                    navNodes.Add(n);
                }
            }
        }

        //Link Nodes
        float linkDist = step * 1.5f;
        foreach (Node n in navNodes)
        {
            foreach (Node ne in navNodes)
            {
                float curDist = Vector3.Distance(ne.position, n.position);
                //Distance Check
                if (curDist <= linkDist && curDist != 0 && ne != n)
                {
                    //LOS check
                    if (!Physics2D.Raycast(n.position, (ne.position - n.position), Vector3.Distance(ne.position, n.position), mask))
                    {
                        n.neighbours.Add(ne);
                    }
                }
            }
        }
    }
    public List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        int ebreak = 0;

        List<Vector3> path = new List<Vector3>();

        List<Node> open = new List<Node>();
        List<Node> visited = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> score = new Dictionary<Node, float>();

        Dictionary<Node, Node> nextNode = new Dictionary<Node, Node>();

        Node startNode = null;
        Node endNode = null;

        //Get nearby start and end nodes
        float shortestDist = Mathf.Infinity;
        foreach (Node n in navNodes)
        {
            float d = Vector3.Distance(n.position, start);
            if (d < shortestDist)
            {
                shortestDist = d;
                startNode = n;
            }
        }
        shortestDist = Mathf.Infinity;
        foreach (Node n in navNodes)
        {
            float d = Vector3.Distance(n.position, end);
            if (d < shortestDist)
            {
                shortestDist = d;
                endNode = n;
            }
        }

        //TRASH* Pathfinding
        /*
        open.Add(endNode);

        while (open.Count > 0)
        {
            Node n = open[0];

            open.Remove(n);

            visited.Add(n);

            if (n == startNode)
            {
                //Construct path
                while (nextNode.ContainsKey(n))
                {
                    path.Add(n);
                    n = nextNode[n];
                }

                path.Add(endNode);

                return path;
            }

            foreach (Node ne in n.neighbours)
            {
                if (!visited.Contains(ne) && !open.Contains(ne))
                {
                    nextNode[ne] = n;
                    open.Add(ne);
                }
            }
        }
        */

        //A* Pathfinding

        open.Add(startNode);

        while (open.Count > 0)
        {
            Node n = open[0];

            if (n == endNode)
            {
                path.Add(n.position);

                while (previous.ContainsKey(n))
                {
                    n = previous[n];
                    path.Insert(0, n.position);

                    if (n == startNode)
                        break;

                    if (ebreak++ >= 10000)
                        break;
                }
                return path;
            }

            open.Remove(n);
            foreach (Node neighbour in n.neighbours)
            {

                float dScore = (score.ContainsKey(n) ? score[n] : 0) + Vector3.Distance(neighbour.position, n.position);

                if (!score.ContainsKey(neighbour) || dScore < score[neighbour])
                {
                    previous[neighbour] = n;
                    score[neighbour] = dScore;
                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }

                if (ebreak++ >= 10000)
                    break;

            }

            if (ebreak++ >= 10000)
                break;

        }


        return path;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Node n in navNodes)
        {
            Gizmos.DrawSphere(n.position, 0.1f);

            foreach (Node nn in n.neighbours)
            {
                Gizmos.DrawLine(n.position, nn.position);
            }
        }
    }
}
public class Node
{
    public Vector3 position = Vector3.zero;
    public List<Node> neighbours = new List<Node>();

    public Node(Vector3 pos)
    {
        position = pos;
    }
}
