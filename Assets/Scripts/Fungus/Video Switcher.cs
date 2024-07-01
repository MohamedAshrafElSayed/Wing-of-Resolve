using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSwitcher : MonoBehaviour
{
    public List<VideoClip> videoClips;
    private VideoPlayer videoPlayer;
    private int currentIndex = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // Check if there are video clips in the list
        if (videoClips.Count > 0)
        {
            // Set the first video clip
            videoPlayer.clip = videoClips[currentIndex];
        }
    }

    // Function to switch to the next video clip
    public void SwitchToNextVideo()
    {
        // Increment the index
        currentIndex++;
        // Check if we reached the end of the list, if so, loop back to the beginning
        if (currentIndex >= videoClips.Count)
        {
            currentIndex = 0;
        }
        // Set the next video clip
        videoPlayer.clip = videoClips[currentIndex];
        // Play the video
        videoPlayer.Play();
    }
}

