using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        if (entity != null)
            entity.OnFlipped += HandleFlip;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip()
    {
        // 1. 强制血条旋转为0，和角色世界坐标对齐
        transform.rotation = Quaternion.identity;
        // 2. 反转X坐标，抵消父物体180°旋转带来的位置偏移
        transform.localPosition = new Vector3(-transform.localPosition.x,transform.localPosition.y,transform.localPosition.z);
        // 3. 防止镜像
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
    }
}