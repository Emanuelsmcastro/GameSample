using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        NONE,
        COIN
    }
    public class CollactableBase : MonoBehaviour
    {
        [Header("Configs")]
        public string tagPlayer = "Player";
        public GameObject graphicItem;
        public Collider myCollider;
        public ItemType itemType;
        [Header("Particles")]
        public ParticleSystem myParticleSystem;
        [Header("Songs")]
        public SFXType mySFXType;
        public SFXPool mySFXPool;
        [Header("Animation")]
        public float animationDuration = 1f;
        public Ease ease = Ease.OutBack;
        public float rotateAmount = .5f;


        private Tween _currentTween;

        private void OnValidate()
        {
            myCollider= GetComponent<Collider>();
            mySFXPool = GetComponent<SFXPool>();
        }
        private void FixedUpdate()
        {
            graphicItem.transform.Rotate(new Vector3(0, rotateAmount, 0) * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag(tagPlayer))
            {
                Collect();
            }
        }

        public virtual void Collect()
        {
            if(myCollider != null) myCollider.enabled = false;  
            if (graphicItem != null && _currentTween == null)
            {
               _currentTween = graphicItem.transform.DOScale(0, animationDuration).SetEase(ease);
               _currentTween.onComplete = FinishedDoScale;
            }
            myCollider.enabled = false;

            OnCollect();
        }

        public virtual void OnCollect()
        {
            myParticleSystem.Emit(50);
            mySFXPool.Play(mySFXType);
        }

        private void FinishedDoScale()
        {
            graphicItem.SetActive(false);
        }
    }
}
