using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VideoNode))]
public class VideoNodeEditor : Editor
{
    private void OnEnable()
    {

    }


    override public void OnInspectorGUI()
    {
        var node = target as VideoNode;

        EditorGUILayout.HelpBox("THIS CODE IS IN BETA, AND VIDEO FORMATS MAY NOT WORK ON ALL DEVICES (especially mobile devices)", MessageType.Warning);
        EditorGUILayout.HelpBox("It is recommended to disable the background and foreground when playing videos, as it renders on a camera (not the canvas).  You may also wish to add a WaitNode after this video if you want to nicely fade in a new background/foreground right before the video ends", MessageType.Info);

        if (node.camera_to_use == null)
            EditorGUILayout.HelpBox("Camera.main will be used if no camera is specified in Camera_to_use field", MessageType.Error);
        if (node.wait_until_video_is_finished == false)
            EditorGUILayout.HelpBox("The video will not be automatically disabled when it finishes playing. You may have to disable the VideoPlayer gameobject in the DialogueCanvas manually when you want it to stop rendering. A PerformActionsNode can be used to disable it", MessageType.Info);

        base.OnInspectorGUI();
    }
}
