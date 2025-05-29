using System;
using RemotePlugin.Remote.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace RemotePlugin.Remote.Convert {
    public static class ResponseConvert {
        public static ResponseResult ToResponseResult(this UnityWebRequest.Result result) { 
            try {
                return (ResponseResult)Enum.ToObject(typeof(ResponseResult), result);
            } catch (InvalidCastException) {
                Debug.LogError($"can't cast {result} to {typeof(ResponseResult)}");
                throw;
            }
        }
    }
}
