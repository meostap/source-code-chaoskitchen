using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCouterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;

    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }

}
