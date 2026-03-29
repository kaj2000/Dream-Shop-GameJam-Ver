using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;    //DOTween
using System.Collections.Generic;

public class ChaosManager : MonoBehaviour
{
    public static ChaosManager Instance;
    [Header("Easter Egg")]
    public GameObject easterEggUIManager;
    [Header("BG Change")]
    public GameObject normalBG; 
    public GameObject dreamBG;  

    [Header("Floor Change")]
    public GameObject normalFloor; 
    public GameObject dreamFloor;

    [Header("Particles")]
    public ParticleSystem dreamParticles; 

  
    [Header("White Flash")]
    public Image flashImage; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ActivateUraPhase()
    {
        if (flashImage != null)
        {

            flashImage.gameObject.SetActive(true);
            flashImage.color = new Color(1, 1, 1, 0); // 从透明开始
            
            flashImage.DOFade(1f, 0.15f).OnComplete(() =>
            {
                ExecuteUraSwap();

                flashImage.DOFade(0f, 1.5f).OnComplete(() => 
                {
                    flashImage.gameObject.SetActive(false);
                });
            });
        }
        else
        {
            ExecuteUraSwap();
        }
    }


    void ExecuteUraSwap()
    {
        if (normalBG != null) normalBG.SetActive(false); 
        if (dreamBG != null) dreamBG.SetActive(true); 

        if (normalFloor != null) normalFloor.SetActive(false); 
        if (dreamFloor != null) dreamFloor.SetActive(true); 

        if (dreamParticles != null) dreamParticles.Play();

        

        if (easterEggUIManager != null)
    {
        easterEggUIManager.SetActive(true);
    }
    }

    
}