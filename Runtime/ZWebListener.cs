using UnityEngine;

namespace Zappar.Additional.VideoRecorder
{
    public abstract class ZWebListener : MonoBehaviour
    {
        public const string UnityObjectName = "_zWebglVideoListener";

        public abstract void MessageCallback(string message);
    }
}