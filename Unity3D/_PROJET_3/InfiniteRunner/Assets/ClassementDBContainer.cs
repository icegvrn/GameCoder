using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DBConstant;

public class ClassementDBContainer : MonoBehaviour
{
   [SerializeField] List<ClassementItem> items;
   [SerializeField] ClassementItem currentUserItem;

    private void Init()
    {
      

    }

    // Start is called before the first frame update
    void Start()
    {
  
            List<DBRank> ranking = ServiceLocator.Instance.GetService<UserSessionData>().GetAllUsersRanking();

            int i = 0;

            foreach (DBRank rank in ranking)
            {
                if (i < items.Count)
                {
                    items[i].Name.text = rank.Username;
                    items[i].Fragments.text = rank.TotalFragments.ToString();

                }

                if (rank.Username == ServiceLocator.Instance.GetService<SessionManager>().CurrentUser)
                {
                    currentUserItem.Rank.text = "Votre place : " + (i + 1).ToString() + ".";
                    currentUserItem.Name.text = rank.Username;
                    currentUserItem.Fragments.text = rank.TotalFragments.ToString();

                }

                i++;
            }

    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
