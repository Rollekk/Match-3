using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject gemGO;
    [SerializeField] GameObject gemParent;
    public PlayerManager playerManager;

    [Header("Gem")]
    [SerializeField] GemSO[] gemStatsArray = null;
    public float gemSpacing = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 7; i++) CreateFirstGemRow(i);
    }

    void CreateFirstGemRow(int i)
    {
        GemController newGem = Instantiate(gemGO, transform.position + new Vector3(i * gemSpacing, 0.0f, 0.0f), gemGO.transform.rotation).GetComponent<GemController>();

        newGem.transform.parent = gemParent.transform;
        newGem.gemStats = gemStatsArray[(int)Random.Range(0, gemStatsArray.Length)];
        newGem.playerManager = this.playerManager;
        newGem.UpdateGameStats();
    }

    void CreateNewGem(Vector3 removedGemPosition)
    {
        GemController newGem = Instantiate(gemGO, new Vector3(removedGemPosition.x, transform.position.y, removedGemPosition.z), gemGO.transform.rotation).GetComponent<GemController>();

        newGem.transform.parent = gemParent.transform;
        newGem.gemStats = gemStatsArray[(int)Random.Range(0, gemStatsArray.Length)];
        newGem.playerManager = this.playerManager;
        newGem.UpdateGameStats();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Gem")) CreateNewGem(collision.transform.position);
    }

}
