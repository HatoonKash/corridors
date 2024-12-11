using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetHandler : MonoBehaviour {

    [SerializeField]
    private NavigationController navigationController;
    [SerializeField]
    private TextAsset targetModelData;
    [SerializeField]
    private TMP_Dropdown targetDataDropdown;

    [SerializeField]
    private GameObject targetObjectPrefab;
    [SerializeField]
    private Transform[] targetObjectsParentTransforms;
    [SerializeField]
    private List<TargetFacade> currentTargetItems = new List<TargetFacade>();
    [SerializeField]
    private bool getDataFromJson=false;

    [SerializeField] private TeacherDataController teacherData;

    private int currentRoom = 0;
    private void Start() {
        GenerateTargetItems();
        FillDropdownWithTargetItems();
    }

    private void GenerateTargetItems() {
        if (getDataFromJson)
        {
            IEnumerable<Target> targets = GenerateTargetDataFromSource();
            foreach (Target target in targets)
            {
                currentTargetItems.Add(CreateTargetFacade(target));
            }
        }
        else 
        {
            currentTargetItems = GenerateTargetItemsFromScene();
        }
    }

    private List<TargetFacade> GenerateTargetItemsFromScene()
    {
        List<TargetFacade> targets = new List<TargetFacade>();

        foreach (var parent in targetObjectsParentTransforms)
        {
            for(int i = 0; i<parent.childCount; i++)
            {
                if(parent.GetChild(i).gameObject.GetComponent<TargetFacade>()!=null)
                    targets.Add(parent.GetChild(i).gameObject.GetComponent<TargetFacade>());
            }
        }
        return targets;
    }
    private IEnumerable<Target> GenerateTargetDataFromSource() {
        return JsonUtility.FromJson<TargetWrapper>(targetModelData.text).TargetList;
    }

    private TargetFacade CreateTargetFacade(Target target) {
        GameObject targetObject = Instantiate(targetObjectPrefab, targetObjectsParentTransforms[target.FloorNumber], false);
        targetObject.SetActive(true);
        targetObject.name = $"{target.FloorNumber} - {target.Name}";
        targetObject.transform.localPosition = target.Position;
        targetObject.transform.localRotation = Quaternion.Euler(target.Rotation);

        TargetFacade targetData = targetObject.GetComponent<TargetFacade>();
        targetData.Name = target.Name;
        targetData.FloorNumber = target.FloorNumber;

        return targetData;
    }

    private void FillDropdownWithTargetItems() {
        List<TMP_Dropdown.OptionData> targetFacadeOptionData =
            currentTargetItems.Select(x => new TMP_Dropdown.OptionData {
                text = $"{x.Name}"
                //text = $"{x.FloorNumber} - {x.Name}"
            }).ToList();

        targetDataDropdown.ClearOptions();
        targetDataDropdown.AddOptions(targetFacadeOptionData);
    }

    public void SetSelectedTargetPositionWithDropdown(int selectedValue)
    {
        
        string select =  targetDataDropdown.options[targetDataDropdown.value].text;
        
        if (select == "")
            return;
        
        string[] split = select.Split('-');
        currentRoom = Int32.Parse(split[1]);
        teacherData.CheckOnlineTeacher(split[0]);
       // selectedValue = Int32.Parse(split[1]);
      //  navigationController.TargetPosition = GetCurrentlySelectedTarget(currentRoom);
        
    }
    public void SetNavigation()
    {
        navigationController.TargetPosition = GetCurrentlySelectedTarget(currentRoom);
    }

    public void ResetNavigation()
    {
        navigationController.TargetPosition = Vector3.zero;
    }

    private Vector3 GetCurrentlySelectedTarget(int selectedValue) {
       // if (selectedValue >= currentTargetItems.Count) {
       //     return Vector3.zero;
      //  }

        foreach (var item in currentTargetItems)
        {
            if (selectedValue == Int32.Parse(item.Name))
            {
                return item.transform.position;
            }
        }


         return Vector3.zero;
    // return currentTargetItems[selectedValue].transform.position;
    }

    public TargetFacade GetCurrentTargetByTargetText(string targetText) {
        return currentTargetItems.Find(x =>
            x.Name.ToLower().Equals(targetText.ToLower()));
    }

    public void BtnBackClick()
    {
        SceneManager.LoadScene("firstPage1");
    }
}
