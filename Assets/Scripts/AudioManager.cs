using UnityEngine;
using UnityEngine.UI; // Required for using the Image component

public class AudioController : MonoBehaviour
{   
     [SerializeField] private Slider volumeSlider;
    [SerializeField] private RawImage buttonImage; // Uses RawImage instead of Image
    [SerializeField] private Texture2D muteIcon;   // Accepts raw textures directly
    [SerializeField] private Texture2D unmuteIcon; 

     public static AudioController Instance { get; private set; }
    private float preMuteVolume = 1f;

    

    private bool isMuted = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


     void Start()
    {
        // Set slider limits to match AudioListener (0 to 1)
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;

        // Set the slider to the current game volume on start
        volumeSlider.value = AudioListener.volume;
        
        // Listen for slider changes dynamically
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            preMuteVolume = volumeSlider.value; // Remember volume before muting
            volumeSlider.value = 0f;            // This automatically triggers OnSliderValueChanged
        }
        else
        {
            // Restore volume. If it was 0, default it back to full (1.0)
            volumeSlider.value = preMuteVolume > 0f ? preMuteVolume : 1f;
        }
    }

    // Automatically runs whenever the slider is dragged
    private void OnSliderValueChanged(float value)
    {
        AudioListener.volume = value;

        // Update the button icon based on the volume level
        if (value <= 0f)
        {
            isMuted = true;
            buttonImage.texture = muteIcon;
        }
        else
        {
            isMuted = false;
            buttonImage.texture = unmuteIcon;
        }
    }

}

