using UnityEngine;
using UnityEngine.Assertions;

namespace Oculus.Interaction
{
    [RequireComponent(typeof(LineRenderer))]
    public class RayInteractorDebugGizmos : MonoBehaviour
    {
        [SerializeField]
        private RayInteractor _rayInteractor;

        [SerializeField]
        private float _rayWidth = 0.01f;

        [SerializeField]
        private Color _normalColor = Color.white;

        [SerializeField]
        private Color _hoverColor = Color.white;

        [SerializeField]
        private Color _selectColor = Color.white;

        private LineRenderer _line;

        protected virtual void Start()
        {
            this.AssertField(_rayInteractor, nameof(_rayInteractor));

            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.startWidth = _rayWidth;
            _line.endWidth = _rayWidth;

            // unlit material so it always looks like a debug gizmo
            var mat = new Material(Shader.Find("Unlit/Color"));
            _line.material = mat;
        }

        private void LateUpdate()
        {
            if (_rayInteractor == null || _rayInteractor.State == InteractorState.Disabled)
            {
                _line.enabled = false;
                return;
            }

            // choose color by interactor state
            switch (_rayInteractor.State)
            {
                case InteractorState.Normal:
                    _line.material.color = _normalColor;
                    break;
                case InteractorState.Hover:
                    _line.material.color = _hoverColor;
                    break;
                case InteractorState.Select:
                    _line.material.color = _selectColor;
                    break;
            }

            _line.startWidth = _rayWidth;
            _line.endWidth = _rayWidth;

            // draw line between origin and end
            _line.enabled = true;
            _line.SetPosition(0, _rayInteractor.Origin);
            _line.SetPosition(1, _rayInteractor.End);
        }

        private void OnDisable()
        {
            if (_line != null)
            {
                _line.enabled = false; 
            }
        }

        #region Inject
        public void InjectAllRayInteractorDebugGizmos(RayInteractor rayInteractor)
        {
            InjectRayInteractor(rayInteractor);
        }

        public void InjectRayInteractor(RayInteractor rayInteractor)
        {
            _rayInteractor = rayInteractor;
        }
        #endregion
    }
}
