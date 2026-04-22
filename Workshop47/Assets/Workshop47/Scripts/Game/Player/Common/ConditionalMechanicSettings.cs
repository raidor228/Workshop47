using System;
using UnityEngine;
using Melador.Utils;

namespace Melador.PlayerController.MovementController.Settings.Root
{
    [Serializable, ConditionalSettingsGroup]
    public class ConditionalMechanicSettings
    {
        [field: SerializeField] 
        public bool IsMechanicAllowed { get; set; } = true;
    }
}