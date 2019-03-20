Shader "Hidden/Subtract"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BufferTex("BufferTex", 2D) = "white" {}
        _Threshold("Threshold", Range(0, 1)) = 0.01
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            // インスペクタに表示したときにわかりやすいように名前を付けておく
            Name "Update"

            CGPROGRAM
            
            // UnityCustomRenderTexture.cgincをインクルードする
            #include "UnityCustomRenderTexture.cginc"

            // 頂点シェーダは決まったものを使う
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _BufferTex;
            float _Threshold;

            // v2f構造体は決まったものを使う
            half4 frag(v2f_customrendertexture i) : SV_Target
            {
                float2 uv = i.globalTexcoord;
                half4 col = tex2D(_MainTex, uv);
                half4 buffer = tex2D(_BufferTex, uv);
                half4 sub = pow(col - buffer, 5.0) * 20.0;
                half4 result = half4(0.0, 0.0, 0.0, 1.0);

                if (length(sub) > _Threshold) {
                    result = half4(1, 1, 1, 1);
                }

                return result;
            }

            ENDCG
        }
    }
}
