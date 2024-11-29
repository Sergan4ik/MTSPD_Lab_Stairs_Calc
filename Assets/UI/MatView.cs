using TMPro;
using UnityEngine;

public class MatView : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TMP_InputField priceText;
    public TMP_InputField densityText;

    public StairsCalc.Material shownMat;
    
    public void Show(StairsCalc.Material material)
    {
        shownMat = material;
        nameText.text = material.Id;
        priceText.text = material.price.ToString();
        densityText.text = material.density.ToString();
        
        priceText.onEndEdit.RemoveAllListeners();
        priceText.onEndEdit.AddListener((s) =>
        {
            if (float.TryParse(s, out var price) && price > 0)
            {
                material.price = price;
            }
            else {
                priceText.text = material.price.ToString();
            }
        });
        
        densityText.onEndEdit.RemoveAllListeners();
        densityText.onEndEdit.AddListener((s) =>
        {
            if (float.TryParse(s, out var density) && density > 0)
            {
                material.density = density;
            }
            else {
                densityText.text = material.density.ToString();
            }
        });
    }
}