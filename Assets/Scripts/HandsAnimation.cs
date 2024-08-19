using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LeftHand : MonoBehaviour
{
    [SerializeField] private InputActionReference gripReference;
    [SerializeField] private InputActionReference triggerReference;
    [SerializeField] private Animator handAnimator;

    // Update is called once per frame
    void Update()
    {
        float gripValue = gripReference.action.ReadValue<float>();
        float triggerValue = triggerReference.action.ReadValue<float>();
        //Debug.Log(gripValue);
        handAnimator.SetFloat("Grip", gripValue);
        handAnimator.SetFloat("Trigger", triggerValue);
    }
}
