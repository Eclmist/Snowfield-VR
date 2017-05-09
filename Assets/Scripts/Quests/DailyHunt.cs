using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyHunt : Hunt
{
    public DailyHunt(string name, JobType jobType, GameObject reward, Session dialog, int experience)
        : base(name,jobType, reward, dialog,experience)
    {
        DialogManager.Instance.DisplayDialogBox(dialog.Title);


    }
}
