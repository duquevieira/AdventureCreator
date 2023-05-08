using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MeadowGames.UINodeConnect4.GraphicRenderer;

namespace MeadowGames.UINodeConnect4
{
    [System.Serializable]
    public class Connection : IGraphElement, ISelectable, IClickable, IDraggable, IHover
    {
        public enum CurveStyle { Spline, Z_Shape, Soft_Z_Shape, Line }

        [SerializeField] string _id = "Connection";
        public string ID
        {
            get => _id;
            set => _id = value;
        }

        public GraphManager graphManager;

        public Port port0; // start
        public Port port1; // end

        public Color selectedColor = new Color(1, 0.58f, 0.04f);
        public Color hoverColor = new Color(1, 0.81f, 0.3f);
        public Color defaultColor = new Color(0.98f, 0.94f, 0.84f);

        public CurveStyle curveStyle = CurveStyle.Soft_Z_Shape;

        public ConnectionLabel label;

        public Line line;

        public Color ElementColor
        {
            get => defaultColor;
            set
            {
                line.color = value;
                defaultColor = value;
            }
        }

        public int Priority => 1;

        [SerializeField] bool _enableDrag = true;
        public bool EnableDrag { get => _enableDrag; set => _enableDrag = value; }
        [SerializeField] bool _enableHover = true;
        public bool EnableHover { get => _enableHover; set => _enableHover = value; }
        [SerializeField] bool _enableSelect = true;
        public bool EnableSelect { get => _enableSelect; set => _enableSelect = value; }
        [SerializeField] bool _disableClick = false;
        public bool DisableClick
        {
            get => _disableClick;
            set => _disableClick = value;
        }

        Connection() { }

        public static Connection NewConnection(Port port0, Port port1)
        {
            GraphManager graphManager = port0.graphManager;

            Connection previousConnectionWithSamePort = Connection.GetConnection(port0, port1);
            if (previousConnectionWithSamePort != null)
            {
                if (graphManager.replaceConnectionByReverse)
                {
                    previousConnectionWithSamePort.Remove();
                }
                else
                {
                    return previousConnectionWithSamePort;
                }
            }

            Connection _connection = graphManager.newConnectionTemplate.Clone();

            if ((port1.Polarity == Port.PolarityType._out && (port0.Polarity == Port.PolarityType._in || port0.Polarity == Port.PolarityType._all))
                || (port1.Polarity == Port.PolarityType._all && port0.Polarity == Port.PolarityType._in))
            {
                _connection.port0 = port1;
                _connection.port1 = port0;
            }
            else
            {
                _connection.port0 = port0;
                _connection.port1 = port1;
            }
            _connection.graphManager = port0.graphManager;

            UICSystemManager.AddConnectionToList(_connection);

            _connection.port0.UpdateIcon();
            _connection.port1.UpdateIcon();

            _connection.UpdateLine();

            _connection.ID = string.Format("Connection ({0} - {1})", port0.node ? port0.node.name : "null", port1.node ? port1.node.name : "null");

            UICSystemManager.UICEvents.TriggerEvent(UICEventType.OnConnectionCreated, _connection);

            return _connection;
        }

        public static Connection GetConnection(Port port0, Port port1)
        {
            foreach (Connection connection in UICSystemManager.Connections)
            {
                if ((port0 == connection.port0 && port1 == connection.port1) ||
                        (port0 == connection.port1 && port1 == connection.port0))
                    return connection;
            }
            return null;
        }

        public void OnPointerDown()
        {
            if (!UICSystemManager.selectedElements.Contains(this))
            {
                Select();
            }
            else
            {
                Unselect();
            }
        }

        public void OnPointerUp()
        {
            _dragStart = true;
        }

        public void Remove()
        {
            Unselect();

            UICSystemManager.RemoveConnectionFromList(this);

            port0.UpdateIcon();
            port1.UpdateIcon();

            if (label)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                    GameObject.Destroy(label.gameObject);
#if UNITY_EDITOR
                else
                {
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        if (label) GameObject.DestroyImmediate(label.gameObject);
                    };
                }
#endif
            }

            if (UICSystemManager.clickedElement == this as IElement)
                UICSystemManager.clickedElement = null;

            UICSystemManager.UICEvents.TriggerEvent(UICEventType.OnConnectionRemoved, this);
        }

        public void Select()
        {
            if (EnableSelect)
            {
                line.color = selectedColor;
                if (!UICSystemManager.selectedElements.Contains(this))
                {
                    UICSystemManager.selectedElements.Add(this);
                    UICSystemManager.UICEvents.TriggerEvent(UICEventType.OnElementSelected, this);
                }
            }
        }

        public void Unselect()
        {
            if (EnableSelect)
            {
                line.color = defaultColor;
                if (UICSystemManager.selectedElements.Contains(this))
                {
                    UICSystemManager.selectedElements.Remove(this);
                    UICSystemManager.UICEvents.TriggerEvent(UICEventType.OnElementUnselected, this);
                }
            }
        }

        public void UpdateLine()
        {
            if (graphManager)
            {
                Vector3[] linePoints = UICUtility.WorldToScreenPointsForRenderMode(graphManager, new Vector3[] {
                    port0.transform.position,
                    port0.controlPoint.Position,
                    port1.controlPoint.Position,
                    port1.transform.position });

                Vector2[] newPoints = LineUtils.ConvertLinePointsToCurve(linePoints, curveStyle);

                line.SetPoints(newPoints);

                if (label)
                {
                    label.UpdateLabel(line);
                }
            }
        }

        public List<Connection> GetCrossedConnections()
        {
            List<Connection> crossedConnections = new List<Connection>();

            foreach (Connection conn in UICSystemManager.Connections)
            {
                if (UICUtility.DoConnectionsIntersect(conn, this))
                    if (!(conn.port0 == port0 || conn.port1 == port1 || conn.port0 == port1 || conn.port1 == port0))
                        crossedConnections.Add(conn);
            }

            return crossedConnections;
        }

        bool _dragStart = true;
        Port otherPort;
        public void OnDrag()
        {
            if (EnableDrag)
            {
                if (_dragStart)
                {
                    _dragStart = false;

                    // check closest port from pointer
                    Port clossestPort = port0;
                    otherPort = port1;
                    float distance = Vector3.Distance(port0.transform.position, InputManager.Instance.GetCanvasPointerPosition(graphManager));
                    if (distance > Vector3.Distance(port1.transform.position, InputManager.Instance.GetCanvasPointerPosition(graphManager)))
                    {
                        clossestPort = port1;
                        otherPort = port0;
                    }

                    UICSystemManager.clickedElement = otherPort;
                    otherPort.OnPointerDown();
                    otherPort.OnDrag();
                    Remove();
                }
            }
        }

        public void OnPointerHoverEnter()
        {
            if (EnableHover)
            {
                line.color = hoverColor;
            }
        }
        public void OnPointerHoverExit()
        {
            if (EnableHover)
            {
                if (UICSystemManager.selectedElements.Contains(this))
                    line.color = selectedColor;
                else
                    line.color = defaultColor;
            }
        }

        public void SetLabel(string value)
        {
            if (label == null)
            {
                ConnectionLabel connectionLabel = GameObject.Instantiate(graphManager.connectionLabelTemplate, graphManager.lineRenderer.transform);
                connectionLabel.SetGraphManager(graphManager);
                label = connectionLabel;
            }

            label.text = value;
        }

        public void RemoveLabel()
        {
            if (label != null)
            {
                GameObject.Destroy(label.gameObject);
                label = null;
            }
        }

        public Connection Clone()
        {
            return UICUtility.Clone(this);
        }

        public Connection CopyVariables()
        {
            return UICUtility.Clone(this);
        }
    }
}
