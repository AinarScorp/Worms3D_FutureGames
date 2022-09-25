using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSectionBtn : MonoBehaviour
{
    [SerializeField] GameObject _thisSection;
    [SerializeField] GameObject _nextSection;
    public void SwitchSections()
    {
        _thisSection.SetActive(false);
        _nextSection.SetActive(true);
    }
}
