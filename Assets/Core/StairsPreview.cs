using System.Collections.Generic;
using Sirenix.OdinInspector;
using StairsCalc;
using UnityEngine;

public class StairsPreview : MonoBehaviour
{
    [AssetsOnly]
    public StepView stepViewPrefab;
    
    [AssetsOnly]
    public HandrailView handrailSqView;
    [AssetsOnly]
    public HandrailView handrailCyView;
    
    [ReadOnly]
    private List<StepView> stepViews = new List<StepView>();
    [ReadOnly]
    private List<HandrailView> handrailViews = new List<HandrailView>();
    
    public void Show(DBProvider provider, Project project)
    {
        //clear
        for (int i = 0; i < stepViews.Count; i++)
        {
            Destroy(stepViews[i].gameObject);
        }
        stepViews.Clear();
        
        for (int i = 0; i < project.stepCount; i++)
        {
            if (i >= stepViews.Count)
            {
                var stepView = Instantiate(stepViewPrefab, transform);
                stepViews.Add(stepView);
            }
            stepViews[i].Show(provider, project.stepPrototype, i);
        }
        
        //handrail
        for (int i = 0; i < handrailViews.Count; i++)
        {
            Destroy(handrailViews[i].gameObject);
        }
        handrailViews.Clear();
        
        for (int i = 0; i < project.handrailCount; i++)
        {
            var handrailViewInstanceLeft = Instantiate(project.handrailPrototype is HandrailSquare ? handrailSqView : handrailCyView, transform);
            handrailViews.Add(handrailViewInstanceLeft);
            handrailViewInstanceLeft.Show(provider, project, project.handrailPrototype, i, true);
            
            var handrailViewInstanceRight = Instantiate(project.handrailPrototype is HandrailSquare ? handrailSqView : handrailCyView, transform);
            handrailViews.Add(handrailViewInstanceRight);
            handrailViewInstanceRight.Show(provider, project, project.handrailPrototype, i, false);
        }
        
        //main handrail
        var mainHandrailView = Instantiate(project.mainHandrail is HandrailSquare ? handrailSqView : handrailCyView, transform);
        handrailViews.Add(mainHandrailView);
        mainHandrailView.ShowMain(provider, project, true);
        
        var mainHandrailViewRight = Instantiate(project.mainHandrail is HandrailSquare ? handrailSqView : handrailCyView, transform);
        handrailViews.Add(mainHandrailViewRight);
        mainHandrailViewRight.ShowMain(provider, project, false);
    }
    
}