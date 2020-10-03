using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(PlatformController))]
public class PlatformHandle : Editor {

    private PlatformController platformController;
    
    void OnEnable() {
        platformController = target as PlatformController;
    }

    void OnSceneGUI() {
        // Don't draw while in play mode, it'd be misleading.
        if (EditorApplication.isPlaying) {
            return;
        }

        Vector3 pos = platformController.transform.position;
        
        // Draw lines between waypoints.
        List<Vector3> polylinePoints = platformController.waypoints.ConvertAll(item => (Vector3)item + pos);
        polylinePoints.Insert(0, pos);
        if (platformController.isCyclical) {
            // If waypoints are set to be cyclical, add the starting point to the polylinePoints to complete loop line.
            polylinePoints.Add(polylinePoints[0]);
            
        }
        Handles.DrawPolyLine(polylinePoints.ToArray());

        // Integrate waypoints with positional handles.
        for(int i = 0; i < platformController.waypoints.Count; i++) {
            platformController.waypoints[i] = Handles.PositionHandle(platformController.waypoints[i] + (Vector2)pos, Quaternion.identity) - pos;
        }
    }
}
