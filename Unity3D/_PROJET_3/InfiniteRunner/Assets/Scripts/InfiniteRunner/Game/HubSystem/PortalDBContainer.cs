using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BestTimeFromDB))]

public class PortalDBContainer : MonoBehaviour
{
    [SerializeField] private DBConstant.Time idFromDb;
    public DBConstant.Time IdFromDb { get { return idFromDb; } }

    [SerializeField] private GameObject BestTimePrefab;
    [SerializeField] private Transform BestTimeParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void SpawnBestTimePanel()
    {
        GameObject bestTimePanel = Instantiate(BestTimePrefab, BestTimeParent.transform);
        BestimeCanvasDecomposer canvasComposer = bestTimePanel.GetComponent<BestimeCanvasDecomposer>();
        DBUsers_TimeJoinTime dbInfos = GetComponent<BestTimeFromDB>().GetBestTimeInformations();
        List<int> fragmentsInfos = GetComponent<BestTimeFromDB>().GetFragmentsStats();

        canvasComposer.FragmentsNb.text = dbInfos.Fragments.ToString() + " fragment(s)";
        canvasComposer.TimeName.text = ("Époque ").ToUpper() + dbInfos.Time_Name.ToString().ToUpper();
        canvasComposer.BestTime.text = dbInfos.best_time.ToString() + " secondes";
        canvasComposer.TotalFragmentsFoundInTime.text = fragmentsInfos[0].ToString() + "/"+ fragmentsInfos[1].ToString();
        canvasComposer.Image.sprite = canvasComposer.ImagesSprite[(int)idFromDb-1];

        // Ajouter le total des fragments trouvés par les joueurs.
        
    }
}
