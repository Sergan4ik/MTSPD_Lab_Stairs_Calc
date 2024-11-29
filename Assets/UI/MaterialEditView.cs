using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Material = StairsCalc.Material;

public class MaterialEditView : MonoBehaviour
{
    public GameObject matRoot;
    
    public Button save, toCalc, add, back;
    public TMP_InputField idInput;
    
    public MatView matPrefab;
    
    private List<MatView> _mats = new List<MatView>();
    
    public void Show(MaterialDB db)
    {
        gameObject.SetActive(true);
        
        foreach (var mat in _mats)
        {
            Destroy(mat.gameObject);
        }
        _mats.Clear();
        
        foreach (var mat in db.materials)
        {
            var matView = Instantiate(matPrefab, matRoot.transform);
            matView.Show(mat);
            _mats.Add(matView);
        }
        
        save.onClick.RemoveAllListeners();
        save.onClick.AddListener(() =>
        {
        });
        
        toCalc.onClick.RemoveAllListeners();
        toCalc.onClick.AddListener(() =>
        {
            FindObjectOfType<CalcView>(true).Show();
            gameObject.SetActive(false);
        });
        
        add.onClick.RemoveAllListeners();
        add.onClick.AddListener(() =>
        {
            if (string.IsNullOrEmpty(idInput.text))
            {
                return;
            }
            db.materials.Add(new Material(idInput.text, 0, 1));
            Show(db);
        });
        
        back.onClick.RemoveAllListeners();
        back.onClick.AddListener(() =>
        {
            FindObjectOfType<SignInView>(true).Show();
            gameObject.SetActive(false);
        });
    }
}