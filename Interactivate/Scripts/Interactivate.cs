// Interactivate.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [AddComponentMenu("Nightbyte/Interactivate"), RequireComponent(typeof(Collider))]
    public class Interactivate : MonoBehaviour
    {
        [System.Serializable] public class DataDisplayField { public string fieldID; [TextArea] public string value; }
        [Header("Dynamic Fields")] public List<DataDisplayField> dataFields = new();
        public string GetDataField(string id) => dataFields.Find(f => f.fieldID == id)?.value;
        public void SetDataField(string id, string v, bool add = true)
        {
            var f = dataFields.Find(d => d.fieldID == id);
            if (f != null) f.value = v;
            else if (add) dataFields.Add(new DataDisplayField { fieldID = id, value = v });
        }

        [System.Serializable]
        public class HoverOptions
        {
            public bool enableHover = true;

            [Header("Audio")] public bool enableHoverAudio; public List<AudioClip> hoverClips = new(); public bool hoverLoop; [Range(0, 1)] public float hoverVolume = 1; public float hoverFadeOut = .3f;
            [Header("FX Prefabs")] public bool enableHoverFX; public List<GameObject> hoverFXPrefabs = new();
        }
        [Header("Hover Options")] public HoverOptions hover = new();
        [Header("Hover Manipulations")] public List<HoverTransformPreset> hoverPresets = new();

        [System.Serializable] public class OutlineSettings { public bool enabled = true; [Range(0, 10)] public float thickness = 2; public Color colour = Color.white; }
        public OutlineSettings outline = new();
        const string OUTLINE_SHADER = "Nightbyte/Interactivate/Outline";
        readonly List<Material> outlineMats = new();
        static readonly int COLOR_ID = Shader.PropertyToID("_OutlineColor");
        static readonly int WIDTH_ID = Shader.PropertyToID("_OutlineWidth");
        static readonly HashSet<Mesh> processedMeshes = new();

        [Header("Trigger Sets")] public List<TriggerSet> triggerSets = new();

        bool isHovered;
        Coroutine hoverRoutine;
        readonly List<Tween> hoverTweens = new();
        AudioSource hoverSrc;

        readonly Dictionary<Transform, Vector3> baseLocalPos = new();
        readonly Dictionary<Transform, Vector3> baseWorldPos = new();
        readonly Dictionary<Transform, Vector3> baseLocalRot = new();
        readonly Dictionary<Transform, Vector3> baseWorldRot = new();
        readonly Dictionary<Transform, Vector3> baseScale = new();

        void Awake()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            PrepareOutline();

            foreach (var hp in hoverPresets)
                foreach (var t in hp.targets)
                    if (t && !baseScale.ContainsKey(t))
                    {
                        baseLocalPos[t] = t.localPosition; baseWorldPos[t] = t.position;
                        baseLocalRot[t] = t.localEulerAngles; baseWorldRot[t] = t.eulerAngles;
                        baseScale[t] = t.localScale;
                    }
        }
        void OnDestroy() { outlineMats.ForEach(m => { if (m) Destroy(m); }); }
        void OnDisable() { foreach (var t in baseScale.Keys) DOTween.Kill(t); hoverTweens.ForEach(t => t.Kill()); }

        void PrepareOutline()
        {
            if (!outline.enabled) return;
            var sh = Shader.Find(OUTLINE_SHADER); if (!sh) { outline.enabled = false; return; }
            void PrepMesh(Mesh m)
            {
                if (m == null || !processedMeshes.Add(m)) return;
                int v = m.vertexCount; var verts = m.vertices; var norms = m.normals;
                var smooth = new List<Vector3>(norms); var look = new Dictionary<Vector3, List<int>>();
                for (int i = 0; i < v; i++) { if (!look.TryGetValue(verts[i], out var lst)) { lst = new(); look[verts[i]] = lst; } lst.Add(i); }
                foreach (var ids in look.Values) { if (ids.Count < 2) continue; Vector3 a = Vector3.zero; foreach (int id in ids) a += norms[id]; a.Normalize(); foreach (int id in ids) smooth[id] = a; }
                m.SetUVs(3, smooth); if (m.subMeshCount > 1) return; var tris = m.triangles; m.subMeshCount++; m.SetTriangles(tris, m.subMeshCount - 1);
            }
            foreach (var f in GetComponentsInChildren<MeshFilter>(true)) PrepMesh(f.sharedMesh);
            foreach (var s in GetComponentsInChildren<SkinnedMeshRenderer>(true)) PrepMesh(s.sharedMesh);
            foreach (var r in GetComponentsInChildren<Renderer>(true))
            {
                var mat = new Material(sh) { hideFlags = HideFlags.HideAndDontSave };
                mat.SetColor(COLOR_ID, outline.colour); mat.SetFloat(WIDTH_ID, outline.thickness); outlineMats.Add(mat);
                var mats = r.sharedMaterials.ToList(); mats.Add(mat); r.sharedMaterials = mats.ToArray();
            }
            ToggleOutline(false);
        }
        void ToggleOutline(bool state) { if (!outline.enabled) return; float w = state ? outline.thickness : 0; outlineMats.ForEach(m => m.SetFloat(WIDTH_ID, w)); }

        void Update()
        {
            float dt = Time.deltaTime;
            foreach (var ts in triggerSets) { ts.TryAutoReset(); ts.ApplyValueLinked(); }
            if (!isHovered) return;
            foreach (var ts in triggerSets)
                if (ts.input.inputMode == InputMode.Button) HandleButton(ts, dt);
                else HandleAxis(ts);
        }

        public void SetHoverState(bool h)
        {
            if (isHovered == h) return;
            isHovered = h; ToggleOutline(h);
            if (!hover.enableHover) return;
            if (hoverRoutine != null) StopCoroutine(hoverRoutine);
            hoverRoutine = StartCoroutine(h ? HoverEnterRoutine() : HoverExitRoutine());
        }

        IEnumerator HoverEnterRoutine()
        {
            hoverTweens.ForEach(t => t.Kill()); hoverTweens.Clear();
            foreach (var hp in hoverPresets) hp.Apply(this, true);
            if (hover.enableHoverAudio && hover.hoverClips.Count > 0) PlayHoverAudio(true);
            if (hover.enableHoverFX && hover.hoverFXPrefabs.Count > 0) DoHoverFX(true);
            yield break;
        }
        IEnumerator HoverExitRoutine()
        {
            hoverTweens.ForEach(t => t.Kill()); hoverTweens.Clear();
            foreach (var hp in hoverPresets) hp.Apply(this, false);
            if (hover.enableHoverAudio) PlayHoverAudio(false);
            if (hover.enableHoverFX) DoHoverFX(false);
            yield break;
        }

        internal Vector3 GetBaseLocalPos(Transform t) => baseLocalPos.TryGetValue(t, out var v) ? v : t.localPosition;
        internal Vector3 GetBaseWorldPos(Transform t) => baseWorldPos.TryGetValue(t, out var v) ? v : t.position;
        internal Vector3 GetBaseLocalRot(Transform t) => baseLocalRot.TryGetValue(t, out var v) ? v : t.localEulerAngles;
        internal Vector3 GetBaseWorldRot(Transform t) => baseWorldRot.TryGetValue(t, out var v) ? v : t.eulerAngles;
        internal Vector3 GetBaseScale(Transform t) => baseScale.TryGetValue(t, out var v) ? v : t.localScale;
        public void TrackHoverTween(Tween tw) { if (tw != null) hoverTweens.Add(tw); }

        void PlayHoverAudio(bool enter)
        {
            hoverSrc = hoverSrc ? hoverSrc : GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
            if (enter)
            {
                var c = hover.hoverClips[Random.Range(0, hover.hoverClips.Count)];
                hoverSrc.clip = c; hoverSrc.volume = 0; hoverSrc.loop = hover.hoverLoop; hoverSrc.Play(); hoverSrc.DOFade(hover.hoverVolume, .1f);
            }
            else
            {
                if (hover.hoverFadeOut > 0) hoverSrc.DOFade(0, hover.hoverFadeOut).OnComplete(hoverSrc.Stop);
                else hoverSrc.Stop();
            }
        }
        void DoHoverFX(bool e) { foreach (var g in hover.hoverFXPrefabs) if (g) g.SetActive(e); }

        void HandleButton(TriggerSet ts, float dt)
        {
            var i = ts.input; bool key = Input.GetKey(i.activationKey);
            switch (i.buttonActivation)
            {
                case ButtonActivationMode.OnKeyDown: if (Input.GetKeyDown(i.activationKey)) { ts.Add(i.increment); Fire(ts); } break;
                case ButtonActivationMode.OnKeyUp: if (Input.GetKeyUp(i.activationKey)) { ts.Add(i.increment); Fire(ts); } break;
                case ButtonActivationMode.WhileKeyHeld:
                    if (key) ts.AddAxis(dt * i.increment);
                    if (key && (i.fireContinuous || Input.GetKeyDown(i.activationKey))) Fire(ts, true); break;
                case ButtonActivationMode.HoldForSeconds:
                    if (key) { i.holdTimer += dt; if (!i.holdFired && i.holdTimer >= i.holdDuration) { i.holdFired = true; ts.Add(i.increment); Fire(ts); } }
                    else { i.holdTimer = 0; i.holdFired = false; }
                    break;
                case ButtonActivationMode.Toggle:
                    if (Input.GetKeyDown(i.activationKey)) { i.toggleState = !i.toggleState; ts.Set(i.toggleState ? i.toggleOnValue : i.toggleOffValue); Fire(ts); }
                    break;
            }
        }
        void HandleAxis(TriggerSet ts)
        {
            var i = ts.input; float v = Input.GetAxis(i.axisName); if (Mathf.Approximately(v, 0)) return;
            if (i.invertAxis) v = -v; ts.AddAxis(v * i.axisIncrement);
        }

        void Fire(TriggerSet ts, bool cont = false)
        {
            if (!cont && ts.Maxed) return;
            if (!cont) { ts.usage.currentUses++; ts.usage.lastUse = Time.time; }
            ts.manipulationPresets.ForEach(p => p.Execute()); ts.fxPresets.ForEach(f => f.Execute()); ts.tagLayerChanges.ForEach(t => t.Execute()); ts.audioPresets.ForEach(a => a.Execute(gameObject)); ts.textPresets.ForEach(t => t.Execute()); ts.onActivateEvents?.Invoke();
            if (!cont && ts.Maxed) { if (ts.usage.removeOnExpire) gameObject.SetActive(false); if (ts.usage.destroyOnExpire) Destroy(gameObject); }
        }

        bool Find(string n, out TriggerSet ts) => (ts = triggerSets.Find(t => t.setName == n)) != null;
        public void SetKey(string n, KeyCode k) { if (Find(n, out var t)) t.input.activationKey = k; }
        public KeyCode GetKey(string n) => Find(n, out var t) ? t.input.activationKey : KeyCode.None;
        public void SetAxis(string n, string a) { if (Find(n, out var t)) t.input.axisName = a; }
        public string GetAxis(string n) => Find(n, out var t) ? t.input.axisName : "";
        public int GetValue(string n) => Find(n, out var t) ? t.value.intValue : 0;
        public void SetValue(string n, int v) { if (Find(n, out var t)) t.Set(v); }
    }
}