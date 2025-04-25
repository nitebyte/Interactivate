using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace Nightbyte.Interactivate
{
    [AddComponentMenu("Nightbyte/Interactivate/Interactivate Caster")]
    public class InteractivateCaster : MonoBehaviour
    {
        #region RAYCAST
        [Header("Raycast")]
        public float rayDistance = 3f;
        public LayerMask layerMask = ~0;
        public QueryTriggerInteraction hitTriggers = QueryTriggerInteraction.Ignore;
        public float checkInterval = 0;

        Camera _cam;
        float  _timer;
        Interactivate _current;
        #endregion

        #region UI BINDINGS
        [System.Serializable]
        public class UIFieldBinding
        {
            public string fieldID = "ItemName";
            public List<GameObject> uiObjects = new();
            public bool updateText = true;
            [Header("Localisation")] public bool applyLocalization;

            [Header("Animation")]
            public bool useFade = true;
            public bool useScale = false;
            public float showDuration = .25f, hideDuration = .25f;
            public Ease  easeIn = Ease.OutQuad, easeOut = Ease.InQuad;
            public float showScale = 1f, hideScale = .8f;

            readonly Dictionary<GameObject, Vector3> _origScale = new();

            public void CacheOriginals()
            {
                foreach (var g in uiObjects)
                    if (g && !_origScale.ContainsKey(g))
                        _origScale[g] = g.transform.localScale;
            }
            public void HideImmediate() { foreach (var g in uiObjects) if (g) g.SetActive(false); }

            public void Show(string value)
            {
                if (applyLocalization) value = Localizer.Localize(value);

                foreach (var g in uiObjects)
                {
                    if (!g) continue;
                    KillTweens(g);
                    g.SetActive(true);

                    var cg = useFade ? GetOrAddCanvasGroup(g) : null;
                    if (useFade && cg) cg.DOFade(1f, showDuration).SetEase(easeIn);
                    if (useScale)      g.transform.DOScale(showScale, showDuration).SetEase(easeIn);
                    else if (_origScale.TryGetValue(g, out var sc)) g.transform.localScale = sc;

                    if (updateText)
                    {
                        var txt = g.GetComponentInChildren<TMP_Text>();
                        if (txt) txt.text = value;
                    }
                }
            }

            public void Hide()
            {
                foreach (var g in uiObjects)
                {
                    if (!g) continue;
                    KillTweens(g);

                    var seq = DOTween.Sequence().SetTarget(g);
                    bool any = false;

                    var cg = useFade ? GetOrAddCanvasGroup(g) : null;
                    if (useFade && cg) { seq.Join(cg.DOFade(0f, hideDuration).SetEase(easeOut)); any = true; }
                    if (useScale)      { seq.Join(g.transform.DOScale(hideScale, hideDuration).SetEase(easeOut)); any = true; }

                    if (any) seq.OnComplete(() => g.SetActive(false));
                    else      g.SetActive(false);
                }
            }

            #region helpers
            void KillTweens(GameObject g)
            {
                DOTween.Kill(g);
                DOTween.Kill(g.transform);
                var cg = g.GetComponent<CanvasGroup>();
                if (cg) DOTween.Kill(cg);
            }
            CanvasGroup GetOrAddCanvasGroup(GameObject g)
            {
                var cg = g.GetComponent<CanvasGroup>();
                if (!cg)
                {
                    bool wasInactive = !g.activeSelf;
                    if (wasInactive) g.SetActive(true);
                    cg = g.AddComponent<CanvasGroup>();
                    if (wasInactive) g.SetActive(false);
                }
                return cg;
            }
            #endregion
        }

        [Header("UI Bindings")] public List<UIFieldBinding> bindings = new();

        void Start()
        {
            _cam = GetComponent<Camera>() ? GetComponent<Camera>() : Camera.main;
            bindings.ForEach(b => { b.CacheOriginals(); b.HideImmediate(); });
        }
        #endregion

        #region UPDATE
        void Update()
        {
            _timer += Time.deltaTime;
            if (checkInterval > 0 && _timer < checkInterval) return;
            _timer = 0;

            RaycastHit hit;
            bool ok = Physics.Raycast(_cam.transform.position, _cam.transform.forward,
                                      out hit, rayDistance, layerMask, hitTriggers);

            var ia = ok ? hit.collider.GetComponentInParent<Interactivate>() : null;
            if (ia == _current) return;

            if (_current) { _current.SetHoverState(false); HideUI(); }

            _current = ia;
            if (_current && !_current.enabled) _current = null;

            if (_current)
            {
                _current.SetHoverState(true);
                ShowUI(_current);
            }
        }
        #endregion

        #region UI CONTROL
        void ShowUI(Interactivate ia)
        {
            foreach (var b in bindings)
            {
                string val = ia.GetDataField(b.fieldID);
                if (!string.IsNullOrEmpty(val)) b.Show(val); else b.Hide();
            }
        }
        void HideUI() { foreach (var b in bindings) b.Hide(); }
        #endregion
    }
}