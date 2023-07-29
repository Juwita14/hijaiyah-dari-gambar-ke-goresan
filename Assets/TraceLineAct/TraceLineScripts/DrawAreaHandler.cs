using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TraceLineManagerName;
using System;

namespace startPoints
{
    public class DrawAreaHandler : MonoBehaviour
    {
        private TraceLineManager traceLineManager;
        public bool isTitik;
        public StartPointHandler startPoint;
        public Transform endPoint;
        public Transform traceArea;
        public Transform traceLine;

        public void Activate(TraceLineManager _traceLineManager) {
            traceLineManager = _traceLineManager;
        }
        private void Awake() {
            Init();
            RegisterCollider();
            if(checkLists.Count!=0) isCheckeds = new bool[checkLists.Count];
        }

        void Init() {
            startPoint = transform.Find("StartPoint").GetComponent<StartPointHandler>();
            startPoint.Activate(this);
            endPoint = transform.Find("EndPoint");
            traceArea = transform.Find("TraceArea");
            traceLine = transform.Find("TraceLine");
        }

        [SerializeField] List<Collider2D> checkLists;

        private void RegisterCollider()
        {
            checkLists = new List<Collider2D>();

            if(GetComponents<Collider2D>() != null) {
                int colLen = GetComponents<Collider2D>().Length;
                for (int i = 0; i < colLen; i++)
                {
                    checkLists.Add(GetComponents<Collider2D>()[i]);
                }
            }
        }

        int currentColliderIndex = 0;
        bool[] isCheckeds;

        bool isFinished;

        public bool IsCorrectCheckPoint(Collider2D col) {
            if(!isFinished) {
                if(col == checkLists[currentColliderIndex]) {
                    isCheckeds[currentColliderIndex] = true;
                    Debug.Log($"checkpoin! {currentColliderIndex}/{checkLists.Count-1}");
                    if(currentColliderIndex>=checkLists.Count-1) {
                        isFinished = true;
                        Debug.Log("finish");
                        ResetChecker();
                    }
                    else {
                        currentColliderIndex++;
                    }
                    return true;
                }
                else {
                    Debug.Log($"Belum saatnya dik. index skrng:{currentColliderIndex}, yg km sntuh:{checkLists.IndexOf(col)}");
                    ResetChecker();
                    return false;
                }
            }
            ResetChecker();
            return false;
        }

        public void ResetChecker() {
            currentColliderIndex = 0;
            isFinished=false;
            for(int i=0; i<isCheckeds.Length; i++) {
                isCheckeds[i]=false;
            }
        }

        public void DestroyLine() {
            traceLineManager.DestoyLine();
        }

    }
}