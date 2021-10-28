using UnityEngine;

public class SaveEnemy : SaveableObject
{
    [SerializeField] Transform[] patrolPoints;

    public Transform[] PatrolPoints { get => patrolPoints; }

    public override SaveableObjectData CreateSaveData()
    {
        EnemyData data = new EnemyData(gameObject, PrefabId, objectType, patrolPoints);

        return data;
    }

    public SaveEnemy()
    {
        objectType = PrefabType.Enemy;
    }
}

[System.Serializable]
public class EnemyData : SaveableObjectData
{
    [System.Serializable]
    struct PatrolPoint
    {
        [SerializeField] float localPosX;
        [SerializeField] float localPosY;

        public Vector2 GetLocalPos { get => new Vector2(localPosX, localPosY); }

        public PatrolPoint(Vector2 worldPos)
        {
            localPosX = worldPos.x;
            localPosY = worldPos.y;
        }
    }

    [SerializeField] PatrolPoint[] patrolPoints;

    public override void LoadAddinational(Transform parents, GameObject obj)
    {
        obj.transform.parent = parents;

        SaveEnemy enemy = obj.GetComponent<SaveEnemy>();

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            enemy.PatrolPoints[i].localPosition = patrolPoints[i].GetLocalPos;
        }
    }

    public EnemyData(GameObject obj, string id, PrefabType type, Transform[] patrol) : base(obj, id, type)
    {
        patrolPoints = new PatrolPoint[patrol.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i] = new PatrolPoint(patrol[i].localPosition);
        }
    }


}