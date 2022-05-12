#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

public class GizmosObject : MonoBehaviour
{
    #if UNITY_EDITOR
    private Dictionary<string, GizmosCommand> _dictionary = new Dictionary<string, GizmosCommand>();
    #endif

    // Line
    //==============
    public void DrawLine(string id, Vector3 from, Vector3 to)
    {
        #if UNITY_EDITOR
        _dictionary[id] = new GizmosCommand
        {
            start = from,
            end = to,
            line = true,
            name = id
        };
        #endif
    }

    public void DrawLine(string id, Vector3 from, Vector3 to, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            line = true,
            color = color,
            name = id
        });
        #endif
    }

    public void DrawLineH(string id, Vector3 from, Vector3 to)
    {
        #if UNITY_EDITOR
        _dictionary[id] = new GizmosCommand
        {
            start = from,
            end = to,
            line = true,
            name = id, 
            handle = true
        };
        #endif
    }

    public void DrawLineH(string id, Vector3 from, Vector3 to, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            line = true,
            color = color,
            name = id,
            handle = true
        });
        #endif
    }


    // Point
    //==============
    public void DrawPoint(string id, Vector3 point)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = true,
            name = id
        });
        #endif
    }
    
    public void DrawPoint(string id, Vector3 point, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = true,
            color = color,
            name = id
        });
        #endif
    }

    // Vector from null
    //==============
    public void DrawVector(string id, Vector3 point)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = false,
            name = id
        });
        #endif
    }
    
    public void DrawVector(string id, Vector3 point, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = false,
            color = color,
            name = id
        });
        #endif
    }

    public void DrawVectorH(string id, Vector3 point)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = false,
            name = id,
            handle = true
        });
        #endif
    }

    public void DrawVectorH(string id, Vector3 point, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            end = point,
            point = false,
            color = color,
            name = id, 
            handle = true
        });
        #endif
    }

    // Vector 
    //==============
    public void DrawVector(string id, Vector3 from, Vector3 to)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            point = false,
            name = id
        });
        #endif
    }

    public void DrawVector(string id, Vector3 from, Vector3 to, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            point = false,
            color = color,
            name = id
        });
        #endif
    }

    public void DrawVectorH(string id, Vector3 from, Vector3 to)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            point = false,
            name = id,
            handle = true
        });
        #endif
    }

    public void DrawVectorH(string id, Vector3 from, Vector3 to, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = from,
            end = to,
            point = false,
            color = color,
            name = id,
            handle = true
        });
        #endif
    }

    // Direction
    //==============
    public void DrawDirection(string id, Vector3 origin, Vector3 direction)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = origin,
            end = origin + direction,
            point = false,
            name = id
        });
        #endif
    }

    public void DrawDirection(string id, Vector3 origin, Vector3 direction, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = origin,
            end = origin+direction,
            point = false,
            color = color,
            name = id
        });
        #endif
    }

    public void DrawDirectionH(string id, Vector3 origin, Vector3 direction)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = origin,
            end = origin + direction,
            point = false,
            name = id,
            handle = true
        });
        #endif
    }

    public void DrawDirectionH(string id, Vector3 origin, Vector3 direction, Color color)
    {
        #if UNITY_EDITOR
        _dictionary[id] = (new GizmosCommand
        {
            start = origin,
            end = origin + direction,
            point = false,
            color = color,
            name = id, 
            handle = true
        });
        #endif
    }
    #if UNITY_EDITOR
    public const float sizeCoef = .15f;
    private static void DrawVectorGizmos(Vector3 from, Vector3 to, bool handle)
    {
        if (handle)
            Handles.DrawLine(from, to);
        else
            Gizmos.DrawLine(from, to);
        var size = HandleUtility.GetHandleSize(to)*sizeCoef;
        var q = Quaternion.LookRotation((to - from).normalized);
        if (handle)
        {
            Handles.SphereHandleCap(-1, from, Quaternion.identity, size, EventType.Layout);
            Handles.ConeHandleCap(-1, to, q, size, EventType.Layout);
        }
        else
        {
            Gizmos.DrawLine(to - q*((Vector3.forward + Vector3.right).normalized*size), to);
            Gizmos.DrawLine(to - q*((Vector3.forward - Vector3.right).normalized*size), to);
        }
    }

    public void OnDrawGizmos() 
    {
        foreach (var command in _dictionary.Values)
            Inside(command);
    }

    private void Inside(GizmosCommand command)
    {
        var size = HandleUtility.GetHandleSize(command.end) * sizeCoef;
        Gizmos.color = command.color;
        Handles.color = command.color;
        if (command.line)
        {
            if (command.handle)
                Handles.DrawLine(command.start, command.end);
            else
                Gizmos.DrawLine(command.start, command.end);
            var v = (command.start + command.end)/2f;
            DrawLabel(v, command.color, command.name);
        }
        else if (command.point)
        {
            Handles.DotHandleCap(-1, command.end, Camera.current.transform.rotation, size * .12f, EventType.Layout);
            DrawLabel(command.end, command.color, command.name);
        }
        else
        {
            DrawVectorGizmos(command.start, command.end, command.handle);
            var v = (command.start + command.end)/2f;
            DrawLabel(v, command.color, command.name);
        }
    }

    private static void DrawLabel(Vector3 v, Color color, string label)
    {
        var cam = Camera.current;
        var point = cam.WorldToScreenPoint(v);

        var style = new GUIStyle("label") { normal = { textColor = color }, contentOffset = new Vector2(2, -7), fontStyle = FontStyle.Bold };
        CreateIfNull();
        if (point.z > 0 && new Rect(0, 0, cam.pixelWidth, cam.pixelHeight).Contains(point))
        {
            var text = "  " + (label ?? "");
            Handles.Label(v, text, style1);
            Handles.Label(v, text, style2);
            Handles.Label(v, text, style3);
            Handles.Label(v, text, style4);
            Handles.Label(v, text, style);
        }
    }

    static void CreateIfNull()
    {
        if (style1 == null)
            style1 = new GUIStyle("label") { normal = { textColor = Color.black }, contentOffset = new Vector2(1, -8), fontStyle = FontStyle.Bold };
        if (style2 == null)
            style2 = new GUIStyle("label") { normal = { textColor = Color.black }, contentOffset = new Vector2(1, -6), fontStyle = FontStyle.Bold };
        if (style3 == null)
            style3 = new GUIStyle("label") { normal = { textColor = Color.black }, contentOffset = new Vector2(3, -8), fontStyle = FontStyle.Bold };
        if (style4 == null) 
            style4 = new GUIStyle("label") { normal = { textColor = Color.black }, contentOffset = new Vector2(3, -6), fontStyle = FontStyle.Bold };
    }
    private static GUIStyle style1, style2, style3, style4;
    
    private class GizmosCommand
    {
        public Color color = Color.white;
        public string name;
        public Vector3 start;
        public Vector3 end;
        public bool point;
        public bool line;
        public bool handle;
    }
    #endif
}
