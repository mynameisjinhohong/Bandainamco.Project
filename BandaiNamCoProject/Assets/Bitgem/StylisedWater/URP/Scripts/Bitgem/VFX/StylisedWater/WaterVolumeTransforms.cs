#region Using statements

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;

#endregion

namespace Bitgem.VFX.StylisedWater
{

    [AddComponentMenu("Bitgem/Water  Volume (Transforms)")]
    public class WaterVolumeTransforms : WaterVolumeBase
    {
        [SerializeField] private float height;
        [SerializeField] private float width;
        [SerializeField] private float upPosY = 380f;
        [SerializeField] private float upTime = 120f;
        [SerializeField] private Bubble_LJH[] bubbles;
        [SerializeField] private GameObject parent;

        private float currTime;
        public bool isFinished = false;
        //[SerializeField] private float lerpTime;

        private Material myMaterial;
        private float originY;

        private void Start()
        {
            myMaterial = GetComponent<MeshRenderer>().sharedMaterial;
            originY = transform.position.y;
            //LerpRebuild(addHeight, lerpTime);
            //Rebuild(height,width);
        }


        private void Update()
        {
            currTime += Time.unscaledDeltaTime;
            myMaterial.SetFloat("_FixedDeltaTime", currTime);
        }

        public void StartBubble()
        {
            foreach(var b in bubbles)
            {
                b.StartBubble();
            }
        }

        public void FinishBubble()
        {
            foreach(var b in bubbles)
            {
                b.FinishBubble();
            }
        }

        public async void StartWave()
        {
            parent.SetActive(true);

            await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);

            float elapsedTime = 0f;
            while (elapsedTime < upTime)
            {
                if (isFinished) break;

                float lerpY = Mathf.Lerp(originY, upPosY, elapsedTime / upTime);
                transform.position = new Vector3(transform.position.x, lerpY, transform.position.z);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            //LerpRebuild(addHeight, lerpTime);
        }

        public async void FinishWave()
        {
            parent.SetActive(false);

            isFinished = true;

            float elapsedTime = 0f;
            float downTime = upTime / 2f;
            float currY = transform.position.y;
            while (elapsedTime < downTime)
            {
                float lerpY = Mathf.Lerp(currY, originY, elapsedTime / downTime);
                transform.position = new Vector3(transform.position.x, lerpY, transform.position.z);
                elapsedTime += Time.unscaledDeltaTime;
                await UniTask.Yield();
            }

            isFinished = false;
        }
        /*
                public void SendFinishWave()
                {
                    itemManager.SetWave(false);
                }*/

        #region MonoBehaviour events

        private void OnDrawGizmos()
        {
            if (!ShowDebug)
            {
                return;
            }

            // iterate the chldren
            for (var i = 0; i < transform.childCount; i++)
            {
                // grab the local position/scale
                var pos = transform.GetChild(i).localPosition;
                var sca = transform.GetChild(i).localScale / TileSize;

                // fix to the grid
                var x = Mathf.RoundToInt(pos.x / TileSize);
                var y = Mathf.RoundToInt(pos.y / TileSize);
                var z = Mathf.RoundToInt(pos.z / TileSize);

                var drawPos = new Vector3(x, y, z) * TileSize;
                var drawSca = new Vector3(Mathf.RoundToInt(sca.x), Mathf.RoundToInt(sca.y), Mathf.RoundToInt(sca.z)) * TileSize;
                drawPos += drawSca / 2f;
                drawPos += transform.position;
                drawPos -= new Vector3(TileSize, TileSize, TileSize);

                // render as wired volumes
                Gizmos.DrawWireCube(drawPos, drawSca);
            }
        }

        //private void OnTransformChildrenChanged()
        //{
        //    Rebuild();
        //}

        #endregion

        #region Public methods

        protected override void GenerateTiles(ref bool[,,] _tiles)
        {
            // iterate the chldren
            for (var i = 0; i < transform.childCount; i++)
            {
                // grab the local position/scale
                var pos = transform.GetChild(i).localPosition;
                var sca = transform.GetChild(i).localScale / TileSize;

                // fix to the grid
                var x = Mathf.RoundToInt(pos.x / TileSize);
                var y = Mathf.RoundToInt(pos.y / TileSize);
                var z = Mathf.RoundToInt(pos.z / TileSize);

                // iterate the size of the transform
                for (var ix = x; ix < x + Mathf.RoundToInt(sca.x); ix++)
                {
                    for (var iy = y; iy < y + Mathf.RoundToInt(sca.y); iy++)
                    {
                        for (var iz = z; iz < z + Mathf.RoundToInt(sca.z); iz++)
                        {
                            // validate
                            if (ix < 0 || ix >= MAX_TILES_X || iy < 0 | iy >= MAX_TILES_Y || iz < 0 || iz >= MAX_TILES_Z)
                            {
                                continue;
                            }

                            // add the tile
                            _tiles[ix, iy, iz] = true;
                        }
                    }
                }
            }
        }

        #endregion


    }
}