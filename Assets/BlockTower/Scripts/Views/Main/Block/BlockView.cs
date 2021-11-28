using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace BlockTower.Views.Main.Block
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Sprite[] sprites;

        [Inject] private IObjectResolver _resolver;

        private Rigidbody2D _rigidbody;

        public void Init(float generateHeight)
        {
            var position = new Vector3(0, generateHeight, 0);
            transform.SetPositionAndRotation(
                position,
                Quaternion.identity
            );

            ResolveInjections();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _rigidbody.position = position;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            gameObject.AddComponent<PolygonCollider2D>();
        }
        
        private void ResolveInjections()
        {
            Resolve<BlockFaller>();
        }

        private void Resolve<T>()
        {
            var component = gameObject.GetComponent<T>();
            if (component == null) return;
            _resolver.Inject(component);
        }
    }
}
