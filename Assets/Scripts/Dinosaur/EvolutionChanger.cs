using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionChanger : MonoBehaviour
{
    [SerializeField] private List<Material> _skinMaterials;
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private List<GameObject> _stars;

    private DinosaurLevelManager _dinosaurLevelManager;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private int _currentSkin;
    private string _parrentName;

    private void Start()
    {
        _dinosaurLevelManager = GetComponentInParent<DinosaurLevelManager>();
        _parrentName = GetComponentInParent<Paddock>().gameObject.name;

        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        if(PlayerPrefs.HasKey("CurrentSkin" + _parrentName))
        {
            _currentSkin = PlayerPrefs.GetInt("CurrentSkin" + _parrentName);

            ChangeSkin(_currentSkin);
        }
        else
        {
            _currentSkin = 0;

            ChangeSkin(_currentSkin);
        }

        foreach (Button button in _buttons)
        {
            button.onClick.AddListener(delegate { ChangeSkin(_buttons.IndexOf(button)); });
        }
    }

    public void ChangeSkin(int index)
    {
        _currentSkin = index;

        PlayerPrefs.SetInt("CurrentSkin" + _parrentName, index);

        switch (index)
        {
            case 0:
                _dinosaurLevelManager.SetLevel(1);
                break;
            case 1:
                _dinosaurLevelManager.SetLevel(11);
                break;
            case 2:
                _dinosaurLevelManager.SetLevel(21);
                break;
            case 3:
                _dinosaurLevelManager.SetLevel(31);
                break;
        }

        if (!_skinMaterials.Contains(_skinMaterials[index]))
            return;

        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].interactable = true;
        }

        foreach (GameObject star in _stars)
        {
            star.gameObject.SetActive(false);
        }

        for (int i = 0; i <= index; i++)
        {
            if (_stars.Contains(_stars[i]))
            _stars[i].gameObject.SetActive(true);
        }

        _buttons[index].interactable = false;

        _skinnedMeshRenderer.material = _skinMaterials[index];
    }
}
