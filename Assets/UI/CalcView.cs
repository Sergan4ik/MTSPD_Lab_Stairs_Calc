using System;
using System.Collections.Generic;
using System.Linq;
using StairsCalc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalcView : MonoBehaviour
{
    public ValueSlider stepLength;
    public ValueSlider stepWidth;
    public ValueSlider stepHeight;
    public TMP_Dropdown stepMaterialDropdown, stepCoverMaterial;
    
    public TMP_Dropdown handrailTypeDropdown;
    public TMP_Dropdown handrailMaterialDropdown;
    public ValueSlider handrailLength;
    public ValueSlider handrailRadius;
    public ValueSlider handrailWidth;
    public ValueSlider handrailHeight;
    public GameObject handrailSquarePanel, HandrailCylinderPanel;
    
    public TMP_InputField handrailCount, stepsCount;
    
    public TMP_Dropdown currencyDropdown;

    public GridLayoutGroup resultsRoot;
    
    public StairsPreview stairsPreview;
    
    public Button calculateButton;
    public Button saveButton;

    private void Start()
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        calculateButton.onClick.RemoveAllListeners();
        saveButton.onClick.RemoveAllListeners();
        
        var materials = Resources.Load<MaterialDB>("Materials");
        var coverMats = Resources.Load<MaterialDB>("CoverMaterials");
        DBProvider dbProvider = new SOProvider(materials, coverMats);
        Handrail handrail = new HandrailCylinder()
        {
            radius = 0.05f, length = 1, materialId = materials.materials[0].Id
        };
        Step step = new Step() 
        {
            width = 1.5f, 
            length = 0.4f, 
            height = 0.3f, 
            materialId = materials.materials[0].Id, 
            coverMaterialId = coverMats.materials[0].Id
        };

        Project project = new Project(ref step, ref handrail, 10, 10, dbProvider);

        handrailTypeDropdown.onValueChanged.AddListener(value =>
        {
            if (value == 0)
            {
                handrail = new HandrailSquare()
                {
                    length = handrail.length,
                    materialId = handrail.materialId,
                    width = 0.1f,
                    height = 0.1f
                };
                project.handrailPrototype = handrail;
                ShowHandrailSetup(handrail, materials);
            }
            else
            {
                handrail = new HandrailCylinder()
                {
                    length = handrail.length,
                    materialId = handrail.materialId,
                    radius = 0.05f
                };
                project.handrailPrototype = handrail;
                ShowHandrailSetup(handrail, materials);
            }
        });
        
        handrailCount.onEndEdit.AddListener(c =>
        {
            if (int.TryParse(c, out var count) && count > 1)
            {
                project.handrailCount = count;
            }
            else 
            {
                handrailCount.text = project.handrailCount.ToString();
            }
        });
        
        stepsCount.onEndEdit.AddListener(c =>
        {
            if (int.TryParse(c, out var count) && count > 0)
            {
                project.stepCount = count;
            }
            else
            {
                stepsCount.text = project.stepCount.ToString();
            }
        });
        
        ShowStepSetup(step, materials, coverMats);
        ShowHandrailSetup(handrail, materials);
        
        handrailTypeDropdown.ClearOptions();
        handrailTypeDropdown.AddOptions(new List<string> {"Square", "Cylinder"});
        
        currencyDropdown.ClearOptions();
        currencyDropdown.AddOptions(Enum.GetNames(typeof(Currency)).ToList());
        
        SubscribeAllChanges(project, dbProvider);
        
        handrailTypeDropdown.value = 1;

        stepsCount.text = "10";
        handrailCount.text = "10";
        
        ShowResults(dbProvider, project);
        
        calculateButton.onClick.AddListener(() => ShowResults(dbProvider, project));
        saveButton.onClick.AddListener(() => project.GetReport("Rep", Enum.Parse<Currency>(currencyDropdown.options[currencyDropdown.value].text)).SaveToFile(
            #if UNITY_EDITOR
            "Assets/report.csv"
            #else
            "report.csv"
            #endif
            ));
    }

    private void SubscribeAllChanges(Project project, DBProvider provider)
    {
        stepLength.OnValueChanged += _ => ShowResults(provider, project);
        stepWidth.OnValueChanged += _ => ShowResults(provider, project);
        stepHeight.OnValueChanged += _ => ShowResults(provider, project);
        // stepMaterialDropdown.onValueChanged.AddListener(_ => ShowResults(provider, project));
        // stepCoverMaterial.onValueChanged.AddListener(_ => ShowResults(provider, project));
        
        handrailLength.OnValueChanged += _ => ShowResults(provider, project);
        handrailWidth.OnValueChanged += _ => ShowResults(provider, project);
        handrailHeight.OnValueChanged += _ => ShowResults(provider, project);
        handrailRadius.OnValueChanged += _ => ShowResults(provider, project);
        // handrailMaterialDropdown.onValueChanged.AddListener(_ => ShowResults(provider, project));
        
        // handrailCount.onEndEdit.AddListener(_ => ShowResults(provider, project));
        // stepsCount.onEndEdit.AddListener(_ => ShowResults(provider, project));
        
        // handrailTypeDropdown.onValueChanged.AddListener(_ => ShowResults(provider, project));
    }

    private void ShowStepSetup(Step step, MaterialDB materials, MaterialDB coverMats)
    {
        stepLength.Show("Step length, m", 0.1f, 10, step.length);
        stepWidth.Show("Step width, m", 0.1f, 10, step.width);
        stepHeight.Show("Step height, m", 0.1f, 10f, step.height);
        stepMaterialDropdown.ClearOptions();
        stepMaterialDropdown.AddOptions(materials.materials.ConvertAll(material => material.Id));
        stepMaterialDropdown.value = 0;
        stepCoverMaterial.ClearOptions();
        stepCoverMaterial.AddOptions(coverMats.materials.ConvertAll(material => material.Id));
        stepCoverMaterial.value = 0;
        
        SubscribeSteps(step, materials, coverMats);
    }

    private void SubscribeSteps(Step step, MaterialDB materials, MaterialDB coverMats)
    {
        stepLength.OnValueChanged += value => step.length = value;
        stepWidth.OnValueChanged += value => step.width = value;
        stepHeight.OnValueChanged += value => step.height = value;
        stepMaterialDropdown.onValueChanged.AddListener(value => step.materialId = materials.materials[value].Id);
        stepCoverMaterial.onValueChanged.AddListener(value => step.coverMaterialId = coverMats.materials[value].Id);
    }
    
    public void ShowHandrailSetup(Handrail handrail, MaterialDB materials)
    {
        handrailLength.Show("Handrail length, m", 0.1f, 10, handrail.length);
        //can change to reflection to avoid this strange stuff
        if (handrail is HandrailSquare handrailSquare)
        {
            handrailSquarePanel.SetActive(true);
            HandrailCylinderPanel.SetActive(false);
            handrailWidth.Show("Handrail width, m", 0.01f, 2f, handrailSquare.width);
            handrailHeight.Show("Handrail height, m", 0.01f, 2f, handrailSquare.height);
        }
        else
        {
            handrailSquarePanel.SetActive(false);
            HandrailCylinderPanel.SetActive(true);
            handrailRadius.Show("Handrail radius, m", 0.01f, 2f, ((HandrailCylinder)handrail).radius);
        }
        handrailMaterialDropdown.ClearOptions();
        handrailMaterialDropdown.AddOptions(materials.materials.ConvertAll(material => material.Id));
        handrailMaterialDropdown.value = 0;
        
        SubscribeHandrail(handrail, materials);
    }

    private void SubscribeHandrail(Handrail handrail, MaterialDB materials)
    {
        handrailLength.OnValueChanged += value => handrail.length = value;
        if (handrail is HandrailSquare handrailSquare)
        {
            handrailWidth.OnValueChanged += value => handrailSquare.width = value;
            handrailHeight.OnValueChanged += value => handrailSquare.height = value;
        }
        else
        {
            handrailRadius.OnValueChanged += value => ((HandrailCylinder)handrail).radius = value;
        }
        handrailMaterialDropdown.onValueChanged.AddListener(value => handrail.materialId = materials.materials[value].Id);
    }

    public void ShowResults(DBProvider provider, Project project)
    {
        var report = project.GetReport("Report", Enum.Parse<Currency>(currencyDropdown.options[currencyDropdown.value].text));
        CurrencyTools.TryGetCurrencySymbol(report.currency.ToString(), out var currencySymbol);
        float rate = GlobalEconomy.GetEconomy().GetRate(Currency.USD, report.currency);

        // structure same as report, get text field by index and set text
        GetOutputText(0, 0).text = "";
        GetOutputText(0, 1).text = "Volumes";
        GetOutputText(0, 2).text = "Masses";
        GetOutputText(0, 3).text = "Prices";
        
        GetOutputText(1, 0).text = "Step";
        GetOutputText(1, 1).text = $"{report.volumes.stepVolume:F2} m^3";
        GetOutputText(1, 2).text = $"{report.masses.stepMass:F2} kg";
        GetOutputText(1, 3).text = $"{(report.prices.stepPrice * rate):N0} {currencySymbol}";
        
        GetOutputText(2, 0).text = "Step cover";
        GetOutputText(2, 1).text = $"{report.volumes.stepCoverSquare:F2} m^2";
        GetOutputText(2, 2).text = $"{report.masses.stepCoverMass:F2} kg";
        GetOutputText(2, 3).text = $"{report.prices.stepCoverPrice * rate:N0} {currencySymbol}";
        
        GetOutputText(3, 0).text = "Handrail vertical";
        GetOutputText(3, 1).text = $"{report.volumes.handrailVerticalVolume:F2} m^3";
        GetOutputText(3, 2).text = $"{report.masses.handrailVerticalMass:F2} kg";
        GetOutputText(3, 3).text = $"{report.prices.handrailVerticalPrice * rate:N0} {currencySymbol}";
        
        GetOutputText(4, 0).text = "Handrail main";
        GetOutputText(4, 1).text = $"{report.volumes.handrailMainVolume:F2} m^3";
        GetOutputText(4, 2).text = $"{report.masses.handrailMainMass:F2} kg";
        GetOutputText(4, 3).text = $"{report.prices.handrailMainPrice * rate:N0} {currencySymbol}";
        
        GetOutputText(5, 0).text = "Total";
        GetOutputText(5, 1).text = "";
        GetOutputText(5, 2).text = $"{report.masses.stepMass + report.masses.stepCoverMass + report.masses.handrailVerticalMass + report.masses.handrailMainMass:F2} kg";
        GetOutputText(5, 3).text = $"{report.prices.totalPrice * rate:N0} {currencySymbol}";
        
        stairsPreview.Show(provider, project);
    }
    
    private TextMeshProUGUI GetOutputText(int x, int y) => resultsRoot.transform.GetChild(x * 4 + y).GetComponent<TextMeshProUGUI>();
}