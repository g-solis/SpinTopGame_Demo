using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDatabase
{
    public static AnimationCurve SmoothCurve
    {
        get
        {
            return accessDatabase.SmoothCurve;
        }
    }

    public static AnimationCurve MontainCurve
    {
        get
        {
            return accessDatabase.MontainCurve;
        }
    }

    public static AnimationCurve PopUpCurve
    {
        get
        {
            return accessDatabase.PopUpCurve;
        }
    }

    public static AnimationCurve OverSmoothCurve
    {
        get
        {
            return accessDatabase.OverSmoothCurve;
        }
    }

    public static AnimationCurve ElasticPopUpCurve
    {
        get
        {
            return accessDatabase.ElasticPopUpCurve;
        }
    }

    public static AnimationCurve FastSmoothCurve
    {
        get
        {
            return accessDatabase.FastSmoothCurve;
        }
    }

    public static AnimationCurve FastAccelerationCurve
    {
        get
        {
            return accessDatabase.FastAccelerationCurve;
        }
    }

    public static AnimationCurve AnticipationCurve
    {
        get
        {
            return accessDatabase.AnticipationCurve;
        }
    }

    public static SoundDB SoundDatabase
    {
        get
        {
            return accessDatabase.SoundDatabase;
        }
    }

    public static PrefabDB PrefabDB
    {
        get
        {
            return accessDatabase.PrefabDatabase;
        }
    }

    private static ConstDatabaseObject accessDatabase
    {
        get
        {
            if(database == null)
            {
                database = (Resources.Load("ConstDatabase") as ConstDatabaseObject);
            }

            return database;
        }
    }

    private static ConstDatabaseObject database = null;
}
