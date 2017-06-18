using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{

    private GridManager Instance;

    private bool isToggled = false, autoLink = false;

    void OnEnable()
    {
        Instance = (GridManager)target;
        SceneView.onSceneGUIDelegate -= OnScene;
        SceneView.onSceneGUIDelegate += OnScene;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        isToggled = GUILayout.Toggle(isToggled, "\n\nActivate(Toggle)\n\n", "Button");
        autoLink = GUILayout.Toggle(autoLink, "\n\nAutoLink(Toggle)\n\n", "Button");
        if (GUILayout.Button("Remove") && (Instance.selectedNode1 != null || Instance.selectedNode2 != null))
        {
            if(Instance.selectedNode1 != null)
            {
                foreach(Node node in Instance.selectedNode1.Neighbours)
                {
                    node.Neighbours.Remove(Instance.selectedNode1);
                }
                Instance.worldNodes.Remove(Instance.selectedNode1);
                DestroyImmediate(Instance.selectedNode1.gameObject);
            }
            if (Instance.selectedNode2 != null)
            {
                foreach (Node node in Instance.selectedNode2.Neighbours)
                {
                    node.Neighbours.Remove(Instance.selectedNode2);
                }
                Instance.worldNodes.Remove(Instance.selectedNode2);
                DestroyImmediate(Instance.selectedNode2.gameObject);
            }
        }
        else if (GUILayout.Button("Link") && Instance.selectedNode1 != null && Instance.selectedNode2 != null)
        {
            if (!Instance.selectedNode1.Neighbours.Contains(Instance.selectedNode2))
            {
                Instance.selectedNode1.Neighbours.Add(Instance.selectedNode2);
                Instance.selectedNode2.Neighbours.Add(Instance.selectedNode1);
            }
        }else if(GUILayout.Button("UnLink") && Instance.selectedNode1 != null && Instance.selectedNode2 != null)
        {
            if (Instance.selectedNode1.Neighbours.Contains(Instance.selectedNode2))
            {
                Instance.selectedNode1.Neighbours.Remove(Instance.selectedNode2);
                Instance.selectedNode2.Neighbours.Remove(Instance.selectedNode1);
            }
            
        }

    }


    void OnScene(SceneView sceneview)
    {
        
        if (isToggled)
        {
            
            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.mouseDown && currentEvent.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, Instance.terrainType))
                {
                    Node currentNode = hit.transform.GetComponent<Node>();
                    if (currentNode != null)
                    {
                        if (!currentNode.Selected)
                        {
                            if (Instance.selectedNode1 == null)
                            {
                                Instance.selectedNode1 = currentNode;
                                Instance.latestSelectedNode = Instance.selectedNode1;

                            }
                            else if (Instance.selectedNode2 == null)
                            {
                                Instance.selectedNode2 = currentNode;
                                Instance.latestSelectedNode = Instance.selectedNode2;
                            }
                            else
                            {
                                if (Instance.latestSelectedNode == Instance.selectedNode1)
                                {
                                    Instance.selectedNode2.Selected = false;
                                    Instance.selectedNode2 = currentNode;
                                    Instance.latestSelectedNode = Instance.selectedNode2;
                                }
                                else
                                {
                                    Instance.selectedNode1.Selected = false;
                                    Instance.selectedNode1 = currentNode;
                                    Instance.latestSelectedNode = Instance.selectedNode1;
                                }

                            }
                        }
                        else
                        {

                            if (currentNode == Instance.selectedNode1)
                                Instance.selectedNode1 = null;
                            else
                                Instance.selectedNode2 = null;
                        }
                        currentNode.Selected = !currentNode.Selected;
                    }
                    else
                    {
                        Node newNode = Instantiate(Instance.nodePrefab, hit.point, Quaternion.identity);
                        newNode.transform.parent = Instance.transform;
                        if (autoLink)
                        {
                            foreach (Node nodes in Instance.worldNodes)
                            {
                                if (Vector3.Distance(newNode.transform.position, nodes.transform.position) < Instance.nodesDistance)
                                {
                                    newNode.Neighbours.Add(nodes);
                                    nodes.Neighbours.Add(newNode);
                                }
                            }
                        }
                        Instance.AddNode(newNode);
                    }
                }
            }
        }
    }

}
