using UnityEngine;

/// <summary>
/// 18.9kHz 이상의 고주파 대역에서 에너지가 감지되면 디버그 출력하는 클래스
/// </summary>
public class SpectrumVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public int spectrumSize = 512;
    public float updateInterval = 0.1f;
    public float frequencyThreshold = 15000f; // 기준 주파수 (Hz)
    public float energyThreshold = 0.01f;     // 감지 임계값

    private float[] spectrum;
    private float nextUpdateTime = 0f;

    void Start()
    {
        spectrum = new float[spectrumSize];
    }

    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            AnalyzeHighFrequencies();
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    /// <summary>
    /// 18.9kHz 이상 주파수 대역에서 값이 임계값을 넘으면 출력
    /// </summary>
    void AnalyzeHighFrequencies()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float nyquist = AudioSettings.outputSampleRate / 2f;
        float freqPerBin = nyquist / spectrumSize;

        int startIndex = Mathf.FloorToInt(frequencyThreshold / freqPerBin);

        for (int i = startIndex; i < spectrumSize; i++)
        {
            float freq = i * freqPerBin;
            float energy = spectrum[i];

            if (energy > energyThreshold)
            {
                Debug.Log($"고주파 감지됨! Index: {i}, Freq: {freq:F0}Hz, Energy: {energy:F4}");
            }
        }
    }
}
