using UnityEngine;

namespace Workshop47.Scripts.Game.Settlement.View.Characters
{
    public class CharacterBinder : MonoBehaviour
    {
        public void Bind(CharacterViewModel viewModel)
        {
            var position = viewModel.Position.CurrentValue;
            transform.position = position;

            var rotation = viewModel.Rotation.CurrentValue;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}