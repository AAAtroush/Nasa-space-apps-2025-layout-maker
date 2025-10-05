using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ApiResultUI : MonoBehaviour
{
    [TextArea(10, 30)]
    public string apiOutput;

    public Transform contentParent;
    public Button nextButton;
    public Button previousButton;
    public TextMeshProUGUI pageIndicator;
    public ApiIntegration apiIntegration;
    public GameObject things;
    
    private GameObject sectionPrefab;
    private List<GameObject> sections = new List<GameObject>();
    private int currentSectionIndex = 0;

    void Awake()
    {
        // Load resources and set up components in Awake
        sectionPrefab = Resources.Load<GameObject>("SectionUI");
        
        if (sectionPrefab == null)
        {
            Debug.LogError("SectionUI prefab not found in Resources!");
            return;
        }

        // Set up button listeners
        if (nextButton != null)
            nextButton.onClick.AddListener(NextSection);
        
        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousSection);
    }

    void Start()
    {
        // In Start, check for API response and build UI
        if (apiIntegration != null && !string.IsNullOrEmpty(apiIntegration.response))
        {
            apiOutput = apiIntegration.response;
            BuildUI(apiOutput);
            things.SetActive(true);
        }
        else if (!string.IsNullOrEmpty(apiOutput))
        {
            // Use the inspector field as fallback
            BuildUI(apiOutput);
        }
        else
        {
            Debug.LogWarning("No API response or apiOutput text available to display.");
        }
    }

    // Add a method to update UI when API response is ready
    public void UpdateUIFromAPI(string apiResponse)
    {
        apiOutput = apiResponse;
        BuildUI(apiOutput);
    }

    void BuildUI(string rawText)
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        sections.Clear();

        Dictionary<string, string> parsedSections = ParseSections(rawText);
        
        foreach (var kvp in parsedSections)
        {
            GameObject newSection = Instantiate(sectionPrefab, contentParent);
            TextMeshProUGUI[] texts = newSection.GetComponentsInChildren<TextMeshProUGUI>();

            if (texts.Length >= 2)
            {
                // Check if it's a score section (---number---)
                if (kvp.Key.StartsWith("---") && kvp.Key.EndsWith("---"))
                {
                    // Extract just the number from ---2275---
                    string justNumber = kvp.Key.Substring(3, kvp.Key.Length - 6);
                    texts[0].text = $"<b><size=140%>Score: {justNumber}</size></b>";
                    texts[1].text = "";
                    texts[0].color = Color.yellow;
                    texts[0].alignment = TextAlignmentOptions.Center;
                }
                else
                {
                    texts[0].text = $"<b><size=120%>{kvp.Key}</size></b>";
                    texts[1].text = kvp.Value.Trim();
                }
            }
            
            sections.Add(newSection);
            newSection.SetActive(false);
        }

        if (sections.Count > 0)
        {
            sections[0].SetActive(true);
            currentSectionIndex = 0;
            UpdateNavigationUI();
        }
    }

    void NextSection()
    {
        if (sections.Count == 0) return;

        sections[currentSectionIndex].SetActive(false);
        currentSectionIndex = (currentSectionIndex + 1) % sections.Count;
        sections[currentSectionIndex].SetActive(true);
        UpdateNavigationUI();
    }

    void PreviousSection()
    {
        if (sections.Count == 0) return;

        sections[currentSectionIndex].SetActive(false);
        currentSectionIndex--;
        if (currentSectionIndex < 0)
            currentSectionIndex = sections.Count - 1;
        sections[currentSectionIndex].SetActive(true);
        UpdateNavigationUI();
    }

    void UpdateNavigationUI()
    {
        if (pageIndicator != null)
        {
            pageIndicator.text = $"{currentSectionIndex + 1} / {sections.Count}";
        }
    }

    Dictionary<string, string> ParseSections(string text)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        
        // No predefined headers - detect sections by content patterns
        List<string> lines = new List<string>(text.Split('\n'));
        System.Text.StringBuilder body = new System.Text.StringBuilder();
        string currentHeader = "";

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            
            // Detect section headers:
            // 1. Lines wrapped in --- (scores)
            // 2. Lines ending with colon
            // 3. Lines with *** patterns
            if ((trimmed.StartsWith("---") && trimmed.EndsWith("---")) ||
                trimmed.EndsWith(":") || 
                (trimmed.StartsWith("***") && trimmed.EndsWith("***")))
            {
                // Save previous section
                if (!string.IsNullOrEmpty(currentHeader))
                {
                    result[currentHeader] = body.ToString();
                }

                // Start new section
                currentHeader = trimmed;
                body.Clear();
            }
            else if (!string.IsNullOrEmpty(currentHeader))
            {
                // Build the body
                body.AppendLine(trimmed);
            }
        }

        // Save the last section
        if (!string.IsNullOrEmpty(currentHeader))
        {
            result[currentHeader] = body.ToString();
        }

        return result;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextSection();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PreviousSection();
        }
    }
}