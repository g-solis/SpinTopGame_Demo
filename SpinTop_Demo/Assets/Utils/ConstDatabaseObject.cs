using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConstDatabase", menuName = "ScriptableObjects/ConstDatabase", order = 0)]
public class ConstDatabaseObject : ScriptableObject
{
    public SoundDB SoundDatabase;
    public PrefabDB PrefabDatabase;
    [Space(10)]
    public AnimationCurve SmoothCurve;
    public AnimationCurve MontainCurve;
    public AnimationCurve PopUpCurve;
    public AnimationCurve OverSmoothCurve;
    public AnimationCurve ElasticPopUpCurve;
    public AnimationCurve FastSmoothCurve;
    public AnimationCurve FastAccelerationCurve;
    public AnimationCurve AnticipationCurve;
}
