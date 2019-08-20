using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace TextEditor
{
    /// <summary>
    /// Class that represents dialogue in TalkTailorWindow
    /// </summary>
    [System.Serializable]
    public class DialogueEditor
    {
        private Vector2 offset;
        private Vector2 drag;

        public Vector2 Drag
        {
            get { return drag; }
            set { drag = value; }
        }
        [SerializeField]
        private List<Node> nodes;
        [SerializeField]
        private List<Connection> connections;
        public Dialogue dialogue;
        
        private GUIStyle inPointStyle;
        private GUIStyle outPointStyle;

        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Rect areaRect;

        private Action<Node> OnNodeSelected;
        private Action<DialogueEditor> HideInspector;

        private int nodeSelected = -1;

        Texture2D tex, tex2;
        /// <summary>
        /// Initialization of the dialogue editor
        /// </summary>
        /// <param name="sectionArea"> Area occupied by this editor </param>
        /// <param name="OnSelectedNode"> Action to be called when a node is selected </param>
        /// <param name="OnClickedArea"> Action to be called when area of this editor is clicked </param>
        public void Initialize(Rect sectionArea, Action<Node> OnSelectedNode, Action<DialogueEditor> OnClickedArea)
        {
            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D;
            inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn on.png") as Texture2D;

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn.png") as Texture2D;
            outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn on.png") as Texture2D;

            areaRect = sectionArea;
            OnNodeSelected = OnSelectedNode;
            HideInspector = OnClickedArea;
            ReloadNodes();
            RefreshConnectionsNodes();
            UpdateConnections();
        }
        /// <summary>
        /// Reloads all nodes when Unity is open
        /// </summary>
        private void ReloadNodes()
        {
            if (nodes != null)
            { 
                foreach (Node item in nodes)
                {
                    item.SetUp(inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                }
            }
        }
        /// <summary>
        /// Draws dialogue area
        /// </summary>
        /// <param name="sectionArea"> Section area to be drawn into </param>
        public void DrawDialogue(Rect sectionArea)
        {
            areaRect = sectionArea;
            GUILayout.BeginArea(areaRect);
            {
                DrawGrid(20, 0.2f, Color.gray);
                DrawGrid(100, 0.4f, Color.gray);
                DrawNodes();
                UpdateConnections();
                DrawConnections();
                DrawConnectionLine(Event.current);
            }
            GUILayout.EndArea();
        }
        /// <summary>
        /// Draws the grid of the area
        /// </summary>
        /// <param name="gridSpacing"> Spacing of the lines of the grid </param>
        /// <param name="gridOpacity"> Opacity of the lines of the grid </param>
        /// <param name="gridColor"> Color of the lines of the grid </param>
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(areaRect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(areaRect.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, areaRect.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(areaRect.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }
        /// <summary>
        /// Draws nodes inside the dialogue area
        /// </summary>
        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw(1);
                }
            }
        }
        /// <summary>
        /// Draws all connections between nodes
        /// </summary>
        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }
        /// <summary>
        /// Draws the line that represents the new connection that will be added
        /// </summary>
        /// <param name="e"> Unity window event </param>
        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }
        /// <summary>
        /// Process node events
        /// </summary>
        /// <param name="e"> Unity window event </param>
        public void ProcessNodeEvents(Event e)
        {
            nodeSelected = -1;
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);
                    if (nodes[i].isSelected || nodes[i].isDragged)
                    {
                        nodeSelected = i;
                    }
                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && areaRect.Contains(e.mousePosition))
                    {
                        if (nodeSelected == -1)
                            OnAreaClicked();
                        else
                            OnSelected();
                    }
                    break;
            }
        }
        /// <summary>
        /// Action callback called when area is clicked
        /// </summary>
        private void OnAreaClicked()
        {
            ClearConnectionSelection();

            if (HideInspector != null)
            {
                HideInspector(this);
            }
        }
        /// <summary>
        /// Action callback called when node is selected
        /// </summary>
        private void OnSelected()
        {
            if (OnNodeSelected != null)
            {
                OnNodeSelected(nodes[nodeSelected]);
            }
        }
        /// <summary>
        /// Method called when dialogue section is dragged
        /// </summary>
        /// <param name="delta"> Delta mouse position where dialogue will be dragged </param>
        public void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }
            GUI.changed = true;
        }
        /// <summary>
        /// Called when clicked context menu to add new node
        /// </summary>
        /// <param name="mousePosition"> Mose position where context menu appeared </param>
        public void OnClickAddNode(Vector2 mousePosition)
        {
            if (nodes == null)
            {
                nodes = new List<Node>();
            }

            nodes.Add(new Node(mousePosition, 200, 55, areaRect.position, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
            nodes[nodes.Count - 1].info.nodeName = "Node";
            nodes[nodes.Count - 1].info.ID = DLLWrapper.AddNode(nodes[nodes.Count - 1].info.nodeName);
            dialogue.AddNode(nodes[nodes.Count - 1].info);
        }
        /// <summary>
        /// Called when connection will be removed
        /// </summary>
        /// <param name="connection"> Connection that will be removed </param>
        private void OnClickRemoveConnection(Connection connection)
        {
            connection.outNode.answers[connection.answerIndex].connection = -1;
            DLLWrapper.DeleteConnection(connection.outNode.ID, connection.answerIndex);
            connections.Remove(connection);
        }
        /// <summary>
        /// Creates a new connection. Adds it to the list
        /// </summary>
        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            int i;
            for (i = 0; (i < connections.Count) && (connections[i].inPointCenter != selectedInPoint.rect.center && connections[i].outPointCenter != selectedOutPoint.rect.center); i++);

            if (connections.Count > 0 && i < connections.Count)
            {
                Connection toRemove = connections[i];
                connections.Remove(toRemove);
            }

            selectedInPoint.conected = true;
            selectedOutPoint.conected = true;
            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

            DLLWrapper.ChangeConnection(selectedOutPoint.node.info.ID, selectedOutPoint.index, selectedInPoint.node.info.ID);
        }
        /// <summary>
        /// Clears the connection selection. Called when clicked outside the connection point when a connection is being created
        /// </summary>
        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }
        /// <summary>
        /// Called when a node is removed
        /// </summary>
        /// <param name="node"> Node to be removed </param>
        private void OnClickRemoveNode(Node node)
        {
            GUI.changed = true;
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (node.inPoint.rect.Contains(connections[i].inPointCenter))
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                    else
                    {
                        for (int j = 0; j < node.info.answerNumber; j++)
                        {
                            if (node.outPoint[j].rect.Contains(connections[i].outPointCenter))
                            {
                                connectionsToRemove.Add(connections[i]);
                            }
                        }
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }
            DLLWrapper.DeleteNode(node.info.ID);
            dialogue.RemoveNode(node.info);
            nodes.Remove(node);
        }
        /// <summary>
        /// Called when a input connection point is clicked 
        /// </summary>
        /// <param name="inPoint"> Input connection point clicked </param>
        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }
        /// <summary>
        /// Called when a output connection point is clicked
        /// </summary>
        /// <param name="outPoint"> Output connection point clicked </param>
        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void RefreshConnectionsNodes()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        if (nodes[j].inPoint.rect.Contains(connections[i].inPointCenter))
                        {
                            connections[i].UpdateInNode(nodes[j].info);
                        }
                        else if (connections[i].answerIndex < nodes[j].outPoint.Count && nodes[j].outPoint[connections[i].answerIndex].rect.Contains(connections[i].outPointCenter) )
                        {
                            connections[i].UpdateOutNode(nodes[j].info);
                        }
                    }
                }
                GUI.changed = true;
            }
        }
        private void UpdateConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        if (nodes[j].info.ID == connections[i].inNode.ID)
                        {
                            connections[i].UpdatePositions(nodes[j].inPoint.rect.center, connections[i].outPointCenter);
                        }
                        else if (connections[i].answerIndex < nodes[j].outPoint.Count && nodes[j].info.ID == connections[i].outNode.ID)
                        {
                            connections[i].UpdatePositions(connections[i].inPointCenter, nodes[j].outPoint[connections[i].answerIndex].rect.center);
                        }
                    }
                }
                GUI.changed = true;
            }
        }
    }
}