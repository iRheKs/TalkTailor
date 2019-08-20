using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
namespace TextEditor
{
    /// <summary>
    /// Class that represents a connection between two nodes
    /// </summary>
    [Serializable]
    public class Connection
    {
        public Vector2 inPointCenter;
        public Vector2 outPointCenter;
        public NodeInfo inNode, outNode;
        public int answerIndex;
        public Action<Connection> OnClickRemoveConnection;
        /// <summary>
        /// Constructor of the cpnnection
        /// </summary>
        /// <param name="inPoint"> Input connection point to get its center position for this connection to be drawn </param>
        /// <param name="outPoint"> Output connection point to get its center position for this connection to be drawn </param>
        /// <param name="OnClickRemoveConnection"> Action to be called when this connection is clicked to be removed </param>
        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPointCenter = inPoint.rect.center;
            this.outPointCenter = outPoint.rect.center;
            inNode = inPoint.node.info;
            outNode = outPoint.node.info;
            answerIndex = outPoint.index;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }
        /// <summary>
        /// Draw method for the connection line
        /// </summary>
        public void Draw()
        {
            Handles.DrawBezier(
                inPointCenter,
                outPointCenter,
                inPointCenter + Vector2.left * 50f,
                outPointCenter - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((inPointCenter + outPointCenter) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }

        public void UpdatePositions(Vector2 inPosition, Vector2 outPosition)
        {
            inPointCenter = inPosition;
            outPointCenter = outPosition;
        }

        public void UpdateInNode(NodeInfo inNode)
        {
            this.inNode = inNode;
        }
        public void UpdateOutNode(NodeInfo outNode)
        {
            this.outNode = outNode;
        }
    }
}