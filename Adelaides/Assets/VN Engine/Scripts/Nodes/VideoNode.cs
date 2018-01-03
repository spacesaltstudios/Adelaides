using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Video;

// THIS CODE IS IN BETA, AND VIDEO FORMATS MAY NOT WORK ON ALL DEVICES (especially mobile devices)
// It is recommended to disable the background and foreground when playing videos, as it appears behind the canvas
// You may also wish to add a WaitNode after this video if you want to nicely fade in a new background/foreground right before the video ends
public class VideoNode : Node
{
    // Clip to play
    public VideoClip video;
    [Range(0, 1.0f)]
    public float volume = 1.0f;
    [Range(0, 1.0f)]
    public float video_player_transparency = 1.0f;
    public Camera camera_to_use;  // If left empty, will try to get the main camera
    public bool wait_until_video_is_finished = true;
    public bool allow_skipping; // If the player presses Spacebar or clicks on dialogue text, video is stopped and skipped
    public bool looping_video = false;
    public float playback_speed = 1.0f;

    public override void Run_Node()
    {
        // Check to make sure we have a videoplayer assigned in the UIManager
        if (UIManager.ui_manager.video_player == null)
        {
            Debug.LogError("VideoPlayer is not assigned in the UIManager, used in VideoNode", this.gameObject);
            Finish_Node();
            return;
        }
        if (camera_to_use == null && Camera.main == null)
        {
            Debug.LogError("No camera found to render video. Please assign a camera in VideoNode", this.gameObject);
            Finish_Node();
            return;
        }
        if (camera_to_use == null)
            camera_to_use = Camera.main;

        // Stop any previous video if it's playing
        if (UIManager.ui_manager.video_player.isPlaying)
            UIManager.ui_manager.video_player.Stop();

        UIManager.ui_manager.video_player.targetCamera = camera_to_use;
        UIManager.ui_manager.video_player.targetCameraAlpha = video_player_transparency;
        UIManager.ui_manager.video_player.playbackSpeed = playback_speed;
        UIManager.ui_manager.video_player.clip = video;
        UIManager.ui_manager.video_player.isLooping = looping_video;

        if (UIManager.ui_manager.video_player.canSetDirectAudioVolume)
        {
            UIManager.ui_manager.video_player.audioOutputMode = VideoAudioOutputMode.Direct;
            UIManager.ui_manager.video_player.SetDirectAudioVolume(0, volume);
        }
        else
        {
            UIManager.ui_manager.video_player.audioOutputMode = VideoAudioOutputMode.AudioSource;
            UIManager.ui_manager.video_player.SetTargetAudioSource(0, UIManager.ui_manager.video_player.GetComponent<AudioSource>());
        }

        UIManager.ui_manager.video_player.gameObject.SetActive(true);
        UIManager.ui_manager.video_player.Play();

        if (wait_until_video_is_finished)
            StartCoroutine(Wait_Till_Video_Finishes());
        else
            Finish_Node();
    }


    public IEnumerator Wait_Till_Video_Finishes()
    {
        while(UIManager.ui_manager.video_player.isPlaying)
        {
            yield return null;
        }
        StopVideo();
        Finish_Node();
    }


    public override void Button_Pressed()
    {
        if (allow_skipping)
        {
            StopVideo();
            Finish_Node();
        }
    }


    // Call this to disable the video
    public void StopVideo()
    {
        if (UIManager.ui_manager.video_player == null)
            return;

        if (UIManager.ui_manager.video_player.isPlaying)
            UIManager.ui_manager.video_player.Stop();

        UIManager.ui_manager.video_player.gameObject.SetActive(false);
    }


    public override void Finish_Node()
    {
        base.Finish_Node();
    }
}
