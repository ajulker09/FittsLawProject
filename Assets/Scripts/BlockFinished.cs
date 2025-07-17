using UnityEngine;
using TMPro;
using System.Collections;

public class BlockFinished : MonoBehaviour
{
    public TMP_Text blockText;
    private int blockNumber = 1;
    private bool isCooldown = false;
    public GameObject spawner;
    public GameObject BfinishedUI;

    private void OnTriggerEnter(Collider other)
    {   

        BfinishedUI.SetActive(false);
        spawner.SetActive(true);   
        if (isCooldown) return;

        

        blockNumber++; 
        blockText.text = $"Block {blockNumber} Finished";

        isCooldown = true;
        StartCoroutine(ResetCooldown());
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(3f);
        isCooldown = false;
    }
}
