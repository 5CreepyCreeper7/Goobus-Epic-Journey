using UnityEngine;
using TMPro;

public class CurrentlyPlaying : MonoBehaviour
{
    private TextMeshProUGUI currentlyPlayingText;

    private void Awake() {
        currentlyPlayingText = GetComponent<TextMeshProUGUI>();
    }

    public void SetPlaying(bool isPlaying) {
        currentlyPlayingText.color = isPlaying ? Color.yellow : Color.white;
    }
 
 }
