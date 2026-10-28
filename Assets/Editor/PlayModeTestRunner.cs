using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Text;

namespace Unity.AI.Assistant.PlayModeTest
{
    [InitializeOnLoad]
    internal static class PlayModeTestRunner
    {
        private const string StateKey = "PlayModeTest.State";
        private const string ResultKey = "PlayModeTest.Result";
        private const string ScriptPathKey = "PlayModeTest.ScriptPath";

        static PlayModeTestRunner()
        {
            if (SessionState.GetString(StateKey, "Idle") == "WaitingForCompile")
            {
                EditorApplication.delayCall += () =>
                {
                    SessionState.SetString(StateKey, "EnteringPlayMode");
                    EditorApplication.isPlaying = true;
                };
            }
            
            if (EditorApplication.isPlaying && SessionState.GetString(StateKey, "Idle") == "EnteringPlayMode")
            {
                SessionState.SetString(StateKey, "InPlayMode");
                EditorApplication.update += RunTest;
            }
        }

        private static int _frame = 0;
        private static void RunTest()
        {
            _frame++;
            if (_frame < 10) return; // wait for init

            var player = GameObject.Find("Player");
            var anim = player.GetComponentInChildren<Animator>();
            var motor = player.GetComponent("ShadowSurfaceMotor");
            
            // Check Test A: Idle at start
            bool airborne = (bool)motor.GetType().GetProperty("IsAirborne").GetValue(motor);
            var state = anim.GetCurrentAnimatorStateInfo(0);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TEST A: Startup");
            sb.AppendLine("Airborne: " + airborne);
            sb.AppendLine("Current State: " + (state.IsName("Player_Idle") ? "Idle" : "Other (" + state.shortNameHash + ")"));
            
            SessionState.SetString(ResultKey, sb.ToString());
            SessionState.SetString(StateKey, "Done");
            EditorApplication.isPlaying = false;
        }

        private static void SelfDestruct()
        {
            string path = SessionState.GetString(ScriptPathKey, "");
            if (!string.IsNullOrEmpty(path)) AssetDatabase.DeleteAsset(path);
            SessionState.EraseString(StateKey);
        }
    }
}