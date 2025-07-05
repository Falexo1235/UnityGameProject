using UnityEngine;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    [Header("Shader Settings")]
    public Material targetMaterial;
    public string pixelHeightPropertyName = "_PixelHeight";

    [Header("UI Elements")]
    public UnityEngine.UI.Slider pixelizationSlider;
    public TextMeshProUGUI pixelizationValueText;

    [Header("Settings")]
    public float minPixelHeight = 50f;
    public float maxPixelHeight = 1920f;
    public float defaultPixelHeight = 1920f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetupSlider();
    }

    private void SetupSlider()
    {
        if (pixelizationSlider != null)
        {
            pixelizationSlider.minValue = 0f;
            pixelizationSlider.maxValue = 100f;
            pixelizationSlider.value = GetPixelizationPercentage();
            pixelizationSlider.onValueChanged.AddListener(OnPixelizationChanged);
        }

        UpdateValueText();
    }

    public void OnPixelizationChanged(float percentage)
    {
        float pixelValue = Mathf.Lerp(maxPixelHeight, minPixelHeight, percentage / 100f);
        SetPixelHeight(pixelValue);
        UpdateValueText();
        SaveSettings();
    }

    private void UpdateValueText()
    {
        if (pixelizationValueText != null)
        {
            float percentage = GetPixelizationPercentage();
            pixelizationValueText.text = $"{percentage:F0}%";
        }
    }

    public float GetPixelizationPercentage()
    {
        float currentPixelHeight = GetPixelHeight();
        return Mathf.Lerp(0f, 100f, (maxPixelHeight - currentPixelHeight) / (maxPixelHeight - minPixelHeight));
    }

    public float GetPixelHeight()
    {
        if (targetMaterial != null)
        {
            return targetMaterial.GetFloat(pixelHeightPropertyName);
        }
        return defaultPixelHeight;
    }

    public void SetPixelHeight(float value)
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetFloat(pixelHeightPropertyName, value);
        }
    }

    private void LoadSettings()
    {
        float savedValue = PlayerPrefs.GetFloat("PixelHeight", defaultPixelHeight);
        SetPixelHeight(savedValue);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("PixelHeight", GetPixelHeight());
        PlayerPrefs.Save();
    }

    public void ResetToDefaults()
    {
        SetPixelHeight(defaultPixelHeight);
        if (pixelizationSlider != null)
        {
            pixelizationSlider.value = GetPixelizationPercentage();
        }
        UpdateValueText();
        SaveSettings();
    }
}